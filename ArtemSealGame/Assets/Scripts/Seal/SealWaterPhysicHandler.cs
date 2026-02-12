using System;
using UnityEngine;

[Serializable]
public class SealWaterPhysicHandler
{
    public bool isWater;
    public float changingSpeed;
    public float waterGravityChangeSpeed;
    private Rigidbody _rb;

    private Vector3 _targetGravition = Physics.gravity;
    private SealPhysicHandler _physicHandler;
    public void Init(Rigidbody rb, SealPhysicHandler physicHandler)
    {
        _rb = rb;
        _physicHandler = physicHandler;
    }

    public void EnterWater()
    {
        isWater = true;
        _rb.mass = 1.0f;
        _physicHandler.ChangeGravity(Vector3.up * 0f, 6f);
        _rb.automaticCenterOfMass = true;
        _rb.angularDamping = 25f;
        _rb.linearDamping = 2.5f;
    }
    public void ExitWater()
    {
        isWater = false;
        _rb.mass = 20f;
        _physicHandler.ChangeGravity(Physics.gravity, 12f);
        _rb.automaticCenterOfMass = false;
        _rb.centerOfMass = new Vector3(0f, -1f, -1.5f);
        _rb.angularDamping = 3f;
        _rb.linearDamping = 0.6f;
    }
    public void Update()
    {
        _physicHandler.gravitionForce = Vector3.MoveTowards(_physicHandler.gravitionForce, _targetGravition, changingSpeed * Time.deltaTime);
    }
}