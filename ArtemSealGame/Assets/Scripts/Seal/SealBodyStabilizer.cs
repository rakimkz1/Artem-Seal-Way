using System;
using UnityEngine;

[Serializable]
public class SealBodyStabilizer
{
    public float verticalRotateSpeed;
    public float velocityRotateSpeed;
    public float velocityProportion;
    public float maxAngleSpeed;

    private Rigidbody _rb;
    public void Init(Rigidbody rb)
    {
        _rb = rb;
    }
    public void RotateToVelocity()
    {
        Quaternion delta = Quaternion.LookRotation(_rb.linearVelocity, Vector3.up) * Quaternion.Inverse(_rb.transform.localRotation);

        if (delta.w < 0f)
            delta = new Quaternion(-delta.x, -delta.y, -delta.z, -delta.w);

        delta.ToAngleAxis(out float angleDeg, out Vector3 axis);

        if (angleDeg < 0.01f)
            return;

        float angleRad = angleDeg * Mathf.Deg2Rad;

        if (angleDeg > 180f)
            angleDeg -= 360f;

        Vector3 desiredAngularVelocity = axis * angleRad * velocityRotateSpeed;
        _rb.angularVelocity = Vector3.ClampMagnitude(_rb.angularVelocity + desiredAngularVelocity, Mathf.Min(velocityProportion * _rb.linearVelocity.magnitude, maxAngleSpeed));
    }
    public void VerticalStabilizer()
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

        Vector3 desiredAngularVelocity = axis * angleRad * verticalRotateSpeed;
        _rb.angularVelocity += desiredAngularVelocity;
    }
}