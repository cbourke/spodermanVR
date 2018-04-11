using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : BaseEnemy {

	private float attTimer;

	public override void Walk(GameObject target) {

	}

	public override void Attack (GameObject target) {
		attTimer -= base.speed * Time.deltaTime;
		if (attTimer <= 0) {
			attTimer = 3f;
			base.animationLock = true ;
			StartCoroutine (shoot(target));
			base.badAnim.SetTrigger ("IdleToShoot");
//			base.badAnim.SetBool ("Walk" , false);
//			Debug.Log ("I was supposed to stop here");
			//			base.badAnim.SetBool ("Walk" , true);
		}
	}

	public override void StopAttack() {

	}

	private IEnumerator shoot(GameObject target) {
		float timeTillShoot = 0.75f / base.speed;
		yield return new WaitForSeconds (timeTillShoot);
		util.playClip (this.gameObject , enemShot);
		GameObject bull = Instantiate ((GameObject)Resources.Load("Prefabs/Bullet"));
//		bull.transform.position = transform.position + new Vector3 (-0.5f , gameObject.GetComponent<BoxCollider> ().bounds.size.y * 0.75f , gameObject.GetComponent<BoxCollider> ().bounds.size.z / 2);
		bull.transform.position = transform.position + transform.right * 0.5f + new Vector3(0,1f,0) + -transform.forward * 0.3f;
	}

}
