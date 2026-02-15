using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SealSlideHandler
{
    public bool isSliding;
    [SerializeField] private PhysicsMaterial slidePhysicMaterial;
    private Vector3 _slideDiraction;
    private Rigidbody _rb;
    private Collider[] _colliders; 
    private List<SlideSurface> _slideSurfaceList = new List<SlideSurface>();

    public void Init(Rigidbody rb)
    {
        _rb = rb;
        _colliders = rb.gameObject.GetComponents<Collider>();
    }
    public void OnSlideEnter(SlideSurface sliderSurface)
    {
        _slideSurfaceList.Add(sliderSurface);

        if (_slideSurfaceList.Count == 1)
        {
            isSliding = true;
            _rb.angularDamping = 1.5f;
            _rb.linearDamping = 0f;
            foreach (var collider in _colliders)
                collider.material = slidePhysicMaterial;
        }
    }

    public void OnSlideExit(SlideSurface sliderSurface)
    {
        _slideSurfaceList.Remove(sliderSurface);

        if (_slideSurfaceList.Count == 0)
        {
            isSliding = false;
            _rb.angularDamping = 3f;
            _rb.linearDamping = 0.6f;
            foreach (var collider in _colliders)
                collider.material = null;
        }
    }
}