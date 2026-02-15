using System;
using System.Collections.Generic;
using UnityEngine;

public class SealWaterPhysicHandler
{
    public bool isWater;
    private Rigidbody _rb;

    private Vector3 _targetGravition = Physics.gravity;
    private SealPhysicHandler _physicHandler;
    private List<WaterZone> _waterList = new List<WaterZone>();
    public void Init(Rigidbody rb, SealPhysicHandler physicHandler)
    {
        _rb = rb;
        _physicHandler = physicHandler;
    }

    public void EnterWater(WaterZone waterZone)
    {
        _waterList.Add(waterZone);
        if(_waterList.Count == 1)
        {
            isWater = true;
            _rb.mass = 1.0f;
            _physicHandler.ChangeGravity(Vector3.up * 0f, 6f);
            _rb.automaticCenterOfMass = true;
            _rb.angularDamping = 25f;
            _rb.linearDamping = 2.5f;
        }
    }
    public void ExitWater(WaterZone waterZone)
    {
        _waterList.Remove(waterZone);
        if(_waterList.Count == 0)
        {
            isWater = false;
            _rb.mass = 20f;
            _physicHandler.ChangeGravity(Physics.gravity, 12f);
            _rb.automaticCenterOfMass = false;
            _rb.centerOfMass = new Vector3(0f, -1f, -1.5f);
            _rb.angularDamping = 3f;
            _rb.linearDamping = 0.6f;
        }
    }
}