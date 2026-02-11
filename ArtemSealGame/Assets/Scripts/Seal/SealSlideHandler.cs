using System;
using UnityEngine;

[Serializable]
public class SealSlideHandler
{
    public bool isSliding;
    [SerializeField] private PhysicsMaterial slidePhysicMaterial;
    private Vector3 _slideDiraction;
    private Rigidbody _rb;
    private Collider[] _colliders; 

    public void Init(Rigidbody rb)
    {
        _rb = rb;
        _colliders = rb.gameObject.GetComponents<Collider>();
    }
    public void OnSlideEnter()
    {
        isSliding = true;
        _rb.angularDamping = 1.5f;
        _rb.linearDamping = 0f;
        foreach (var collider in _colliders)
            collider.material = slidePhysicMaterial;
    }

    public void OnSlideExit()
    {
        isSliding = false;
        _rb.angularDamping = 3f;
        _rb.linearDamping = 0.6f;
        foreach (var collider in _colliders)
            collider.material = null;
    }
}