using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float jump = 1f;

    private Rigidbody rb;
    private CharacterController controller;
    private PlayerInput playerInput;
    private float xDirection;
    private float zDirection;
    private float yMotion = 0f;
    private bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        playerInput.onActionTriggered += PlayerInput_onActionTriggered;
    }

    private void Update()
    {
        Vector3 motion3D = new Vector3(xDirection, 0, zDirection).normalized * speed;

        yMotion -= gravity * Time.deltaTime;
        if (controller.isGrounded)
        {
            yMotion = 0f;
            isJumping = false;
        }
        motion3D.y = yMotion;

        controller.Move(motion3D * Time.deltaTime);
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
}
