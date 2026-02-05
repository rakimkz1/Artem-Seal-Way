using System;
using UnityEngine;

[Serializable]
public class SealCameraHandler
{
    public float mouseSensivity;

    public float cameraMinRotate;
    public float cameraMaxRotate;
    public Action<Vector3> onDiraction;

    [SerializeField] private GameObject camera;
    [SerializeField] private Transform followTarget;
    private Vector3 _cameraRot;
    public void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        _cameraRot += new Vector3(-y * mouseSensivity * Time.deltaTime, x * mouseSensivity * Time.deltaTime, 0f);
        _cameraRot.x = Mathf.Clamp(_cameraRot.x, cameraMinRotate, cameraMaxRotate);

        camera.transform.rotation = Quaternion.Euler(_cameraRot);
        camera.transform.position = followTarget.position;

        onDiraction?.Invoke(Quaternion.Euler(_cameraRot) * Vector3.forward);
    }
}