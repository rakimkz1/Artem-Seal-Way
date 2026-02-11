using System;
using UnityEngine;

[Serializable]
public class SealWaterPhysicHandler
{
    public bool isWater;
    private Rigidbody _rb;
    public void Init(Rigidbody rb)
    {
        _rb = rb;
    }

    public void EnterWater()
    {
        isWater = true;
        _rb.useGravity = false;
        _rb.mass = 1.0f;
        _rb.automaticCenterOfMass = true;
        _rb.angularDamping = 25f;
        _rb.linearDamping = 2.5f;
    }
    public void ExitWater()
    {
        isWater = false;
        _rb.useGravity = true;
        _rb.mass = 20f;
        _rb.automaticCenterOfMass = false;
        _rb.centerOfMass = new Vector3(0f, -1f, -1.5f);
        _rb.angularDamping = 3f;
        _rb.linearDamping = 0.6f;
    }
}