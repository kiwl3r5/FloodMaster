using System;
using Script.Manager;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Player
{
    public class PlayerLocomotion : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private AnimatorManager _animatorManager;
        private InputManager _inputManager;
        [SerializeField]private Vector3 _moveDirection;
        private Camera _cameraObj;
        public Rigidbody playerRigidbody;

        [Header("Falling")]
        public float inAirTimer;
        public float leapingVelocity;
        public float fallingVelocity;
        public float rayCastHighOffSet = 0.5f;
        public LayerMask groundLayer;
        
        [Header("Movement Flags")]
        public bool isGrounded;
        public bool isJumping;
        public bool isDriving;
        public bool isStunt;
        public float stuntDuration = 2f;
        private float _defaultStuntDuration;
        private bool _outOfWater;

        [Header("Move Speed")]
        public float movementSpeed = 2f;
        public float sprintingSpeed = 7f;
        public float rotationSpeed = 20;
        [SerializeField] private float defaultMoveSpeed;
        public float defaultSprintSpeed;

        [Header("Jump")]
        public float jumpHeight = 3;
        public float gravityIntensity = -15;
        public float jumpCount = 1;


        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private static readonly int IsDriving = Animator.StringToHash("IsDriving");
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
            playerRigidbody = GetComponent<Rigidbody>();
            _cameraObj = Camera.main;
            defaultMoveSpeed = movementSpeed;
            defaultSprintSpeed = sprintingSpeed;
            _defaultStuntDuration = stuntDuration;
            Debug.Assert(_cameraObj != null,"_cameraObj != null");
            _outOfWater = false;
        }
        
        private void Start()
        {
            /*if (!GameManager.Instance.superSpeed.isOn) return;
            movementSpeed *= 3;
            sprintingSpeed *= 3;*/
            if (!GameManager.Instance.superJump.isOn) return;
            jumpHeight *= 4;
        }

        public void HandleAllMovement()
        {
            if (isDriving)
            {
                return;
            }
            HandleFallingAndLanding();
            if (_playerManager.isInteracting) return;
            if (isStunt)
            {
                if (GameManager.Instance.isInvincibleCheatOn)
                {
                    goto skip;
                }
                stuntDuration -= Time.deltaTime;
                if (stuntDuration>0)
                {
                    playerRigidbody.velocity = Vector3.zero;
                    return;
                }
                skip:
                stuntDuration = _defaultStuntDuration;
                isStunt = false;
            }
            MovementSpeed();
            HandleMovement();
            HandleRotation();
        }

        private void MovementSpeed()
        {
            if (_playerManager.isBoots || _outOfWater)
            {
                sprintingSpeed = defaultSprintSpeed;
                movementSpeed = defaultMoveSpeed;
                return;
            }
            var floodSys = FloodSystem.Instance;
            switch (floodSys.floodPoint)
            {
                case > 75:
                    sprintingSpeed = defaultSprintSpeed*0.4f;
                    movementSpeed = defaultMoveSpeed*0.4f;
                    break;
                case > 50:
                    sprintingSpeed = defaultSprintSpeed*0.6f;
                    movementSpeed = defaultMoveSpeed*0.6f;
                    break;
                case > 25:
                    sprintingSpeed = defaultSprintSpeed*0.8f;
                    movementSpeed = defaultMoveSpeed*0.8f;
                    break;
                default:
                    sprintingSpeed = defaultSprintSpeed;
                    movementSpeed = defaultMoveSpeed;
                    break;
            }
        }

        private void HandleMovement()
        {
            if(isJumping)
                return;
            Vector3 forward = _cameraObj.transform.forward;
            Vector3 right = _cameraObj.transform.right;
            forward.y = 0;
            right.y = 0;
            forward = forward.normalized;
            right = right.normalized;
            _moveDirection = forward * _inputManager.verticalInput;
            _moveDirection += right * _inputManager.horizontalInput;
            //_moveDirection.Normalize();
            //_moveDirection.y = 0;
            if (_inputManager.moveAmount > 0.5f)
            {
                _moveDirection *= sprintingSpeed;
            }
            else
            {
                _moveDirection *= movementSpeed;   
            }

            var movementVelocity = _moveDirection;
            playerRigidbody.velocity = movementVelocity;
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
                    _animatorManager.PlayTargetAnimation("Falling",true,0.4f);
                }

                inAirTimer += Time.deltaTime * 1.3f;
                playerRigidbody.AddForce(transform.forward * leapingVelocity);
                playerRigidbody.AddForce(-Vector3.up * (fallingVelocity * inAirTimer));
            }

            if (Physics.SphereCast(rayCastOrigin,0.2f,-Vector3.up,out var hit,groundLayer))
            {
                if (!isGrounded && !_playerManager.isInteracting)
                {
                    _animatorManager.PlayTargetAnimation("Land",true,0.2f);
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
            if (!isGrounded || isJumping) return;
            _animatorManager.animator.SetBool(IsJumping,true);
            _animatorManager.PlayTargetAnimation("Jump",false,0.4f);

            jumpCount--;
            var jumpingVelocity = Mathf.Sqrt(-2f * gravityIntensity * jumpHeight);
            var playerVelocity = _moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelocity;
        }

        public void LookAtTarget(Vector3 target)
        {
            var playerPosition = transform.position;
            var targetLookRotation = Quaternion.LookRotation(target - playerPosition);
            targetLookRotation.x = 0;
            transform.rotation = targetLookRotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("NoSlowArea"))
            {
                _outOfWater = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("NoSlowArea"))
            {
                _outOfWater = false;
            }
        }
    }
}
