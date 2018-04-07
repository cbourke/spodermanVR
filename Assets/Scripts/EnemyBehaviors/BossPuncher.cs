using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPuncher : BaseEnemy {

	public override void Update () {
		if (!IsDead ()) {
			if (Time.fixedTime - lastWebShotTime >= 5f) {
				speed = 1;
				ChangeEnemySkin (enemyDefaultSkin);
			}
			badAnim.speed = speed;

				this.LookAt (head.gameObject);
				this.Walk (head.gameObject);
				this.Attack (head.gameObject);

		} else {
			StopWalk ();
			StopAttack ();
			if (Time.fixedTime - deadAt >= 3.0f) {
				Destroy (this.gameObject);
			}
		}
	}

	public override void Attack(GameObject target) {
		if (Vector3.Distance (target.transform.position, transform.position) <= 1.5f) {
			base.StopWalk ();
			if (!base.animationLock) {
				int randAtt = Random.Range (1,4);
				base.badAnim.SetTrigger ("Attack" + randAtt);
				StartCoroutine (Damage(target));
			}
			base.animationLock = true ;

			base.badAnim.SetBool ("Walk" , false);
		}
	}

	private IEnumerator Damage(GameObject targ) {
		yield return new WaitForSeconds (0.5f / base.speed);
		util.playClip (this.gameObject , base.enemPunch);
		if (Vector3.Distance (targ.transform.position, transform.position) <= 1.5f) 
			base.head.Damage (25f);
	}
}
