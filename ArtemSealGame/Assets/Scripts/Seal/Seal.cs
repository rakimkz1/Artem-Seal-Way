using UnityEngine;

public class Seal : MonoBehaviour, ISwimable, ISlidables
{
    public SealSwimingHandler swimingHandler;
    public SealCameraHandler cameraHandler;
    public SealWaterPhysicHandler waterHandler;
    public SealSurfaceMovementHandler surfaceMovementHandler;
    public SealVerticalStabilizer verticalStabilizer;
    public SealSlideHandler slideHandler;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        FacadInitialize();
        cameraHandler.onDiraction += swimingHandler.GetCameraDiraction;
        cameraHandler.onDiraction += surfaceMovementHandler.GetDiraction;
    }

    private void FacadInitialize()
    {
        waterHandler.Init(_rb);
        swimingHandler.Init(_rb);
        verticalStabilizer.Init(_rb);
        cameraHandler.Init(_rb);
        slideHandler.Init(_rb);
        surfaceMovementHandler.Init(_rb, slideHandler);
    }

    private void Update()
    {
        if (waterHandler.isWater) swimingHandler.Update();
        else
        {
            surfaceMovementHandler.Update();
            verticalStabilizer.Update();
        }
    }

    private void LateUpdate()
    {
        cameraHandler.Update();
        cameraHandler.CameraDistancingByVelocity(waterHandler.isWater);
    }

    public void EnterWater() => waterHandler.EnterWater();
    public void ExitWater() => waterHandler.ExitWater();

    public void OnSlideSurfaceEnter() => slideHandler.OnSlideEnter();

    public void OnSlideSurfaceExit() => slideHandler.OnSlideExit();
}
