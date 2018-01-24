using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPos : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
//	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//		Debug.Log (animator.gameObject.transform.position.x);
//		animator.GetCurrentAnimatorStateInfo()
//	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
//	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//		animator.transform.parent.position = animator.transform.position;
//	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.transform.parent.position = animator.transform.position;

	}
}
