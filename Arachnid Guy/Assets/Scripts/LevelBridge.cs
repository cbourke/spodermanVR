using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBridge : MonoBehaviour {

	public int newLevel;
	public bool open;
	public Material doorClosed;
	public Material doorOpen;

	void Awake() {
		open = true;
	}

	void Start() {
		doorOpen = Resources.Load ("Materials/NewLevel")as Material;
		doorClosed = Resources.Load ("Materials/NewLevelDisabled") as Material;
	}

	void Update() {
		if (GetComponent<ButtonHighlighter> ()) {

		} else {
			if (open) {
				GetComponent<Renderer> ().material = doorOpen;
			} else {
				GetComponent<Renderer> ().material = doorClosed;
			}
		}
	}

}
