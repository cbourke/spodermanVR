/*
 *This script should be put in an empty GameObject in the world to keep track of all nodes. This script is used when we want to spawn a point
 *in the world. The first point just exists until the second point is called. As it stands, the points exist regardless of mode and the 
 *rope is spawned when the second point is created. This allows both controllers to be in Rope mode simaltaneously without conflict. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRopeNodeTracker : MonoBehaviour {

	public List<Vector3> nodeKeeper;
	public List<GameObject> ropeKeeper;
	public LayerMask layerMask;
	public int layerMaskInt;
	private GameObject node1;
//	private GameObject node2;
	private AudioSource speaker;
	private GameObject speakerObj;
	private Material valid;
	private Material invalid;
	private EventUtil util;
	public GameObject previewRopeObj;

	public Vector3 lookerOffset;
	public Vector3 lookerDest;
	public Vector3 destiOffset;
	public GameObject lookerVisibleObj;

	void Awake() {
		speakerObj = GetComponentInChildren<AudioSource> ().gameObject;
		speaker = GetComponentInChildren<AudioSource> ();
		ropeKeeper = new List<GameObject>();
//		layerMaskInt = ~layerMask.value;
		valid = (Material)Resources.Load ("Materials/General/ropePreviewValid");
		invalid = (Material)Resources.Load ("Materials/General/ropePreviewInvalid");
		util = transform.Find("Events").gameObject.GetComponent<EventUtil>();
	}
	// Use this for initialization
	void Start () {
		previewRopeObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		previewRopeObj.name = "Preview Rope";
		Destroy (previewRopeObj.GetComponent<CapsuleCollider>());
		previewRopeObj.SetActive (false);
	}

	public void spawnNode (Vector3 spawnPoint , GameObject callingController) {

		//spawn a visible preview node at point controller is pointing
		//add position of node to nodeKeeper list
		if (!node1) {
			
			node1 = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			node1.transform.position = spawnPoint;
			node1.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);
			callingController.GetComponent<AudioSource> ().clip = speaker.clip;
			callingController.GetComponent<AudioSource> ().Play ();
			node1.GetComponent<Renderer> ().material = (Material)Resources.Load ("Materials/General/ValidPreviewNode");
			Destroy (node1.GetComponent<SphereCollider>());
			nodeKeeper.Add (spawnPoint);
		} else {
			if (nodeLooker (nodeKeeper[0] , spawnPoint)) {
				nodeKeeper.Add (spawnPoint);
			} else {
				util.playClip (callingController , (AudioClip)Resources.Load("Audio/windowAudio/error"));
			}

//			node2 = GameObject.CreatePrimitive (PrimitiveType.Sphere);
//			node2.transform.position = spawnPoint;
//			node2.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);

		}




	}

	public void deleteRopes() {
		foreach (GameObject rope in ropeKeeper) {
			Destroy (rope);
		}
		ropeKeeper.Clear ();
	}

	public List<GameObject> getRopeKeeper() {
		return ropeKeeper;
	}
	
	// Update is called once per frame
	void Update () {
		if (nodeKeeper.Count >= 2) {
			//spawn a narrow cylinder between the two points
			//this block to transform the cylinder courtesy of user Mike 3 in a Unity answers post 
			Vector3 sumPoints = new Vector3(0,0,0);
			Vector3 offset = nodeKeeper [1] - nodeKeeper [0];
			Vector3 modScale = new Vector3(0.05f, offset.magnitude / 2.0f , 0.05f);
			sumPoints = nodeKeeper[0] + nodeKeeper[1];
			GameObject rope = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			ropeKeeper.Add (rope);
			rope.transform.position = Vector3.Scale (sumPoints , new Vector3(0.5f,0.5f,0.5f));
			speakerObj.transform.position = Vector3.Scale (sumPoints , new Vector3(0.5f,0.5f,0.5f));
			rope.transform.up = offset;
			rope.transform.localScale = modScale;
			rope.tag = "Rope";
			rope.AddComponent(typeof(Rigidbody));
			rope.GetComponent<Rigidbody> ().useGravity = false;
			rope.GetComponent<Rigidbody> ().isKinematic = true;
			speaker.Play ();


			//delete the two points from the list and nodes
			Destroy(node1);
//			Destroy(node2);
			nodeKeeper.Clear();
			destroyPreviewRope ();
		}
	}

	public void ropePreview(Vector3 secPoint) {
		if (nodeKeeper.Count == 1) {
			if (!previewRopeObj.activeSelf) {
				previewRopeObj.SetActive (true);
			}
			Vector3 sumPoints = new Vector3(0,0,0);
			sumPoints = nodeKeeper[0] + secPoint;
			Vector3 offset = secPoint - nodeKeeper [0];
			Vector3 modScale = new Vector3(0.05f, offset.magnitude / 2.0f , 0.05f);
			previewRopeObj.transform.up = offset;
			previewRopeObj.transform.localScale = modScale;
			previewRopeObj.transform.position = Vector3.Scale (sumPoints , new Vector3(0.5f,0.5f,0.5f));
			if (nodeLooker(nodeKeeper[0] , secPoint)) {
				previewRopeObj.GetComponent<Renderer> ().material = valid;
			} else {
				previewRopeObj.GetComponent<Renderer> ().material = invalid;
			}

		} 
	}

	private bool nodeLooker (Vector3 origin , Vector3 destination) {
		Vector3 towardsPoint = destination - origin;
		Vector3 towardsPointNorm = towardsPoint / towardsPoint.magnitude;
		Vector3 scaledNorm = towardsPointNorm * 0.001f;
		Vector3 lookOff = origin + scaledNorm;
		float destOffMag = towardsPoint.magnitude - scaledNorm.magnitude;
		destOffMag -= 0.01f;
		RaycastHit hit;
		if (Physics.Raycast (lookOff, towardsPointNorm, out hit , destOffMag)) {
			//lookerVisibleObj = hit.collider.attachedRigidbody.gameObject;
			if (hit.collider.gameObject.CompareTag ("Rope")) {
				return true;
			} else
				return false;
		}
		else return true;
	}

	public void destroyPreviewRope() {
		if (previewRopeObj.activeSelf) {
			previewRopeObj.SetActive (false);
		}

	}
		
}
