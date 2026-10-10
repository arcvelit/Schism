using System.Collections;
using UnityEngine;

namespace Character
{
    public class PlayableCharacter : MonoBehaviour
    {
        [Header("Character Attributes")] 
        [SerializeField] private float walkSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float horizontalSensitivity;
        [SerializeField] private float verticalSensitivity;
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpCooldown;
        
        [Header("Component References")]
        [SerializeField] private Transform playerCamera;
        [SerializeField] private LayerMask groundLayerMask;
        
        private Rigidbody _rigidbody;
        
        public bool IsGrounded { get; private set; }
        
        private Vector2 _movementInputVector;
        private Vector2 _lookInputVector;
        private Vector3 _desiredDirection;
        private Vector3 _velocity;
        
        private bool _canJump = true;
        private bool _jumpRequested;
        private bool _sprintRequested;

        public float _movementSpeed;
        private float _xRotation;
        private const float CharacterHeight = 1.71f; // Average human height

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
        /// Processes a look request for the playable character by storing the specified
        /// look input vector. This input is used to control the character's viewing direction
        /// or camera orientation.
        /// </summary>
        /// <param name="inputVector">
        /// A <see cref="Vector2"/> representing the character's look input.
        /// The x component corresponds to horizontal rotation, and the y component
        /// corresponds to vertical rotation.
        /// </param>
        public void RequestLook(Vector2 inputVector)
        {
            _lookInputVector = inputVector;
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
            _sprintRequested = isSprinting;
        }
        
        private void HandleMovement()
        {
            // 1. Get camera forward and right, ignoring the vertical component (so you only move horizontally)
            Vector3 camForward = playerCamera.forward;
            Vector3 camRight = playerCamera.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();
            
            // 2. Compute movement direction from input (relative to camera orientation)
            _desiredDirection = camRight * _movementInputVector.x + camForward * _movementInputVector.y;
            
            // 3. Apply movement forces
            
            // 4. Clamp horizontal velocity
            _velocity = _desiredDirection.normalized * _movementSpeed;
            
            _rigidbody.linearVelocity = new Vector3(_velocity.x, _rigidbody.linearVelocity.y, _velocity.z);
        }

        /// <summary>
        /// Handles the character's camera and body rotation based on the stored look input vector.
        /// This includes vertical rotation for the camera and horizontal rotation for the character's body,
        /// adjusted by sensitivity.
        /// </summary>
        private void HandleLook()
        {
            // Multiply by sensitivity and frame time
            float mouseX = _lookInputVector.x * horizontalSensitivity * Time.deltaTime;
            float mouseY = _lookInputVector.y * verticalSensitivity * Time.deltaTime;

            // Vertical rotation (camera only)
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

            playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

            // Horizontal rotation (player body)
            transform.Rotate(Vector3.up * mouseX);
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
            
            HandleLook();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }
    }
}