using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour {

	public HeadColliderHandler head;
	protected EventUtil util;
	protected Animator badAnim;
	protected bool canSeePlayer;
	public float speed = 1;
	private GameObject halo;
	public bool animationLock = false;

//	protected bool getAnimationLock(){
//		if (!_animationLock) {
//			_animationLock = true;
//		}
//		return _animationLock;
//	}
//
//	public void releaseAnimationLock() {
//		_animationLock = false;
//	}

	void Awake() {
		util = EventUtil.FindMe ();
		badAnim = this.GetComponent<Animator> ();
		head = HeadColliderHandler.FindMe ();
		halo = transform.Find ("Halo").gameObject;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		badAnim.speed = speed;
		halo.SetActive (speed != 1);
		canSeePlayer = util.FieldOfVision (this.gameObject);
		if (canSeePlayer) {
			this.LookAt (head.gameObject);
			this.Walk (head.gameObject);
			this.Attack (head.gameObject);
		} else {
			this.StopWalk ();
			this.StopAttack ();
		}
	}

	public virtual void Walk(GameObject target) {
		if (!badAnim.GetBool("Walk")) {
			badAnim.SetBool ("Walk", true);

		}
		if (!animationLock) {
			gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position , target.transform.position , speed * Time.deltaTime);
		}
	}

	public virtual void Attack(GameObject target){
		Debug.Log ("BASE.ATTACK!!!!!");
	}

	public virtual void LookAt(GameObject target) {
		Vector3 headUpVec = target.transform.position + new Vector3 (0,3f,0);
		Vector3 normalOfTri = Vector3.Cross (target.transform.position - transform.position , headUpVec - transform.position); 
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(normalOfTri) , 2f * Time.deltaTime);
//		Debug.DrawRay (transform.position , transform.forward , Color.red);
//		Debug.DrawRay (transform.position , target.transform.position - transform.position , Color.blue);
	}

	public virtual void StopWalk() {
		badAnim.SetBool ("Walk" , false);
	}

	public virtual void StopAttack() {

	}
}
