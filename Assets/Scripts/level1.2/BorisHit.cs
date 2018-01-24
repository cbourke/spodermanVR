using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorisHit : MonoBehaviour {
	public GameObject face;
	private Texture ouchFace;
	public AudioClip ouchie;
	public bool punched;

	void Awake() {
		face = transform.GetChild (0).gameObject;
		ouchFace = (Texture)Resources.Load ("Textures/level1.2/ouchFace");
		ouchie = (AudioClip)Resources.Load ("Audio/Speech/ouch");
		punched = false;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision coll)
    {
		if (coll.collider.gameObject.name== "Fist")
        {
			face.GetComponent<Renderer> ().material.mainTexture = ouchFace;
			face.GetComponent<AudioSource> ().clip = ouchie;
			face.GetComponent<AudioSource> ().Play ();
			punched = true;
        }

    }
}
