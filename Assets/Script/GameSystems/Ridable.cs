using System;
using Script.Player;
using UnityEngine;
using MidniteOilSoftware;
using Script.Manager;
using UnityEngine.Serialization;

namespace Script.GameSystems
{
    public class Ridable : MonoBehaviour,IInteractable
    {
        private InputManager _inputManager;
        //[SerializeField] private PlayerManager _playerManager;
        [SerializeField] private PlayerLocomotion _playerLocomotion;
        [SerializeField] private GameObject Player;
        [SerializeField] private CapsuleCollider PlayerCollider;
        //[SerializeField] private Transform defaultPlayerTransform;
        [SerializeField] private bool isDriving;

        private Camera _cameraObj;
        [SerializeField]private Vector3 _moveDirection;
        public float waterThrust = 6;
        public float moveThrust = 8;
        public float maxDuration = 300;
        private float _defaultMaxDuration;
        [SerializeField] private float _rideMaxDuration = 30;
        [SerializeField] private bool _isRidden;
        public Rigidbody rb;
        private float floodLevelThrust;
        
        private void Start()
        {
            _defaultMaxDuration = maxDuration;
            _cameraObj = Camera.main;
            rb = GetComponent<Rigidbody>();
            //_playerManager = PlayerManager.Instance;
            _playerLocomotion = PlayerLocomotion.Instance;
            Player = GameObject.FindGameObjectWithTag("Player");
            _inputManager = Player.GetComponent<InputManager>();
            //defaultPlayerTransform = Player.transform;
            PlayerCollider = Player.GetComponent<CapsuleCollider>();
        }
        
        private void FixedUpdate()
        {
            floodLevelThrust = FloodSystem.Instance.FloodLvThrust();
            if (GameManager.Instance.isEmptyPool)
            {
                ObjectPoolManager.DespawnGameObject(gameObject);
                ObjectPoolManager.EmptyPool();
            }
            rb.AddForce(0,0,waterThrust*floodLevelThrust);
            maxDuration -= Time.deltaTime;
            if (maxDuration <= 0)
            {
                Driving(false);
                ObjectPoolManager.DespawnGameObject(gameObject);
                maxDuration = _defaultMaxDuration;
                _isRidden = false;
            }
            if (!isDriving) return;
            HandleMovement();
            HandleRotation();
        }

        private void Update()
        {
            if (isDriving)
            {
                var o = gameObject;
                Player.transform.position = o.transform.position;
                Player.transform.rotation = o.transform.rotation;
            }
        }

        private void Driving(bool driving)
        {
            //_playerManager.enabled = !isDriving;
            _playerLocomotion.isDriving = driving;
            //_playerLocomotion.enabled = !isDriving;
            PlayerCollider.enabled = !driving;
            this.isDriving = driving;
            AnimatorManager.Instance.animator.SetBool(IsDriving,driving);
        }
        
        private void HandleMovement()
        {
            var camTransform = _cameraObj.transform;
            Vector3 forward = camTransform.forward;
            Vector3 right = camTransform.right;
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
                _moveDirection *= moveThrust*floodLevelThrust;
            }
            else
            {
                _moveDirection *= moveThrust*floodLevelThrust;   
            }

            var movementVelocity = _moveDirection;
            //rb.velocity = movementVelocity;
            rb.AddForce(movementVelocity);
        }
        
        private void HandleRotation()
        {

            var targetDirection = 
                CameraRelativeVectorFromInput(_inputManager.horizontalInput, _inputManager.verticalInput);
            //var targetDirection = _cameraObj.forward * _inputManager.verticalInput;
            //targetDirection += _cameraObj.right * _inputManager.horizontalInput;
            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero)
                targetDirection = transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, 1.5f * Time.deltaTime);

            transform.rotation = playerRotation;
        }
        
        private Vector3 CameraRelativeVectorFromInput(float x, float y)
        {
            var up = transform.up;
            var forward = Vector3.ProjectOnPlane(_cameraObj.transform.forward, up).normalized;
            var right = Vector3.Cross(up, forward);

            return x * right + y * forward;
        }

        [Header("Interact")]
        [SerializeField] private bool eKeyEnable;
        [SerializeField] private string prompt;
        [SerializeField] private bool fKeyEnable;
        [SerializeField] private string prompt1;
        private static readonly int IsDriving = Animator.StringToHash("IsDriving");
        public string InteractionPrompt => prompt;
        public string InteractionPrompt1 => prompt1;
        public bool IsEKeyEnable => eKeyEnable;
        public bool IsFKeyEnable => fKeyEnable;
        public bool Interact(Interactor interactor)
        {
            if (!isDriving)
            {
                Driving(true);
                AnimatorManager.Instance.PlayTargetAnimation("LayingSit",true,0f);
                if (!_isRidden)
                {
                    maxDuration = _rideMaxDuration;
                }
                _isRidden = true;
                return true;
            }
            Driving(false);
            return false;
        }

        public bool Interact1(Interactor interactor)
        {
            return false;
        }
    }
}
