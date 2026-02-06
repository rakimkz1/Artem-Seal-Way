using System;
using UnityEngine;

[Serializable]
public class SealCameraHandler
{
    public float mouseSensivity;

    public float cameraMinRotate;
    public float cameraMaxRotate;
    public AnimationCurve cameraDistancingCurve;
    public float cameraDefaultField;
    public Action<Vector3> onDiraction;

    [SerializeField] private GameObject cameraTarget;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform followTarget;
    private Vector3 _cameraRot;
    private Rigidbody _rb;

    public void Init(Rigidbody rb)
    {
        _rb = rb;
    }
    public void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        _cameraRot += new Vector3(-y * mouseSensivity * Time.deltaTime, x * mouseSensivity * Time.deltaTime, 0f);
        _cameraRot.x = Mathf.Clamp(_cameraRot.x, cameraMinRotate, cameraMaxRotate);

        cameraTarget.transform.rotation = Quaternion.Euler(_cameraRot);
        cameraTarget.transform.position = followTarget.position;
        CameraDistancingByVelocity();

        onDiraction?.Invoke(Quaternion.Euler(_cameraRot) * Vector3.forward);
    }

    private void CameraDistancingByVelocity()
    {
        float velocityMagnitude = _rb.linearVelocity.magnitude;
        camera.fieldOfView = cameraDefaultField * cameraDistancingCurve.Evaluate(velocityMagnitude);
    }
}