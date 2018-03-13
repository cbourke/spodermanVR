using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Boss : MonoBehaviour {

	public EventUtil util;
	public GameObject playerHead;

	// Use this for initialization
	void Awake() {
		util = EventUtil.FindMe ();
		playerHead = HeadColliderHandler.FindMe ().gameObject;
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion q = Quaternion.LookRotation (playerHead.transform.position - transform.position);
		transform.rotation = Quaternion.RotateTowards (transform.rotation , q , 100 * Time.deltaTime);
	}
}
