using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RoomResize : MonoBehaviour {
	public Vector3 size;
	private GameObject northWall;
	private GameObject eastWall;
	private GameObject southWall;
	private GameObject westWall;
	private GameObject floor;
	private GameObject ceiling;

	void Awake () {
		northWall = GameObject.Find ("NorthWall");
		eastWall = GameObject.Find ("EastWall");
		southWall = GameObject.Find ("SouthWall");
		westWall = GameObject.Find ("WestWall");
		floor = GameObject.Find ("Floor");
		ceiling = GameObject.Find ("Ceiling");
	}
	// Use this for initialization
	void Start () {
		//this method of finding room size courtesy of user Lukeus_Maximus in a reddit post 
		HmdQuad_t hmdQuad = new HmdQuad_t();
		if (SteamVR_PlayArea.GetBounds (SteamVR_PlayArea.Size.Calibrated, ref hmdQuad)) {
			float width = Mathf.Abs (hmdQuad.vCorners0.v0 - hmdQuad.vCorners1.v0);
			float length = Mathf.Abs (hmdQuad.vCorners2.v2 - hmdQuad.vCorners1.v2);
			size = new Vector3 (width, 0, length);
		} else {
			return;
		}

		northWall.transform.position = new Vector3 (size.x/2,1,0);
		southWall.transform.position = new Vector3 (-size.x/2,1,0);
		eastWall.transform.position = new Vector3 (0,1,-size.z/2);
		westWall.transform.position = new Vector3 (0,1,size.z/2);
		floor.transform.localScale = new Vector3 (size.x/8,1,size.z/8);
		ceiling.transform.localScale = new Vector3 (size.x,0.2f,size.z);

	}

}
