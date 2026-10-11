using System;
using System.Collections;
using UnityEngine;

namespace Character
{
    public class PlayableCharacter : MonoBehaviour
    {
        [Header("Character Attributes")] 
        [SerializeField] private float walkSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float maxStamina;
        
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpCooldown;
        
        [Header("Player Settings")]
        [SerializeField] private float gamepadHorizontalSensitivity;
        [SerializeField] private float gamepadVerticalSensitivity;
        [SerializeField] private float mouseSensitivity;
        
        [Header("Component References")]
        [SerializeField] private Transform playerCamera;
        [SerializeField] private LayerMask groundLayerMask;
        
        private Rigidbody _rigidbody;
        
        public bool IsGrounded { get; private set; }
        public int staminaPercent => (int)(100 * _stamina/maxStamina);
        
        private Vector2 _movementInputVector;
        private Vector2 _gamepadLookInputVector;
        private Vector2 _mouseLookInputVector;
        private Vector3 _desiredDirection;
        private Vector3 _velocity;
        
        private bool _canJump = true;
        private bool _canSprint = true;
        private bool _jumpRequested;
        private bool _sprintRequested;

        public float _movementSpeed;
        private float _xRotation;
        private float _yawRotation;
        private const float CharacterHeight = 1.71f; // Average human height
        public float _stamina;
        

        /// <summary>
        /// Processes a movement request for the playable character by storing the specified
        /// movement input vector. This input is used to determine the direction
        /// in which the character moves.
        /// </summary>
        /// <param name="inputVector">
        /// A <see cref="Vector2"/> representing the character's movement input.
        /// The x component corresponds to lateral movement, and the y component
        /// corresponds to forward movement.
        /// </param>
        public void RequestMove(Vector2 inputVector)
        {
            _movementInputVector = inputVector;
        }


        /// <summary>
        /// Processes a look request for the playable character using gamepad input by storing the
        /// specified input vector. This input is used to determine changes in the character's
        /// orientation based on gamepad analog stick movements.
        /// </summary>
        /// <param name="inputVector">
        /// A <see cref="Vector2"/> representing the character's gamepad look input.
        /// The x component corresponds to horizontal rotation, and the y component
        /// corresponds to vertical rotation.
        /// </param>
        public void RequestGamepadLook(Vector2 inputVector)
        {
            _gamepadLookInputVector = inputVector;
        }


        /// <summary>
        /// Processes a look request for the playable character using mouse input by accumulating the
        /// specified input vector. This input is used to adjust the character's orientation based
        /// on mouse movements.
        /// </summary>
        /// <param name="inputVector">
        /// A <see cref="Vector2"/> representing the character's mouse look input.
        /// The x component corresponds to horizontal rotation, and the y component
        /// corresponds to vertical rotation.
        /// </param>
        public void RequestMouseLook(Vector2 inputVector)
        {
            _mouseLookInputVector += inputVector;
        }

        /// <summary>
        /// Processes a jump request for the playable character by setting the necessary flags
        /// to initiate a jump action. The jump will only be requested if the character is
        /// grounded and able to jump.
        /// </summary>
        public void RequestJump()
        {
            if (IsGrounded && _canJump)
            {
                _jumpRequested = true;
            }
        }

        /// <summary>
        /// Processes a sprint request for the playable character by setting the sprint state
        /// based on the specified input. This determines whether the character should
        /// be sprinting or not.
        /// </summary>
        /// <param name="isSprinting">
        /// A <see cref="bool"/> indicating whether sprinting is enabled. Pass true to
        /// request sprinting, and false to stop sprinting.
        /// </param>
        public void RequestSprint(bool isSprinting)
        {
            if (_canSprint)
            {
                _sprintRequested = isSprinting;
            }
            else
            {
                if (!isSprinting)
                {
                    _sprintRequested = false;
                }
            }
        }

        /// <summary>
        /// Handles the movement of the playable character based on the stored movement input.
        /// This involves calculating the desired direction relative to the camera's orientation,
        /// applying movement forces, and clamping the character's horizontal velocity.
        /// </summary>
        private void HandleMovement()
        {
            // 1. Get camera forward and right.
            // The camera has no local yaw, so the player's Rigidbody orientation is
            // the horizontal look direction. Use it for planar movement.
            Vector3 camForward = transform.forward;
            Vector3 camRight = transform.right;
            
            // 2. Compute movement direction from input (relative to camera orientation)
            _desiredDirection = camRight * _movementInputVector.x + camForward * _movementInputVector.y;
            
            // 3. Apply movement forces
            
            // 4. Clamp horizontal velocity
            _velocity = _desiredDirection.normalized * _movementSpeed;
            
            _rigidbody.linearVelocity = new Vector3(_velocity.x, _rigidbody.linearVelocity.y, _velocity.z);
        }

        /// <summary>
        /// Handles camera pitch. Rigidbody yaw is applied in FixedUpdate so that
        /// movement and rotation are both owned by the physics simulation.
        /// </summary>
        private void HandleCameraPitch()
        {
            float gamepadY = _gamepadLookInputVector.y * gamepadVerticalSensitivity * Time.deltaTime;
            float mouseY = _mouseLookInputVector.y * mouseSensitivity;
            _mouseLookInputVector.y = 0f;

            // Vertical rotation (camera only)
            _xRotation -= gamepadY + mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

            playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }

        /// <summary>
        /// Controls the horizontal body rotation of the playable character
        /// based on input from both gamepad and mouse. Gamepad input is treated
        /// as a rate of rotation, while mouse input is treated as a displacement.
        /// The resulting rotation is applied to the character's rigidbody.
        /// </summary>
        private void HandleBodyRotation()
        {
            // Gamepad input is a rate; mouse input is already a displacement.
            float gamepadYaw = _gamepadLookInputVector.x * gamepadHorizontalSensitivity * Time.fixedDeltaTime;
            float mouseYaw = _mouseLookInputVector.x * mouseSensitivity;
            _mouseLookInputVector.x = 0f;

            _yawRotation += gamepadYaw + mouseYaw;
            _rigidbody.MoveRotation(Quaternion.Euler(0f, _yawRotation, 0f));
        }

        /// <summary>
        /// Executes the jump action for the character by applying an upward force
        /// to the character's Rigidbody. Disables jumping for a cooldown period
        /// after executing the action.
        /// </summary>
        private void HandleJump()
        {
            _canJump  = false;
            
            // Reset Y velocity so jump is consistent
            _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);
            
            _rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            
            StartCoroutine(ResetJumpCoroutine());
        }
        
        private void CheckIsGrounded()
        {
            IsGrounded = Physics.Raycast(transform.position, Vector3.down,  CharacterHeight * 0.5f + 0.3f,  groundLayerMask);
        }
        
        private IEnumerator ResetJumpCoroutine()
        {
            yield return new WaitForSeconds(jumpCooldown);
            _canJump = true;
        }

        private void UpdateMovementSpeed()
        {
            if (!IsGrounded)
            {
                return;
            }
            
            _movementSpeed = _sprintRequested ? sprintSpeed : walkSpeed;
        }
        
        private void Awake()
        {
            _rigidbody =  GetComponent<Rigidbody>();
            if (!_rigidbody)
            {
                Debug.LogError("Rigid body not found");
            }
            _rigidbody.freezeRotation = true;
            _yawRotation = transform.eulerAngles.y;
        }

        private void Start()
        {
            _stamina = maxStamina;
        }

        private void Update()
        {
            CheckIsGrounded();
            
            if (_jumpRequested)
            {
                _jumpRequested = false;
                
                HandleJump();
            }

            UpdateMovementSpeed();

            if (_sprintRequested)
            {
                _stamina = Mathf.Clamp(_stamina - Time.deltaTime, 0, maxStamina);
                if (_stamina <= 0)
                {
                    _canSprint = false;
                    _sprintRequested = false;
                    // PlayerSounds.Instance.PlayOutOfBreath();
                }
            }
            else
            {
                _stamina = Mathf.Clamp(_stamina + Time.deltaTime, 0, maxStamina);
                if(!_canSprint && _stamina >= 2)
                {
                    _canSprint = true;
                }
            }
            
            HandleCameraPitch();
        }

        private void FixedUpdate()
        {
            HandleBodyRotation();
            HandleMovement();
        }
    }
}
