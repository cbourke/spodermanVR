using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LandingPadResize : MonoBehaviour {
	private Vector3 size;

	// Use this for initialization
	void Start () {
		HmdQuad_t hmdQuad = new HmdQuad_t();
		if (SteamVR_PlayArea.GetBounds (SteamVR_PlayArea.Size.Calibrated, ref hmdQuad)) {
			float width = Mathf.Abs (hmdQuad.vCorners0.v0 - hmdQuad.vCorners1.v0);
			float length = Mathf.Abs (hmdQuad.vCorners2.v2 - hmdQuad.vCorners1.v2);
			size = new Vector3 (width, 0, length);
		} else {
			return;
		}

		transform.localScale = new Vector3 (size.x,1,size.z);
	}

}
