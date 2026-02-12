using Cysharp.Threading.Tasks.Triggers;
using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class SealCameraHandler
{
    public float mouseSensivity;

    public float cameraMinRotate;
    public float cameraMaxRotate;
    public AnimationCurve cameraDistancingCurve;
    public float cameraDefaultField;
    public float cameraFieldChangeSpeed;
    public Action<Vector3> onDiraction;

    [SerializeField] private GameObject cameraTarget;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform followTarget;
    [SerializeField] private LayerMask cameraMask;
    private float cameraTargetField;
    private Vector3 _cameraRot;
    private Vector3 _cameraInitialPos;
    private Rigidbody _rb;

    public void Init(Rigidbody rb)
    {
        _rb = rb;
        cameraTargetField = cameraDefaultField;
        _cameraInitialPos = camera.transform.localPosition;
    }
    public void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        _cameraRot += new Vector3(-y * mouseSensivity * Time.deltaTime, x * mouseSensivity * Time.deltaTime, 0f);
        _cameraRot.x = Mathf.Clamp(_cameraRot.x, cameraMinRotate, cameraMaxRotate);

        cameraTarget.transform.rotation = Quaternion.Euler(_cameraRot);
        cameraTarget.transform.position = followTarget.position;

        CheckCameraOverlap();
        
        onDiraction?.Invoke(Quaternion.Euler(_cameraRot) * Vector3.forward);
    }

    public void CameraDistancingByVelocity(bool isInWater)
    {
        float velocityMagnitude = _rb.linearVelocity.magnitude;
        cameraTargetField = cameraDefaultField * cameraDistancingCurve.Evaluate((isInWater ? velocityMagnitude : 0f));
        camera.fieldOfView = Mathf.MoveTowards(camera.fieldOfView, cameraTargetField, cameraFieldChangeSpeed * Time.deltaTime);
    }
    public void CheckCameraOverlap()
    {
        Ray ray = new Ray(cameraTarget.transform.position, cameraTarget.transform.rotation * _cameraInitialPos);
        Debug.DrawRay(cameraTarget.transform.position, cameraTarget.transform.rotation * _cameraInitialPos, Color.blue);
        RaycastHit[] hit = Physics.RaycastAll(ray, Vector3.Magnitude(_cameraInitialPos),cameraMask, QueryTriggerInteraction.Ignore);
        camera.transform.localPosition = _cameraInitialPos;
        foreach(var hitItem in hit)
        {
            if(hitItem.collider != null && hitItem.collider.gameObject.tag != "Player")
            {
                camera.transform.position = hitItem.point;
                break;
            }
        }
    }
}