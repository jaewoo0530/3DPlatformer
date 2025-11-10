using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float defaultSpeed = 5;
    public float runSpeed = 10;
    public float staminaCostRun = 1;
    public float jumpForce;

    public LayerMask groundLayerMask;
    private Vector2 curMovementInput;
    private Vector2 mouseDelta;
    public float rayLength;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Rigidbody rigidbody;
    private bool isRun = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        moveSpeed = defaultSpeed;
    }

    private void Update()
    {
        if (isRun && curMovementInput.magnitude > 0)
        {
            CharacterManager.Instance.Player.status.ReduceStamina(staminaCostRun * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (CharacterManager.Instance.Player.status.stamina > 0)
            {
                moveSpeed = runSpeed;
                isRun = true;
            }
            else
            {
                moveSpeed = defaultSpeed;
                isRun = false;
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveSpeed = defaultSpeed;
            isRun = false;
        }
    }

    private void Move()
    {
        Vector3 dir = transform.right * curMovementInput.x + transform.forward * curMovementInput.y;
        dir *= moveSpeed;
        dir.y = rigidbody.velocity.y;
        rigidbody.velocity = dir;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], rayLength, groundLayerMask))
            {
                Debug.Log("¶¥ °¨Áö");
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3[] origins = new Vector3[4]
        {
            transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f),
            transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f),
            transform.position + (transform.right * 0.2f) + (transform.up * 0.01f),
            transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f)
        };

        foreach (Vector3 origin in origins)
        {
            Gizmos.DrawLine(origin, origin + Vector3.down * rayLength);
        }
    }
}
