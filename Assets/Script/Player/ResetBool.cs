using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    public string isInteractiveBool;

    public bool isInteractiveStatus;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractiveBool,isInteractiveStatus);
    }
}
