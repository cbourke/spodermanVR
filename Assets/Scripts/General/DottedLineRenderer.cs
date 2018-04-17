using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoTiling;

public class DottedLineRenderer : MonoBehaviour {

	Vector2 setVec;
	private float texHeight;
	public bool outward;
	public bool valid;

	private Texture validTex;
	private Texture invalidTex;

	void Start () {
		texHeight = GetComponent<Renderer>().material.mainTexture.width;
		texHeight /= 500;
		validTex = (Texture)Resources.Load ("Textures/General/previewTex");
		invalidTex = (Texture)Resources.Load ("Textures/General/previewTexInval");
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 firstPoint = GetComponent<LineRenderer>().GetPosition(0);
		Vector3 secondPoint = GetComponent<LineRenderer>().GetPosition(1);
		Vector3 diff = secondPoint - firstPoint;
		float scale = diff.magnitude / texHeight;
		GetComponent<Renderer> ().material.SetTextureScale ("_MainTex" , new Vector2( scale , 1));
		if (outward) 
			setVec -= new Vector2(Time.deltaTime * 1 , 0);
		else 
			setVec += new Vector2(Time.deltaTime * 1 , 0);

		if (setVec.magnitude >= texHeight * 2) {
			setVec = new Vector2 (0,0);
		}
		GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex" , setVec);

		if (valid) {
			if (GetComponent<Renderer> ().material.mainTexture != validTex) {
				GetComponent<Renderer> ().material.mainTexture = validTex;
			}

		} else {
			if (GetComponent<Renderer> ().material.mainTexture != invalidTex) {
				GetComponent<Renderer> ().material.mainTexture = invalidTex;
			}
		}
	}


}
