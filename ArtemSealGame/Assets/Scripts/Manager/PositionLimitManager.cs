using System.Collections.Generic;
using UnityEngine;

public class PositionLimitManager : MonoBehaviour
{
    public Transform[] limitObjects;
    public Vector3 minVectorLimits;
    public Vector3 maxVectorLimits;

    private List<Vector3> initialObjectPosition = new List<Vector3>();
    private void Start()
    {
        for(int i = 0;  i < limitObjects.Length; i++)
            initialObjectPosition.Add(limitObjects[i].localPosition);
    }
    private void LateUpdate()
    {
        for(int i = 0; i  < limitObjects.Length; i++)
        {
            Vector3 limited = Vector3.zero;
            limited.x = Mathf.Clamp(limitObjects[i].localPosition.x, initialObjectPosition[i].x + minVectorLimits.x, initialObjectPosition[i].x + maxVectorLimits.x);
            limited.y = Mathf.Clamp(limitObjects[i].localPosition.y, initialObjectPosition[i].y + minVectorLimits.y, initialObjectPosition[i].y + maxVectorLimits.y);
            limited.z = Mathf.Clamp(limitObjects[i].localPosition.z, initialObjectPosition[i].z + minVectorLimits.z, initialObjectPosition[i].z + maxVectorLimits.z);
            limitObjects[i].localPosition = limited;
        }
    }
}
