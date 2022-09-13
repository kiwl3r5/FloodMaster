using Script.Manager;
using UnityEngine;

namespace Script.Player
{
    public class PlayerLocomotion : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private AnimatorManager _animatorManager;
        private InputManager _inputManager;
        private Vector3 _moveDirection;
        private GameObject _cameraObj;
        private Rigidbody _playerRigidbody;

        [Header("Falling")]
        public float inAirTimer;
        public float leapingVelocity;
        public float fallingVelocity;
        public float rayCastHighOffSet = 0.5f;
        public LayerMask groundLayer;
        
        [Header("Movement Flags")]
        public bool isGrounded;
        public bool isJumping;
        
        [Header("Move Speed")]
        public float movementSpeed = 2f;
        public float sprintingSpeed = 7f;
        public float rotationSpeed = 20;

        [Header("Jump")]
        public float jumpHeight = 3;
        public float gravityIntensity = -15;
        public float jumpCount = 1;


        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        
        private static PlayerLocomotion _instance;
        public static PlayerLocomotion Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            } else {
                _instance = this;
            }
            _playerManager = GetComponent<PlayerManager>();
            _animatorManager = GetComponent<AnimatorManager>();
            _inputManager = GetComponent<InputManager>();
            _playerRigidbody = GetComponent<Rigidbody>();
            _cameraObj = GameObject.FindWithTag("LockCam");
            Debug.Assert(_cameraObj != null,"_cameraObj != null");
        }
        
        private void Start()
        {
            if (!GameManager.Instance.superSpeed.isOn) return;
            movementSpeed *= 3;
            sprintingSpeed *= 3;
            if (!GameManager.Instance.superJump.isOn) return;
            jumpHeight *= 4;
        }

        public void HandleAllMovement()
        {
            HandleFallingAndLanding();
            if (_playerManager.isInteracting) 
                return;
            HandleMovement();
            HandleRotation();
        }

        private void HandleMovement()
        {
            if(isJumping)
                return;
            _moveDirection = _cameraObj.transform.forward * _inputManager.verticalInput;
            _moveDirection += _cameraObj.transform.right * _inputManager.horizontalInput;
            _moveDirection.Normalize();
            _moveDirection.y = 0;
            if (_inputManager.moveAmount > 0.5f)
            {
                _moveDirection *= sprintingSpeed;
            }
            else
            {
                _moveDirection *= movementSpeed;   
            }

            var movementVelocity = _moveDirection;
            _playerRigidbody.velocity = movementVelocity;
        }

        private void HandleRotation()
        {
            if (isJumping)
                return;
            
            var targetDirection = 
                CameraRelativeVectorFromInput(_inputManager.horizontalInput, _inputManager.verticalInput);
            //var targetDirection = _cameraObj.forward * _inputManager.verticalInput;
            //targetDirection += _cameraObj.right * _inputManager.horizontalInput;
            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero)
                targetDirection = transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            transform.rotation = playerRotation;
        }
        
        private Vector3 CameraRelativeVectorFromInput(float x, float y)
        {
            var up = transform.up;
            var forward = Vector3.ProjectOnPlane(_cameraObj.transform.forward, up).normalized;
            var right = Vector3.Cross(up, forward);

            return x * right + y * forward;
        }

        private void HandleFallingAndLanding()
        {
            var position = transform.position;
            var rayCastOrigin = position;
            rayCastOrigin.y += rayCastHighOffSet;
            var targetPosition = position;

            if (!isGrounded && !isJumping)
            {
                if (!_playerManager.isInteracting)
                {
                    _animatorManager.PlayTargetAnimation("Falling",true);
                }

                inAirTimer += Time.deltaTime * 1.5f;
                _playerRigidbody.AddForce(transform.forward * leapingVelocity);
                _playerRigidbody.AddForce(-Vector3.up * (fallingVelocity * inAirTimer));
            }

            if (Physics.SphereCast(rayCastOrigin,0.2f,-Vector3.up,out var hit,groundLayer))
            {
                if (!isGrounded && !_playerManager.isInteracting)
                {
                    _animatorManager.PlayTargetAnimation("Land",true);
                }

                var rayCasHitPoint = hit.point;
                targetPosition.y = rayCasHitPoint.y;
                inAirTimer = 0;
                isGrounded = true;
                jumpCount = 1;
            }
            else
            {
                isGrounded = false;
            }

            if (!isGrounded || isJumping) return;
            if (_playerManager.isInteracting || _inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }

        public void HandleJumping()
        {
            if (jumpCount <= 0 || isJumping) return;
            _animatorManager.animator.SetBool(IsJumping,true);
            _animatorManager.PlayTargetAnimation("Jump",false);

            jumpCount--;
            var jumpingVelocity = Mathf.Sqrt(-2f * gravityIntensity * jumpHeight);
            var playerVelocity = _moveDirection;
            playerVelocity.y = jumpingVelocity;
            _playerRigidbody.velocity = playerVelocity;
        }
    }
}
