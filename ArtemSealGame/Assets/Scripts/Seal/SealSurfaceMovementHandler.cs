using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

[Serializable]
public class SealSurfaceMovementHandler
{
    public float groundCheckDistance;
    public float pushingMovementVelocity;
    public float pushingAngulerVelocity;
    public float slicePushingMovementVelocity;
    public float slicePushingAngulerVelocity;
    public float slicingMaxVelocity;
    public LayerMask groundLayerMask;
    public float pushColdown;

    private Vector3 boxCenter = new Vector3(0f, -1f, -1f);
    private Vector3 boxScale = new Vector3(0.7f, 0.5f, 2.5f);
    public bool isGrounded { get; set; }
    private bool isLeftPushable = true;
    private bool isRightPushable = true;
    private Rigidbody _rb;
    private SealSlideHandler _slideHandler;
    private Vector3 _diraction;

    public void Init(Rigidbody rb, SealSlideHandler slideHandler)
    {
        _rb = rb;
        _slideHandler = slideHandler;
    }

    public void Update()
    {
        GroundChecker();
        if (!isGrounded)
            return;

        Debug.DrawRay(_rb.position + _rb.rotation * _rb.centerOfMass, Vector3.up * 2f, Color.green);

        if (Input.GetKeyDown(KeyCode.A) && isLeftPushable)
        {
            Push((Vector3.down * 90f) * Mathf.Deg2Rad);
            PushingColdown(false);
        }
        if (Input.GetKeyDown(KeyCode.D) && isRightPushable)
        {
            Push((Vector3.up * 90f) * Mathf.Deg2Rad);
            PushingColdown(true);
        }
    }

    private void Push(Vector3 axis)
    {
        Vector3 movementDiraction = _rb.rotation * Vector3.forward;
        movementDiraction.y = 0;
        movementDiraction = movementDiraction.normalized;

        if (_slideHandler.isSliding == false)
        {
            _rb.linearVelocity = movementDiraction * pushingMovementVelocity;
            _rb.angularVelocity = axis * pushingAngulerVelocity; ;
        }
        else
        {
            _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity + movementDiraction * slicePushingMovementVelocity, slicingMaxVelocity);
            _rb.angularVelocity = axis * slicePushingAngulerVelocity;
        }
    }

    private void GroundChecker()
    {
        if (Physics.CheckBox(boxCenter + _rb.transform.position, boxScale, _rb.rotation, groundLayerMask))
            isGrounded = true;
        else
            isGrounded = false;
    }
    public async UniTask PushingColdown(bool isRight)
    {
        if (isRight) isRightPushable = false;
        else isLeftPushable = false;

        await UniTask.Delay((int)(pushColdown * 1000f));

        if (isRight) isRightPushable = true;
        else isLeftPushable = true;
    }
    public void GetDiraction(Vector3 diraction) => _diraction = diraction;
}