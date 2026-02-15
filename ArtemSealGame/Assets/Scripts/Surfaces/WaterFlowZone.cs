using UnityEngine;

public class WaterFlowZone : MonoBehaviour
{
    public Vector3 FlowDirection;
    public float FlowForce;

    public void OnValidate()
    {
        FlowDirection = FlowDirection.normalized;
    }
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
        Gizmos.DrawLine(transform.position, transform.position + FlowDirection * FlowForce);
    }
}
