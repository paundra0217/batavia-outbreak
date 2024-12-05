using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerMovement
{
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
            float xValue = context.ReadValue<Vector2>().x;
            float yValue = context.ReadValue<Vector2>().y;

            Vector2 newDirection = TranslateMovementDirection(xValue, yValue);
            xDirection = newDirection.x;
            zDirection = newDirection.y;
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

        private Vector2 TranslateMovementDirection(float xDir, float yDir)
        {
            MovementSetting currentSetting = CameraManager.GetMovementSettings();
            Vector2 newDirection = new Vector2();

            if (yDir > 0f)
            {
                // W key pressed
                switch (currentSetting.wDirection)
                {
                    case MovementDirection.PositiveX:
                        newDirection.x = 1f;
                        break;

                    case MovementDirection.NegativeX:
                        newDirection.x = -1f;
                        break;

                    case MovementDirection.PositiveZ:
                        newDirection.y = 1f;
                        break;

                    case MovementDirection.NegativeZ:
                        newDirection.y = -1f;
                        break;
                }
            }
            else if (yDir < 0f)
            {
                // S key pressed
                switch (currentSetting.sDirection)
                {
                    case MovementDirection.PositiveX:
                        newDirection.x = 1f;
                        break;

                    case MovementDirection.NegativeX:
                        newDirection.x = -1f;
                        break;

                    case MovementDirection.PositiveZ:
                        newDirection.y = 1f;
                        break;

                    case MovementDirection.NegativeZ:
                        newDirection.y = -1f;
                        break;
                }
            }
            else
            {
                var wDir = currentSetting.wDirection;
                var sDir = currentSetting.sDirection;

                if (
                    wDir == MovementDirection.PositiveX && sDir == MovementDirection.NegativeX ||
                    wDir == MovementDirection.NegativeX && sDir == MovementDirection.PositiveX
                    )
                {
                    newDirection.x = 0f;
                }
                else if (
                    wDir == MovementDirection.PositiveZ && sDir == MovementDirection.NegativeZ ||
                    wDir == MovementDirection.NegativeZ && sDir == MovementDirection.PositiveZ
                    )
                {
                    newDirection.y = 0f;
                }
            }

            if (xDir > 0f)
            {
                // D key pressed
                switch (currentSetting.dDirection)
                {
                    case MovementDirection.PositiveX:
                        newDirection.x = 1f;
                        break;

                    case MovementDirection.NegativeX:
                        newDirection.x = -1f;
                        break;

                    case MovementDirection.PositiveZ:
                        newDirection.y = 1f;
                        break;

                    case MovementDirection.NegativeZ:
                        newDirection.y = -1f;
                        break;
                }
            }
            else if (xDir < 0f)
            {
                // A key pressed
                switch (currentSetting.aDirection)
                {
                    case MovementDirection.PositiveX:
                        newDirection.x = 1f;
                        break;

                    case MovementDirection.NegativeX:
                        newDirection.x = -1f;
                        break;

                    case MovementDirection.PositiveZ:
                        newDirection.y = 1f;
                        break;

                    case MovementDirection.NegativeZ:
                        newDirection.y = -1f;
                        break;
                }
            }
            else
            {
                var aDir = currentSetting.aDirection;
                var dDir = currentSetting.dDirection;

                if (
                    aDir == MovementDirection.PositiveX && dDir == MovementDirection.NegativeX ||
                    aDir == MovementDirection.NegativeX && dDir == MovementDirection.PositiveX
                    )
                {
                    newDirection.x = 0f;
                }
                else if (
                    aDir == MovementDirection.PositiveZ && dDir == MovementDirection.NegativeZ ||
                    aDir == MovementDirection.NegativeZ && dDir == MovementDirection.PositiveZ
                    )
                {
                    newDirection.y = 0f;
                }
            }

            return newDirection;
        }
    }
}