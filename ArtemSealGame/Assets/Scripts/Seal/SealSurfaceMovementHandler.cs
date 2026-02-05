using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

[Serializable]
public class SealSurfaceMovementHandler
{
    public float groundCheckDistance;
    public float pushingForce;
    public LayerMask groundLayerMask;
    public float pushColdown;

    private bool isGrounded = false;
    private bool isPushable = true;
    private Rigidbody _rb;
    private Vector3 _diraction;

    public void Init(Rigidbody rb)
    {
        _rb = rb;
    }

    public void Update()
    {
        GroundChecker();
        if (!isGrounded)
            return;

        if (Input.GetKeyDown(KeyCode.A) && isPushable)
            Push(_rb.transform.right + _rb.transform.position);
        if (Input.GetKeyDown(KeyCode.D) && isPushable)
            Push( -_rb.transform.right + _rb.transform.position);
    }

    private void Push(Vector3 axis)
    {
        _rb.AddForceAtPosition(new Vector3(_diraction.x, 0f, _diraction.z) * pushingForce, axis, ForceMode.Impulse);
        Debug.Log("push");
        PushingColdown();
    }

    private void GroundChecker()
    {
        Ray ray = new Ray(_rb.transform.position, Vector3.down);

        if (Physics.RaycastAll(ray, groundCheckDistance, (int)groundLayerMask).Length != 0)
            isGrounded = true;
        else
            isGrounded = false;
    }
    public void GetDiraction(Vector3 diraction) => _diraction = diraction;

    public async UniTask PushingColdown()
    {
        isPushable = false;
        await UniTask.Delay((int)(pushColdown * 1000f));
        isPushable = true;
    }
}