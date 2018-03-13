using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubLevelBridge : LevelBridge {

	public Material levelMat;
	public Material disableMat;
	public Texture[] levelMatArr;
	public override void ChangeStatus() {
		if (base.open) {
			switch (base.newLevel) {
			case 4:
				levelMat.mainTexture = levelMatArr [3];
				GetComponent<Renderer> ().material = levelMat;
				break;
			case 5:
				levelMat.mainTexture = levelMatArr [4];
				GetComponent<Renderer> ().material = levelMat;
				break;
			default:
				GetComponent<Renderer> ().material = disableMat;
				break;
			}
		} else {
			GetComponent<Renderer> ().material = disableMat;
		}
	}
}
