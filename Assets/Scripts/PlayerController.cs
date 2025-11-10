using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;

    public LayerMask groundLayerMask;
    public float rayLength;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
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
