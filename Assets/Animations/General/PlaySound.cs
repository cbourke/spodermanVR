using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : StateMachineBehaviour {

	private AudioClip sound;
	private AudioSource source;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		source = animator.gameObject.GetComponent<AudioSource> ();
		if (stateInfo.IsName ("SignBlinkIn")) {
			sound = (AudioClip)Resources.Load ("Audio/windowAudio/openUIWindow");
		} else if (stateInfo.IsName ("SignBlinkOff")) {
			sound = (AudioClip)Resources.Load ("Audio/windowAudio/closeUIWindow");
		} else if (stateInfo.IsName ("FloatLeft") || stateInfo.IsName ("FloatRight") || stateInfo.IsName ("FloatRightHalf")) {
			sound = (AudioClip)Resources.Load ("Audio/windowAudio/floatWindow");
		} else {
			return;
		}
		source.clip = sound;
		source.Play ();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
