using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour {

	public HeadColliderHandler head;
	public GameObject headObj;
	protected EventUtil util;
	protected Animator badAnim;

	void Awake() {
		util = EventUtil.FindMe ();
		badAnim = this.GetComponent<Animator> ();
		head = HeadColliderHandler.FindMe ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (util.FieldOfVision (this.gameObject)) {
			this.LookAt (headObj);
			this.Walk ();
			this.Attack ();
		} else {
			badAnim.SetBool ("Walk" , false);
		}
	}

	public virtual void Walk() {
		badAnim.SetBool ("Walk" , true);
		gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position , head.gameObject.transform.position , Time.deltaTime);
	}

	public virtual void Attack(){
		Debug.Log ("BASE.ATTACK!!!!!");
	}

	public virtual void LookAt(GameObject target) {
//		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position) , Time.deltaTime);
//		transform.rotation = Quaternion.RotateTowards (transform.rotation , Quaternion.LookRotation(target.transform.position - transform.position) , Time.deltaTime);
		//TODO: THIS

	}
}
