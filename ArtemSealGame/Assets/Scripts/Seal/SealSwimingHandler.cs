using System;
using UnityEngine;

[Serializable]
public class SealSwimingHandler
{
    public float acelaration;
    public float maxSpeed;
    public event Action<Vector3> OnSwim;

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
        Vector3 diraction = Vector3.ClampMagnitude(new Vector3(x, 0f, y), 1f);
        diraction = Quaternion.LookRotation(_cameraDiraction, Vector3.up) * diraction;
        _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity + diraction * acelaration * Time.deltaTime, maxSpeed);

        if (x != 0f || y != 0f)
            OnSwim?.Invoke(diraction);
    }

    public void GetCameraDiraction(Vector3 diraction) => _cameraDiraction = diraction;
}