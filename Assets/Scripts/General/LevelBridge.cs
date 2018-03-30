using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBridge : MonoBehaviour {

	public int newLevel;
	public bool open;
	public Material doorClosed;
	public Material doorOpen;

	public static Dictionary<string, KeyValuePair<int, Texture[]>> LEVELS;

	void Awake() {
		open = false;
		LEVELS = new Dictionary<string, KeyValuePair<int, Texture[]>>(){
			{
				"LVL3",
				new KeyValuePair<int, Texture[]>(
					4,
					new Texture[] {
						// load texures
						Resources.Load<Texture>("Textures/HUB2.0/lvl3Screen")
					}
				)

			},		
			{
				"LVL4",
				new KeyValuePair<int, Texture[]>(
					5,
					new Texture[] {
						// load texures
						Resources.Load<Texture>("Textures/HUB2.0/lvl4Screen")
					}
				)
			}
		};
	}

	void Start() {
		doorOpen = Resources.Load ("Materials/General/NewLevel")as Material;
		doorClosed = Resources.Load ("Materials/General/NewLevelDisabled") as Material;
	}

	void Update() {
//		if (GetComponent<ButtonHighlighter> ()) {
//
//		} else {
//			if (open) {
//				GetComponent<Renderer> ().material = doorOpen;
//			} else {
//				GetComponent<Renderer> ().material = doorClosed;
//			}
//		}
		ChangeStatus();
	}

	public virtual void ChangeStatus() {
		if (open) {
			GetComponent<Renderer> ().material = doorOpen;
		} else {
			GetComponent<Renderer> ().material = doorClosed;
		}
	}

}
