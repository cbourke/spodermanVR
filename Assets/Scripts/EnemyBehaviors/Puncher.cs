using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puncher : BaseEnemy {

	public override void Attack(GameObject target) {
		if (Vector3.Distance (target.transform.position, transform.position) <= 1.5f) {
			base.StopWalk ();
			if (!base.animationLock) {
				int randAtt = Random.Range (1,4);
				Debug.Log (randAtt);
				base.badAnim.SetTrigger ("Attack" + randAtt);
				StartCoroutine (Damage());
			}
			base.animationLock = true ;

			base.badAnim.SetBool ("Walk" , false);
		}
	}

	private IEnumerator Damage() {
		yield return new WaitForSeconds (0.5f / base.speed);
		base.head.Damage (25f);
	}
}
