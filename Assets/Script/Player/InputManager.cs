using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Player
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private AnimatorManager _animatorManager;
        private PlayerLocomotion _playerLocomotion;

        public float moveAmount;
        public float verticalInput;
        public float horizontalInput;
        public bool shiftInput;
        //public bool jumpInput;

        [SerializeField]private Vector2 movementInput;

        private void Awake()
        {
            _playerLocomotion = GetComponent<PlayerLocomotion>();
            _animatorManager = GetComponent<AnimatorManager>();
        }

        private void OnEnable()
        {
            if (_playerInput == null)
            {
                _playerInput = new PlayerInput();
                _playerInput.PlayerMovement.movement.performed += i => movementInput = i.ReadValue<Vector2>();
                _playerInput.PlayerAction.Sprint.performed += i => shiftInput = true;
                _playerInput.PlayerAction.Sprint.canceled += i => shiftInput = false;
                _playerInput.PlayerAction.Jump.performed += HandleJumpInput;
                //_playerInput.PlayerAction.Jump.performed += i => jumpInput = true;
            }
            _playerInput.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Disable();
        }

        public void HandleAllInput()
        {
            HandleMovementInput();
            //HandleJumpingInput();
        }

        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            if (!shiftInput)
            {
                moveAmount /= 2;
            }
            _animatorManager.UpdateAnimatorValues(0,moveAmount);
        }
        
        private void HandleJumpInput(InputAction.CallbackContext obj)
        {
            if (_playerLocomotion.isDriving || _playerLocomotion.isStunt)
            {
                return;
            }
            _playerLocomotion.HandleJumping();
        }

        /*private void HandleJumpingInput()
        {
            if (jumpInput)
            {
                jumpInput = false;
                _playerLocomotion.HandleJumping();
            }
        }*/
        
    }
}
