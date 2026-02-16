using UnityEngine;

public class Seal : MonoBehaviour, ISwimable, ISlidables, IFlowable
{
    public SealSwimingHandler swimingHandler;
    public SealCameraHandler cameraHandler;
    public SealWaterPhysicHandler waterHandler;
    public SealSurfaceMovementHandler surfaceMovementHandler;
    public SealBodyStabilizer bodyStabilizer;
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
        bodyStabilizer.Init(_rb);
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
            //bodyStabilizer.VerticalStabilizer();
        }

        //if (surfaceMovementHandler.isGrounded == false && waterHandler.isWater == false || waterHandler.isWater == true && swimingHandler.GetMovementDiraction() == Vector3.zero)
        //    bodyStabilizer.RotateToVelocity();
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

    public void EnterWater(WaterZone waterZone) => waterHandler.EnterWater(waterZone);
    public void ExitWater(WaterZone waterZone) => waterHandler.ExitWater(waterZone);
    public void OnSlideSurfaceEnter(SlideSurface sliderSurface) => slideHandler.OnSlideEnter(sliderSurface);
    public void OnSlideSurfaceExit(SlideSurface sliderSurface) => slideHandler.OnSlideExit(sliderSurface);

    public void OnFlowZoneEnter(WaterFlowZone flowZone) => physicHandler.AddFlowForce(flowZone);

    public void OnFlowZoneExit(WaterFlowZone flowZone) => physicHandler.RemoveFlowForce(flowZone);
}
