using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed;
    public float defaultSpeed = 5;
    public float runSpeed = 10;
    public float staminaCostRun = 1;
    public float jumpForce;
    public float wallSpeed = 3;

    private Vector2 curMovementInput;

    [Header("감지")]
    public LayerMask groundLayerMask;
    public LayerMask wallLayerMask;
    public LayerMask ropeLayerMask;
    public float groundRayLength;
    public float wallRayLength;

    [HideInInspector] public Rigidbody rigidbody;
    [HideInInspector] public bool isDoubleJump = false;
    [HideInInspector] public PlayerState playerState;

    [Header("로프")]
    public Vector3 pivot;
    public float maxRopeLength = 3f;
    public float ropeLength;
    public float gravity = 9.8f;
    private bool isRope;
    private Vector3 velocity = Vector3.zero;

    private Camera camera;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        moveSpeed = defaultSpeed;
        camera = Camera.main;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
    }

    private void Update()
    {
        if (playerState == PlayerState.Run && curMovementInput.magnitude > 0)
        {
            CharacterManager.Instance.Player.status.ReduceStamina(staminaCostRun * Time.deltaTime);
        }
        UpdateLineRenderer();
    }

    private void FixedUpdate()
    {
        if (isRope)
        {
            RopeAction();
        }
        else if (IsWall())
        {
            WallMove();
        }
        else
        {
            Move();
        }
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
        if (context.phase == InputActionPhase.Started)
        {
            Jump();
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (CharacterManager.Instance.Player.status.curStamina > 0)
            {
                playerState = PlayerState.Run;
            }
            else
            {
                playerState = PlayerState.Walk;
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            playerState = PlayerState.Walk;
        }

    }

    public void OnRopeShot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            RopeCheck();
        }
    }

    private void Move()
    {
        Vector3 dir = transform.right * curMovementInput.x + transform.forward * curMovementInput.y;

        if (playerState == PlayerState.Run)
        {
            if (CharacterManager.Instance.Player.status.curStamina > 0)
            {
                moveSpeed = runSpeed;
            }
            else
            {
                playerState = PlayerState.Walk;
            }
        }
        else if(playerState == PlayerState.Walk)
        {
            moveSpeed = defaultSpeed;
        }

        dir *= moveSpeed;
        dir.y = rigidbody.velocity.y;
        rigidbody.velocity = dir;
    }

    private void WallMove()
    {
        Vector3 dir = transform.right * curMovementInput.x + transform.up * curMovementInput.y;
        dir *= wallSpeed;
        rigidbody.velocity = dir;
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if (isRope)
        {
            isRope = false;
            Vector3 vector= rigidbody.velocity;
            vector.y = 0f;
            rigidbody.velocity = vector;
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if (isDoubleJump)
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isDoubleJump = false;
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
            if (Physics.Raycast(rays[i], groundRayLength, groundLayerMask))
            {
                Debug.Log("땅 감지");

                return true;
            }
        }

        return false;
    }

    private bool IsWall()
    {
        float radius = 0.4f;
        Vector3 center = transform.position + transform.up * 1.0f;

        Collider[] hits = Physics.OverlapSphere(center, radius, wallLayerMask);

        if (hits.Length > 0)
        {
            Debug.Log("벽 감지");
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3[] origins = new Vector3[4]
        {
            transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f),
            transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f),
            transform.position + (transform.right * 0.2f) + (transform.up * 0.01f),
            transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f)
        };

        foreach (Vector3 origin in origins)
        {
            Gizmos.DrawLine(origin, origin + Vector3.down * groundRayLength);
        }

        Vector3 center = transform.position + transform.up * 1.0f;
        Gizmos.DrawWireSphere(center, 0.4f);

        if (camera == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(camera.transform.position, camera.transform.position + camera.transform.forward * maxRopeLength);
    }

    private void RopeCheck()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRopeLength, ropeLayerMask))
        {
            isRope = true;
            pivot = hit.point;
            ropeLength = hit.distance;

            velocity = Vector3.zero;
        }
        else
        {
            isRope = false;
        }
    }

    private void RopeAction()
    {
        float curRopeLength = Vector3.Distance(transform.position, pivot);

        if (curRopeLength >= maxRopeLength || IsGrounded())
        {
            isRope = false;
            return;
        }

        Vector3 dir = (transform.position - pivot).normalized;

        Vector3 acceleration = -gravity * dir;

        velocity += acceleration * Time.fixedDeltaTime;

        Vector3 newPos = transform.position + velocity * Time.fixedDeltaTime;

        Vector3 offset = newPos - pivot;
        transform.position = pivot + offset.normalized * ropeLength;
    }

    private void UpdateLineRenderer()
    {
        if (isRope)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, pivot);          // 줄의 시작점
            lineRenderer.SetPosition(1, transform.position); // 줄의 끝점 (플레이어)
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
