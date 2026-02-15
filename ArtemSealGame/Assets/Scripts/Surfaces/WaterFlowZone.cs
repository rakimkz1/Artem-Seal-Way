using UnityEngine;

public class WaterFlowZone : MonoBehaviour
{
    public Vector3 FlowForceDirection;
    public void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IFlowable flowable))
        {
            flowable.OnFlowZoneEnter(this);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IFlowable flowable))
        {
            flowable.OnFlowZoneExit(this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + FlowForceDirection);
    }
}
