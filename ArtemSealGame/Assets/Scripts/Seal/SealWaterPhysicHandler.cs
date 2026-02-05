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
        _rb.angularDamping = 25f;
        _rb.linearDamping = 2.5f;
    }
    public void ExitWater()
    {
        isWater = false;
        _rb.useGravity = true;
        _rb.angularDamping = 3f;
        _rb.linearDamping = 1.0f;
    }
}