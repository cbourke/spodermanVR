using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IPersistObject {

	public HeadColliderHandler head;
	protected EventUtil util;
	protected Animator badAnim;
	protected bool canSeePlayer;
	public float speed = 1;
	public bool animationLock = false;
	protected float lastWebShotTime;
	protected float deadAt;
	protected AudioClip enemPunch;
	protected AudioClip enemShot;
	public  Material enemyDefaultSkin;
	public  Material enemySlowSkin;
	private Renderer limbRend;
	private Renderer bodyRend;
	private Renderer eyeRend;
	protected bool heyB;
	public AudioClip hey;

	public virtual void Persist(){
		if (this.deadAt > 0)
			this.deadAt = Time.fixedTime;
	}

	void Awake() {
		util = EventUtil.FindMe ();
		badAnim = this.GetComponent<Animator> ();
		head = HeadColliderHandler.FindMe ();
		limbRend = transform.Find ("Body").gameObject.GetComponent<Renderer>();
		bodyRend = transform.Find ("Chest").gameObject.GetComponent<Renderer>();
		eyeRend = transform.Find ("Eye").gameObject.GetComponent<Renderer>();
		enemyDefaultSkin = (Material)Resources.Load("Materials/General/Enemy");
		enemySlowSkin = (Material)Resources.Load("Materials/General/EnemySlow");
		enemPunch = (AudioClip)Resources.Load ("Audio/General/enemPunch");
		enemShot = (AudioClip)Resources.Load ("Audio/General/shot");
		hey = (AudioClip)Resources.Load ("Audio/Speech/hey");
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (!IsDead ()) {
			if (Time.fixedTime - lastWebShotTime >= 5f) {
				speed = 1;
				ChangeEnemySkin (enemyDefaultSkin);
			}
			badAnim.speed = speed;
			canSeePlayer = util.FieldOfVision (this.gameObject);
			if (canSeePlayer) {
				if (!heyB) {
					heyB = true;
					util.playClip (this.gameObject , hey);
				}
				this.LookAt (head.gameObject);
				this.Walk (head.gameObject);
				this.Attack (head.gameObject);
			} else {
				this.StopWalk ();
				this.StopAttack ();
			}
		} else {
			StopWalk ();
			StopAttack ();
			if (Time.fixedTime - deadAt >= 3.0f) {
				Destroy (this.gameObject);
			}
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
	}

	public virtual void LookAt(GameObject target) {
		Vector3 headUpVec = target.transform.position + new Vector3 (0,3f,0);
		Vector3 normalOfTri = Vector3.Cross (target.transform.position - transform.position , headUpVec - transform.position); 
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(normalOfTri) , 2f * Time.deltaTime);
	}

	public virtual void StopWalk() {
		badAnim.SetBool ("Walk" , false);
	}

	public virtual void StopAttack() {

	}

	public virtual void SlowSpeed(float newSpd) {
		speed = newSpd;
		lastWebShotTime = Time.fixedTime;
		util.playClip (this.gameObject , (AudioClip)Resources.Load("Audio/Speech/slowEnemy"));
		ChangeEnemySkin (enemySlowSkin);
	}

	public virtual void ChangeEnemySkin(Material mat) {
		bodyRend.material = mat;
		limbRend.material = mat;
	}

	public virtual void Damage() {
		util.playClip (this.gameObject , (AudioClip)Resources.Load("Audio/Speech/ouch"));
	}

	public virtual bool IsDead() {
		if (deadAt > 0)
			return true; 
		
		float sideAngleDiff = Mathf.Abs(Vector3.Dot (Vector3.up , transform.forward.normalized));
		float frontAngleDiff = Mathf.Abs(Vector3.Dot (Vector3.up , transform.right.normalized));
		if (sideAngleDiff > 0.8f || frontAngleDiff > 0.8f) {
			deadAt = Time.fixedTime;
			util.playClip (this.gameObject , (AudioClip)Resources.Load("Audio/Speech/death"));
		}
		return deadAt > 0;
	}
}
