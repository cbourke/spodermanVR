﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Events : MonoBehaviour {

	public GameObject window1;	//intro window
	public GameObject window2;	//first room enter
//	public GameObject window3;	//low health warning
	public Texture[] window1Feed;
	public Texture[] window2Feed;
//	public Texture[] window3Feed;
	public GameObject trapdoor;
	public bool trapdoorBool = false;
	public Level4Boss boss;
	public int nextLevel;

	private EventUtil util;

	public static Level4Events FindMe() {
		return  GameObject.FindObjectOfType<Level4Events>();
	}
	// Use this for initialization
	void Awake() {
		util = EventUtil.FindMe ();
		boss = Level4Boss.FindMe ();

	}
	void Start () {
		util.GetWindowControllerFromWindow (window1).updateArray (window1Feed);
		util.GetWindowControllerFromWindow (window2).updateArray (window2Feed);
//		util.GetWindowControllerFromWindow (window3).updateArray (window3Feed);
		window1.SetActive (true);
		window2.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (trapdoorBool && trapdoor.transform.position.z >= 1f)
			trapdoor.transform.Translate (-trapdoor.transform.forward * Time.deltaTime);
	}

	public void TriggerZone1() {
		window2.SetActive (true);
		util.GetAnimFromWindow (window1).SetTrigger ("TurnOff");
	}

	public void TriggerZone3() {
		trapdoorBool = true;
		boss.activate = true;
	}

	public void EndScene() {
		UnityEngine.SceneManagement.SceneManager.LoadScene (nextLevel);
	}
}
