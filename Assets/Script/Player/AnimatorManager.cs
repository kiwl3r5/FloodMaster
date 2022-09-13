using Script.Player;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    private int _horizontal;
    private int _vertical;
    [SerializeField] private float curSpeed;
    
    private Vector3 _previousPosition;
    private PlayerLocomotion _playerLocomotion;

    private static readonly int IsInteracting = Animator.StringToHash("IsInteracting");

    private void Awake()
    {
        _playerLocomotion = GetComponent<PlayerLocomotion>();
        //_inputManager = GetComponent<InputManager>();
        animator = GetComponentInChildren<Animator>();
        _horizontal = Animator.StringToHash("Horizontal");
        _vertical = Animator.StringToHash("Vertical");

    }

    private void FixedUpdate()
    {
        var position = transform.position;
        var positionXZ = new Vector3(position.x,0,position.z);
        var curMove = positionXZ - _previousPosition;
        curSpeed = curMove.magnitude / Time.deltaTime;
        _previousPosition = positionXZ;
        curSpeed /= _playerLocomotion.sprintingSpeed;
        curSpeed = Mathf.Round(curSpeed * 10f) / 10f;
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        animator.SetBool(IsInteracting,isInteracting);
        animator.CrossFade(targetAnimation,0.2f);
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
    {
        //Snapping
        float snappingHorizontal;
        //float snappingVertical;
      
        #region Snapping Horizontal
        if (horizontalMovement > 0 && horizontalMovement < 0.55f && curSpeed >0.1)
        {
            snappingHorizontal = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            snappingHorizontal = 1;
        }
        else if (curSpeed <= 0.1)
        {
            snappingHorizontal = 0;
        }
        else
        {
            snappingHorizontal = 0;
        }
        #endregion
        
        /*#region Snapping Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f && curSpeed >0.1)
        {
            snappingVertical = 0.5f;
        }
        else if (verticalMovement > 0.55f && curSpeed >0.5)
        {
            snappingVertical = 1;
        }
        else if (curSpeed <= 0.1)
        {
            snappingVertical = 0;
        }
        else
        {
            snappingVertical = 0;
        }
        #endregion*/
        
        if (curSpeed > 1)
        {
            curSpeed = 1;
        }
        if (!_playerLocomotion.isGrounded)
        {
            curSpeed = 0;
        }
        
        animator.SetFloat(_horizontal,snappingHorizontal,0.1f,Time.deltaTime);
        animator.SetFloat(_vertical, curSpeed, 0.1f, Time.deltaTime);
    }
}
