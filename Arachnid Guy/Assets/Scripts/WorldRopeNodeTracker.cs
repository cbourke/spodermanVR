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
	private GameObject node1;
	private GameObject node2;

	// Use this for initialization
	void Start () {

	}

	public void spawnNode (Vector3 spawnPoint) {

		//spawn a visible preview node at point controller is pointing
		//add position of node to nodeKeeper list
		if (!node1) {
			
			node1 = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			node1.transform.position = spawnPoint;
			node1.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);
			node1.GetComponent<Renderer> ().material = (Material)Resources.Load ("Materials/ValidPreviewNode");
			nodeKeeper.Add (spawnPoint);
		} else {

			nodeKeeper.Add (spawnPoint);
			node2 = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			node2.transform.position = spawnPoint;
			node2.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);
			nodeKeeper.Add (spawnPoint);
		}




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
			rope.transform.position = Vector3.Scale (sumPoints , new Vector3(0.5f,0.5f,0.5f));
			rope.transform.up = offset;
			rope.transform.localScale = modScale;
			rope.tag = "Climbable";
			rope.AddComponent(typeof(Rigidbody));
			rope.GetComponent<Rigidbody> ().useGravity = false;
			rope.GetComponent<Rigidbody> ().isKinematic = true;


			//delete the two points from the list and nodes
			Destroy(node1);
			Destroy(node2);
			nodeKeeper.Clear();


		}
	}
}
