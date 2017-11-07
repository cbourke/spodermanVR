using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallScrip : MonoBehaviour {
	public GameObject key1;
	public GameObject key2;
	private int counter = 500;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (key1.GetComponent<buttonScrip>().activated && key2.GetComponent<buttonScrip>().activated && counter >= 0) {
			transform.Translate (Vector3.down * 0.5f * Time.deltaTime);
			counter--;
		}
	}
		
}
