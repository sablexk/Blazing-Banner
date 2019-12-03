using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.GetComponent<Character>().Attack = true;

        animator.SetFloat("Speed", 0);
        //test line

        if(animator.tag == "Player")
        {
            if (Player.Instance.OnGround)
            {

                Player.Instance.MyRB.velocity = Vector2.zero;
            }

            if (!Player.Instance.OnGround)
            {
                if (Player.Instance.MyRB.velocity.y > 0)
                {
                    
                    Player.Instance.MyRB.AddForce(new Vector2(0f, 500f));
                }
            }
        }
        
       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Character>().Attack = false;
        animator.GetComponent<Character>().WeaponCollider[0].enabled = false;
        animator.ResetTrigger("Attack");
       
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
