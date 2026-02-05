using UnityEngine;

public class Seal : MonoBehaviour, ISwimable
{
    public SealSwimingHandler swimingHandler;
    public SealCameraHandler cameraHandler;
    public SealWaterPhysicHandler waterHandler;
    public SealSurfaceMovementHandler surfaceMovementHandler;
    public SealVerticalStabilizer verticalStabilizer;

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
        surfaceMovementHandler.Init(_rb);
        verticalStabilizer.Init(_rb);
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
    }

    public void EnterWater() => waterHandler.EnterWater();
    public void ExitWater() => waterHandler.ExitWater();
}
