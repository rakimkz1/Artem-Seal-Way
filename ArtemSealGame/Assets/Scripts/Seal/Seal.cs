using UnityEngine;

public class Seal : MonoBehaviour, ISwimable, ISlidables, IFlowable
{
    public SealSwimingHandler swimingHandler;
    public SealCameraHandler cameraHandler;
    public SealWaterPhysicHandler waterHandler;
    public SealSurfaceMovementHandler surfaceMovementHandler;
    public SealVerticalStabilizer verticalStabilizer;
    public SealSlideHandler slideHandler;
    public SealPhysicHandler physicHandler; 

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
        physicHandler = new SealPhysicHandler();
        waterHandler = new SealWaterPhysicHandler();
        physicHandler.Init(_rb);
        waterHandler.Init(_rb, physicHandler);
        swimingHandler.Init(_rb);
        verticalStabilizer.Init(_rb);
        cameraHandler.Init(_rb);
        slideHandler.Init(_rb);
        surfaceMovementHandler.Init(_rb, slideHandler);
    }

    private void Update()
    {
        if (waterHandler.isWater)
        {
            swimingHandler.Update();
        }
        else
        {
            surfaceMovementHandler.Update();
            verticalStabilizer.Update();
        }
    }
    private void FixedUpdate()
    {
        physicHandler.FixedUpdate();
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

    public void OnFlowZoneEnter(WaterFlowZone flowZone) => physicHandler.AddFlowForce(flowZone);

    public void OnFlowZoneExit(WaterFlowZone flowZone) => physicHandler.RemoveFlowForce(flowZone);
}
