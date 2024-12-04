using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    public float speed = 5.0f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float jump = 1f;

    private Rigidbody rb;
    private CharacterController controller;
    private PlayerInput playerInput;
    private float xDirection;
    private float zDirection;
    private float yMotion = 0f;
    private bool isJumping = false;
    private float currentSpeed;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("PlayerCamera") != null)
        {
            CinemachineVirtualCamera playerVirtualCamera =
                GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<CinemachineVirtualCamera>();

            playerVirtualCamera.Follow = transform;
        }
        
        if (GameObject.FindGameObjectWithTag("MinimapCamera") != null)
        {
            CinemachineVirtualCamera mapVirtualCamera =
                GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<CinemachineVirtualCamera>();

            mapVirtualCamera.Follow = transform;
        }

        currentSpeed = speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        playerInput.onActionTriggered += PlayerInput_onActionTriggered;
    }

    private void Update()
    {
        Movement();
        Rotation();
    }

    private void Movement()
    {
        Vector3 motion3D = new Vector3(xDirection, 0, zDirection).normalized * currentSpeed;

        yMotion -= gravity * Time.deltaTime;
        if (controller.isGrounded)
        {
            yMotion = 0f;
            isJumping = false;
        }
        motion3D.y = yMotion;

        controller.Move(motion3D * Time.deltaTime);
    }

    private void Rotation()
    {
        //if (!MapCamera.IsInMiniMapMode()) return;

        Vector3 mouseTarget = GameObject.FindGameObjectWithTag("MouseTarget").transform.position;

        transform.LookAt(mouseTarget);
        transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
    }

    private void PlayerInput_onActionTriggered(InputAction.CallbackContext context)
    {
        Debug.Log(context);
    }

    public void Move(InputAction.CallbackContext context)
    {
        xDirection = context.ReadValue<Vector2>().x;
        zDirection = context.ReadValue<Vector2>().y;
    }

    public void Jump()
    {
        if (isJumping) return;

        yMotion = jump;
        isJumping = true;
    }

    public void SetCurrentSpeed(float speed)
    {
        this.currentSpeed = speed;
    }
}
