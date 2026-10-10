using System.Collections;
using UnityEngine;

namespace Character
{
    public class PlayableCharacter : MonoBehaviour
    {
        [Header("Character Attributes")] 
        [SerializeField] private float walkSpeed;
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
        
        private float _xRotation;
        private const float CharacterHeight = 1f;
        
        public void RequestMove(Vector2 inputVector)
        {
            _movementInputVector = inputVector;
        }

        public void RequestLook(Vector2 inputVector)
        {
            _lookInputVector = inputVector;
        }
        
        public void RequestJump()
        {
            if (IsGrounded && _canJump)
            {
                _jumpRequested = true;
            }
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
            _velocity = _desiredDirection.normalized * walkSpeed;
            
            _rigidbody.linearVelocity = new Vector3(_velocity.x, _rigidbody.linearVelocity.y, _velocity.z);
        }

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
            IsGrounded = Physics.Raycast(transform.position, Vector3.down,  CharacterHeight + 0.3f,  groundLayerMask);
        }
        
        private IEnumerator ResetJumpCoroutine()
        {
            yield return new WaitForSeconds(jumpCooldown);
            _canJump = true;
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
            
            HandleLook();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }
    }
}