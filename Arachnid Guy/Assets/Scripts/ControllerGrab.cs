/*
	This ControllerGrab script is designed to handle interaction with physics objects, as well as climbing interactions.
	To use this script, it must be attached to both controllers.
	The controllers must also have a Rigidbody, not using gravity, but being kinematic, and have the SteamVR_TrackedObject script attached as well. 
	The public variable of otherController must be assigned to ensure climbing mutual exclusion.
	To interact with an object and climb it, the object must be tagged as "Climbable".
	Object must also have a Rigidbody, but must not use gravity, be kinematic, and also must have a Collider, but not be a trigger. 

	This is also where we handle level switches, in the Grab method.
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
		cameraRigTransform = GameObject.Find ("[CameraRig]").transform;
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
		collidingObject = null;
		var joint = AddFixedJoint ();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody> ();

	}

	public void GrabPhysicsObjectWithParam(GameObject obj) {
		if (obj == null) {
			return;
		}
		objectInHand = obj;
		collidingObject = null;
		var joint = AddFixedJoint ();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody> ();

	}

	private FixedJoint AddFixedJoint() {
		FixedJoint fx = gameObject.AddComponent<FixedJoint> ();
		fx.breakForce = 200000000;
		fx.breakTorque = 20000000;
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
		if (!otherController.GetComponent<FunctionController>().isClimbing) {
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
		if (collidingObject && collidingObject.GetComponent<Rigidbody> () && !collidingObject.CompareTag ("Climbable")) {
			GrabPhysicsObject ();
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
		else if (collidingObject && collidingObject.GetComponent<LevelBridge>()) {
			UnityEngine.SceneManagement.SceneManager.LoadScene (collidingObject.GetComponent<LevelBridge>().newLevel);
			return false;
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
		if (GetComponent<FixedJoint>()) {
			GetComponent<FixedJoint> ().connectedBody = null;
			Destroy (GetComponent<FixedJoint> ());
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