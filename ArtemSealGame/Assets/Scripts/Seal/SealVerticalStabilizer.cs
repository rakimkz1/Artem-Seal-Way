using System;
using UnityEngine;

[Serializable]
public class SealVerticalStabilizer
{
    public float rotateSpeed;

    private Rigidbody _rb;
    public void Init(Rigidbody rb)
    {
        _rb = rb;
    }

    public void Update()
    {
        Quaternion delta = Quaternion.Euler(_rb.gameObject.transform.localEulerAngles.x, _rb.gameObject.transform.localEulerAngles.y, 0f) * Quaternion.Inverse(_rb.transform.localRotation);

        if (delta.w < 0f)
            delta = new Quaternion(-delta.x, -delta.y, -delta.z, -delta.w);

        delta.ToAngleAxis(out float angleDeg, out Vector3 axis);

        if (angleDeg < 0.01f)
            return;

        float angleRad = angleDeg * Mathf.Deg2Rad;

        if (angleDeg > 180f)
            angleDeg -= 360f;

        Vector3 desiredAngularVelocity = axis * angleRad * rotateSpeed;
        _rb.angularVelocity += desiredAngularVelocity;
    }
}