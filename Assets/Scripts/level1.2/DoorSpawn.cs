using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSpawn : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		this.transform.position = new Vector3 (player.transform.position.x - 0.3f , 0.75f , player.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
