using System.Collections.Generic;
using UnityEngine;
public class SealPhysicHandler
{
    public Vector3 gravitionForce;
    private Rigidbody _rb;
    private Vector3 _targetGravity;
    private float _gravityChangeSpeed = 10f;
    private List<WaterFlowZone> flowZoneList = new List<WaterFlowZone>();
    public void Init(Rigidbody rb)
    {
        _rb = rb;
        gravitionForce = Physics.gravity;
        _targetGravity = Physics.gravity;
    }
    public void FixedUpdate()
    {
        gravitionForce = Vector3.MoveTowards(gravitionForce, _targetGravity, _gravityChangeSpeed * Time.fixedDeltaTime);
        Vector3 flowDiraction = Vector3.zero;
        float flowForce = 0f;
        foreach (var flow in flowZoneList)
        {
            flowDiraction += flow.FlowDirection;
            flowForce += flow.FlowForce;
        }
        Vector3 totalFlow = (flowZoneList.Count != 0 ? (flowDiraction.normalized * (flowForce / flowZoneList.Count)) : Vector3.zero);
        _rb.AddForce(gravitionForce + totalFlow, ForceMode.Acceleration);
    }
    public void ChangeGravity(Vector3 targetGravity, float changeSpeed)
    {
        _targetGravity = targetGravity;
        _gravityChangeSpeed = changeSpeed;
    }
    public void AddFlowForce(WaterFlowZone zone)
    {
        flowZoneList.Add(zone);
    }
    public void RemoveFlowForce(WaterFlowZone zone)
    {
        flowZoneList.Remove(zone);
    }
}