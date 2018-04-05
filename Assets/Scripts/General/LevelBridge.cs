using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBridge : MonoBehaviour {

	public int newLevel;
	public bool open;
	public Material doorClosed;
	public Material doorOpen;

	public static LevelBridge FindMe() {
		return  GameObject.FindObjectOfType<LevelBridge>();
	}

	public static Dictionary<string, KeyValuePair<int, Texture[]>> LEVELS;

	public bool isOpen() {
		return newLevel > -1;
	}

	void Awake() {
		newLevel = -1;
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
			},
			{
				"LVL0",
				new KeyValuePair<int, Texture[]>(
					1,
					new Texture[] {
						// load texures
						Resources.Load<Texture>("Textures/HUB2.0/lvl0Screen")
					}
				)
			},
			{
				"LVL2",
				new KeyValuePair<int, Texture[]>(
					3,
					new Texture[] {
						// load texures
						Resources.Load<Texture>("Textures/HUB2.0/lvl2Screen")
					}
				)
			},
			{
				"LVL1",
				new KeyValuePair<int, Texture[]>(
					2,
					new Texture[] {
						// load texures
						Resources.Load<Texture>("Textures/HUB2.0/lvl1Screen")
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
		//open = newLevel
		ChangeStatus();

	}

	public virtual void ChangeStatus() {
		
		if (newLevel > -1) {
			GetComponent<Renderer> ().material = doorOpen;
		} else {
			GetComponent<Renderer> ().material = doorClosed;
		}
	}

}
