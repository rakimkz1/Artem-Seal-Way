using System;
using UnityEngine;

[Serializable]
public class SealSwimingHandler
{
    public float acelaration;
    public float sprintAcelaration;
    public float maxSpeed;
    public float maxSprintSpeed;
    public float maxRotateSpeed;
    public float rotationGain = 5f;
    public bool isSprinting;

    private Vector3 _cameraDiraction;
    private Rigidbody _rb;
    public void Init(Rigidbody rb)
    {
        _rb = rb;
    }
    public void Update()
    {
        Swiming();
    }

    private void Swiming()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
            isSprinting = true;
        else
            isSprinting = false;

        Vector3 diraction = Vector3.ClampMagnitude(new Vector3(x, 0f, y), 1f);
        
        if (diraction == Vector3.zero)
            return;

        diraction = Quaternion.LookRotation(_cameraDiraction, Vector3.up) * diraction;

        if (isSprinting == false)
            _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity + diraction * acelaration * Time.deltaTime, maxSpeed);
        else
            _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity + diraction * sprintAcelaration * Time.deltaTime, maxSprintSpeed);

        Debug.DrawRay(_rb.transform.position, diraction * 20f);

        RotateBody(diraction);
    }

    private void RotateBody(Vector3 diraction)
    {
        Quaternion delta = Quaternion.LookRotation(diraction, Vector3.up) * Quaternion.Inverse(_rb.rotation);

        if (delta.w < 0f)
            delta = new Quaternion(-delta.x, -delta.y, -delta.z, -delta.w);

        delta.ToAngleAxis(out float angleDeg, out Vector3 axis);

        if (angleDeg < 0.01f)
            return;
        
        float angleRad = angleDeg * Mathf.Deg2Rad;

        if (angleDeg > 180f)
            angleDeg -= 360f;

        Vector3 desiredAngularVelocity = axis * angleRad * rotationGain;
        _rb.angularVelocity = Vector3.ClampMagnitude(desiredAngularVelocity, maxRotateSpeed);
    }

    public void GetCameraDiraction(Vector3 diraction) => _cameraDiraction = diraction;
}