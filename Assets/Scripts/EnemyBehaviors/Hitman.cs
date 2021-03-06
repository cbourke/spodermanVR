﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitman : BaseEnemy {

	protected float attTimer = 1.5f;


	public override void Walk(GameObject target) {
		if (Vector3.Distance (target.transform.position, transform.position) >= 5f) {
			base.Walk (target);
		} else {
			base.StopWalk ();
		}

	}

	public override void Attack (GameObject target) {
		attTimer -= speed * Time.deltaTime;
		if (attTimer <= 0) {
			attTimer = 3f;
			base.animationLock = true ;
			StartCoroutine (shoot(target));
			base.badAnim.SetTrigger ("IdleToShoot");
			base.badAnim.SetBool ("Walk" , false);
		}
	}

	public override void StopAttack() {
		
	}

	private IEnumerator shoot(GameObject target) {
		float timeTillShoot = 0.75f / base.speed;
		yield return new WaitForSeconds (timeTillShoot);
		util.playClip (this.gameObject , enemShot);
		GameObject bull = Instantiate ((GameObject)Resources.Load("Prefabs/Bullet"));
		bull.transform.position = transform.position + new Vector3 (-0.5f , gameObject.GetComponent<BoxCollider> ().bounds.size.y * 0.75f , gameObject.GetComponent<BoxCollider> ().bounds.size.z / 2);
	}
}
