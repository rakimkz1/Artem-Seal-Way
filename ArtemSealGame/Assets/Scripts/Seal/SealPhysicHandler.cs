using UnityEngine;
public class SealPhysicHandler
{
    public Vector3 gravitionForce;
    private Rigidbody _rb;
    private Vector3 _targetGravity;
    private float _gravityChangeSpeed = 10f;
    public void Init(Rigidbody rb)
    {
        _rb = rb;
        gravitionForce = Physics.gravity;
        _targetGravity = Physics.gravity;
    }
    public void FixedUpdate()
    {
        gravitionForce = Vector3.MoveTowards(gravitionForce, _targetGravity, _gravityChangeSpeed * Time.fixedDeltaTime);
        _rb.AddForce(gravitionForce, ForceMode.Acceleration);
    }
    public void ChangeGravity(Vector3 targetGravity, float changeSpeed)
    {
        _targetGravity = targetGravity;
        _gravityChangeSpeed = changeSpeed;
    }
}