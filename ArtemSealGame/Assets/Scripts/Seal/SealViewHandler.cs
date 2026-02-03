using System;
using UnityEngine;

[Serializable]
public class SealViewHandler
{
    public float rotateSpeed;

    public Transform headView;
    public void RotateHead(Vector3 diraction)
    {
        headView.rotation = Quaternion.RotateTowards(headView.rotation, Quaternion.LookRotation(diraction, Vector3.up), rotateSpeed * Time.deltaTime);
    }
}