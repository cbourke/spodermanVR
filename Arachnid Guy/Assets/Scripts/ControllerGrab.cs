/*
	This ControllerGrab script is designed to handle interaction with physics objects, as well as climbing interactions.
	To use this script, it must be attached to both controllers.
	The controllers must also have a Rigidbody, not using gravity, but being kinematic, and have the SteamVR_TrackedObject script attached as well. 
	The public variable of otherController must be assigned to ensure climbing mutual exclusion.
	To interact with an object and climb it, the object must be tagged as "Climbable".
	Object must also have a Rigidbody, but must not use gravity, be kinematic, and also must have a Collider, but not be a trigger. 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrab : MonoBehaviour {

	public SteamVR_TrackedObject trackedObj;
	public GameObject collidingObject;
	public GameObject objectInHand;
	public GameObject otherController;
	public Vector3 startingControllerPosition;
	public Transform cameraRigTransform;

	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input ((int)trackedObj.index); }
	}

	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();

	}

	private void SetCollidingObject(Collider col)
	{
		if (collidingObject || !col.GetComponent<Rigidbody>())
		{
			return;
		}
		collidingObject = col.gameObject;


	}

	public void OnTriggerEnter(Collider other) {
		SetCollidingObject (other);
	}

	public void OnTriggerStay(Collider other) {
		SetCollidingObject (other);
	}

	public void OnTriggerExit(Collider other) {
		if (!collidingObject) {
			return;
		}
		collidingObject = null;
	}

	public void GrabPhysicsObject() {
		objectInHand = collidingObject;
		if (objectInHand == null)
			throw new UnityException ("ObjectInHand is null you fucknut");
		collidingObject = null;
		var joint = AddFixedJoint ();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody> ();

	}

	public void GrabPhysicsObjectWithParam(GameObject obj) {
		objectInHand = obj;
		if (objectInHand == null)
			throw new UnityException ("ObjectInHand is null you fucknut");
		collidingObject = null;
		var joint = AddFixedJoint ();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody> ();

	}

	private FixedJoint AddFixedJoint() {
		FixedJoint fx = gameObject.AddComponent<FixedJoint> ();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}

	public void ReleasePhysicsObject() {
		if (GetComponent<FixedJoint> ()) {
			GetComponent<FixedJoint> ().connectedBody = null;
			Destroy (GetComponent<FixedJoint> ());
			objectInHand.GetComponent<Rigidbody> ().velocity = Controller.velocity;
			objectInHand.GetComponent<Rigidbody> ().angularVelocity = Controller.angularVelocity;
			objectInHand = null;
		}
	}

	public void GrabClimbableObject() {
		if (!otherController.GetComponent<FunctionController>().isClimbing) {	//at this point, this check is redundant. remove if it doesn't break anything
			startingControllerPosition = trackedObj.transform.position;
			objectInHand = collidingObject;
			collidingObject = null;
		}

	}

	public void ReleaseClimbableObject() {
		startingControllerPosition = new Vector3 (0,0,0);
		objectInHand = null;

	}

	public void MoveCameraRig() {
		Vector3 difference = startingControllerPosition - trackedObj.transform.position;
		cameraRigTransform.position += difference;
	}

	public bool Grab() {
		Debug.Log ("I'm trying to grab it!");
		if (collidingObject && collidingObject.GetComponent<Rigidbody> () && !collidingObject.CompareTag ("Climbable")) {
			GrabPhysicsObject ();
			Debug.Log ("I'm grabbing it!");
            return false;
		} 

		else if (collidingObject && collidingObject.CompareTag ("Climbable")) {
			if (otherController.GetComponent<FunctionController> ().isClimbing) {
				otherController.GetComponent<ControllerGrab> ().ReleaseClimbableObject ();
				otherController.GetComponent<FunctionController> ().isClimbing = false;
				GrabClimbableObject ();
                return true;
			} else {
				GrabClimbableObject ();
                return true;
			}
		}
        else
        {
            return false;
        }

	}

	public void UnGrab() {
		if (objectInHand && objectInHand.GetComponent<Rigidbody> () && !objectInHand.CompareTag ("Climbable")) {
			ReleasePhysicsObject ();
		} 

		else if (objectInHand && objectInHand.CompareTag ("Climbable")) {
			if (this.GetComponent<FunctionController>().isClimbing) {
				ReleaseClimbableObject ();
			} else {
				return;
			}

		}
	}
}