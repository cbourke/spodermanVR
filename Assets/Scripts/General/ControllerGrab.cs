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
		if (this.name.Equals("Controller (left)")) 
			otherController = GameObject.Find ("[CameraRig]").transform.Find ("Controller (right)").gameObject;
		else 
			otherController = GameObject.Find ("[CameraRig]").transform.Find ("Controller (left)").gameObject;
	}
	void Update(){
		if (objectInHand != null) {
			IPersistObject o = objectInHand.GetComponent<IPersistObject> ();
			if (o != null) {
				o.Persist ();
			}
		}

	}
	private void SetCollidingObject(Collider col)
	{
		if (collidingObject || !col.GetComponent<Rigidbody>())
		{
			return;
		}
		collidingObject = col.gameObject;


	}

	public void OnTriggerEnter(Collider other) {	//prone to errors
		SetCollidingObject (other);
		if (this.GetComponent<ControllerRetract>().retracting && this.GetComponent<ControllerRetract>().retractobj && this.GetComponent<ControllerRetract>().retractobj.GetInstanceID() == other.attachedRigidbody.gameObject.GetInstanceID()) {//gameobject.GetInstanceID()) {
			this.GetComponent<ControllerRetract> ().retracting = false;
			this.GetComponent<ControllerRetract> ().retractobj = null;
			other.attachedRigidbody.isKinematic = false;
			other.attachedRigidbody.useGravity = true;
		}
		Controller.TriggerHapticPulse (4000);
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
		if (collidingObject && collidingObject.GetComponent<Rigidbody> () && collidingObject.GetComponent<Rigidbody> ().useGravity) {
			GrabPhysicsObject ();
            return false;
		} 

		else if (collidingObject && (collidingObject.CompareTag ("Climbable") || collidingObject.CompareTag ("Rope"))) {
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
		else if (collidingObject && collidingObject.GetComponent<LevelBridge>() && collidingObject.GetComponent<LevelBridge>().open) {
			UnityEngine.SceneManagement.SceneManager.LoadScene (collidingObject.GetComponent<LevelBridge>().newLevel);
			return false;
		}
        else
        {
            return false;
        }

	}

	public void UnGrab() {
		if (objectInHand && objectInHand.GetComponent<Rigidbody> () && !objectInHand.CompareTag ("Climbable") && !objectInHand.CompareTag ("Rope")) {
			ReleasePhysicsObject ();
		} 
		if (GetComponent<FixedJoint>()) {
			GetComponent<FixedJoint> ().connectedBody = null;
			Destroy (GetComponent<FixedJoint> ());
		}

		else if (objectInHand && (objectInHand.CompareTag ("Climbable") || objectInHand.CompareTag ("Rope"))) {
			if (this.GetComponent<FunctionController>().isClimbing) {
				ReleaseClimbableObject ();
			} else {
				return;
			}

		}
	}

	public void RopeSlide () {
		if (objectInHand == null || collidingObject == null) {
			return;
		}
		if (objectInHand.CompareTag("Rope") && collidingObject.GetInstanceID() == objectInHand.GetInstanceID()) {
			Vector3 ropeUp = objectInHand.transform.up;
			Vector3 controllerUp = trackedObj.gameObject.transform.forward;
			if (Vector3.Dot(ropeUp , controllerUp) >= 0) {
				
				cameraRigTransform.Translate (ropeUp * Time.deltaTime);
				startingControllerPosition += ropeUp * Time.deltaTime;
			} 
			else {
				cameraRigTransform.Translate (-ropeUp * Time.deltaTime);
				startingControllerPosition += -ropeUp * Time.deltaTime;
			}

		}


	}
		
}