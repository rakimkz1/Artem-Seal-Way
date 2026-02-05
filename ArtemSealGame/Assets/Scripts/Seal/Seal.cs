using UnityEngine;

public class Seal : MonoBehaviour, ISwimable
{
    public SealSwimingHandler swimingHandler;
    public SealCameraHandler cameraHandler;
    public SealViewHandler viewHandler;
    public SealWaterPhysicHandler waterHandler;
    public SealSurfaceMovementHandler surfaceMovementHandler;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        waterHandler.Init(_rb);
        swimingHandler.Init(_rb);
        surfaceMovementHandler.Init(_rb);
        cameraHandler.onDiraction += swimingHandler.GetCameraDiraction;
        cameraHandler.onDiraction += surfaceMovementHandler.GetDiraction;
    }
    private void Update()
    {
        if (waterHandler.isWater) swimingHandler.Update();
        else surfaceMovementHandler.Update();
    }

    private void LateUpdate()
    {
        cameraHandler.Update();
    }
    public void EnterWater() => waterHandler.EnterWater();

    public void ExitWater() => waterHandler.ExitWater();
}
