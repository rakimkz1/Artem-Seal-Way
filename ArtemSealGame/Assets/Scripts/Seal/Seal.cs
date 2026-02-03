using UnityEngine;

public class Seal : MonoBehaviour, ISwimable
{
    public SealSwimingHandler swimingHandler;
    public SealCameraHandler cameraHandler;
    public SealViewHandler viewHandler;
    public SealWaterPhysicHandler waterHandler;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        waterHandler.Init(_rb);
        swimingHandler.Init(_rb);
        cameraHandler.onDiraction += swimingHandler.GetCameraDiraction;
        swimingHandler.OnSwim += viewHandler.RotateHead;
    }
    private void Update()
    {
        swimingHandler.Update();
        cameraHandler.Update();
    }
    public void EnterWater() => waterHandler.EnterWater();

    public void ExitWater() => waterHandler.ExitWater();
}
