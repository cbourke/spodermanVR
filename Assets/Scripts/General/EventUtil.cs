using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventUtil : MonoBehaviour {

	public GameObject headset;
	public GameObject visibleObj;
	public bool lookingBool;
	public float lookRecognitionTime;
	public bool talking;
	//public GameObject leftController { get { return this.getLeftController (); } }
	//public GameObject rightController { get { return this.getRightController(); } }
	public static readonly Random RAND = new Random();
	private AudioClip sound;
	private AudioClip speechSound;
	private LayerMask layerMask;
	private HeadColliderHandler headColliderHandler;
	private Light[] lightsInScene;
	public LayerMask lightMask;

	private GameObject _leftController = null;
	public GameObject getLeftController() {
		return _leftController == null
			? GameObject.Find ("[CameraRig]").transform.Find("Controller (left)").gameObject
			: _leftController;
	}

	public void setLeftController(GameObject controller){
		if (controller == null)
			throw new System.ArgumentNullException ("controller");
		_leftController = controller;
	}

	private GameObject _rightController = null;
	public GameObject getRightController() {
		return _rightController == null
			? GameObject.Find ("[CameraRig]").transform.Find("Controller (right)").gameObject
			: _rightController;
	}

	public void setRightController(GameObject controller){
		if (controller == null)
			throw new System.ArgumentNullException ("controller");
		_rightController = controller;
	}

	public static EventUtil FindMe() {
		return  GameObject.FindObjectOfType<EventUtil>();
	}

	void Awake() {
		headset = GameObject.Find ("Camera (eye)");
		layerMask = ~LayerMask.GetMask ("RopeIgnore" , "CameraZoneCollisions");
//		leftController = GameObject.Find ("[CameraRig]").transform.Find("Controller (left)").gameObject;
//		rightController = GameObject.Find ("[CameraRig]").transform.Find("Controller (right)").gameObject;
		headColliderHandler = HeadColliderHandler.FindMe ();
		lightMask = ~LayerMask.GetMask ("Unlit");
	}

	void Start() {
		lightsInScene = FindObjectsOfType<Light> ();
		foreach (Light l in lightsInScene) {
			l.cullingMask = lightMask;
		}
	}


	// Update is called once per frame
	void Update () {
		RaycastHit hit;	
//		if (Physics.Raycast (headset.transform.position - new Vector3(0,0.6f,0), headset.transform.forward, out hit, 100, layerMask)) {
		if (Physics.SphereCast (headset.transform.position, 0.5f , headset.transform.forward, out hit, 100, layerMask)) {
			visibleObj = hit.collider.gameObject;

		} else {
			visibleObj = null;
		}
	}

	public GameObject getObject() {
		return visibleObj;
	}

	public bool lookingAtObj(GameObject obj) {
		if (visibleObj && obj && visibleObj.GetInstanceID () == obj.GetInstanceID ()) {
			return true;
		} else
			return false;
	}

	public IEnumerator lookingAtCounter(GameObject obj) {
		float timer = 0f;
		while (timer <= lookRecognitionTime /*&& !this.GetComponent<EventUtil>().lookingAtObj (feedAText)*/) {
			if (lookingAtObj (obj)) {
				timer += 0.25f;
				yield return new WaitForSeconds(0.25f);
				if (timer >= lookRecognitionTime) {
					lookingBool = true;
					yield return new WaitForSeconds(0.25f);
					lookingBool = false;
					yield break;
				}
				yield return null;
			}
			else {
				timer = 0f;
				yield return null;
			}
		}
		yield return null;

	}


	public void changeTex(GameObject obj , GameObject textObj , Texture tex) {
		sound = (AudioClip)Resources.Load ("Audio/windowAudio/textChange");
		obj.GetComponent<AudioSource> ().clip = sound;
		textObj.GetComponent<Renderer>().material.mainTexture = tex;
		obj.GetComponent<AudioSource> ().Play ();
	}



	public void playClip(GameObject obj , AudioClip clip) {
		obj.GetComponent<AudioSource> ().clip = clip;
		obj.GetComponent<AudioSource> ().Play ();
	}

	public void cancelRetract() {
		GameObject leftController = this.getLeftController ();
		GameObject rightController = this.getRightController ();
		leftController.GetComponent<ControllerRetract> ().retracting = false;
		rightController.GetComponent<ControllerRetract> ().retracting = false;
		if (leftController.GetComponent<ControllerRetract> ().retractobj) {
			leftController.GetComponent<ControllerRetract> ().retractobj.GetComponent<Rigidbody> ().isKinematic = false;
			leftController.GetComponent<ControllerRetract> ().retractobj.GetComponent<Rigidbody> ().useGravity = true;
			leftController.GetComponent<ControllerRetract> ().retractobj = null;
		}
		if (leftController.GetComponent<ControllerRetract> ().retractobj) {
			leftController.GetComponent<ControllerRetract> ().retractobj.GetComponent<Rigidbody> ().isKinematic = false;
			leftController.GetComponent<ControllerRetract> ().retractobj.GetComponent<Rigidbody> ().useGravity = true;
			leftController.GetComponent<ControllerRetract> ().retractobj = null;
		}
	}

	public bool FieldOfVision(GameObject lookingActor , float angle = 0.5f , bool isBadguy = true) {
		RaycastHit hit;
		Vector3 correctedPosition;
		Vector3 lookingActorForwardDir;
		if (isBadguy) {
			correctedPosition = lookingActor.transform.position + new Vector3 (0, lookingActor.GetComponent<BoxCollider> ().bounds.size.y - 0.1f, 0);
			lookingActorForwardDir = lookingActor.transform.right;
		} else {
			correctedPosition = lookingActor.transform.position;
			lookingActorForwardDir = lookingActor.transform.forward;
		}
		Vector3 diffVec = headset.transform.position - correctedPosition;
//		GameObject point = GameObject.CreatePrimitive (PrimitiveType.Sphere);
//		point.transform.position = correctedPosition;
//		point.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
//		Destroy (point.GetComponent<SphereCollider>());
		if (Physics.Raycast (correctedPosition, diffVec.normalized, out hit, diffVec.magnitude, layerMask)) {
			if (hit.collider.gameObject.GetInstanceID () == headColliderHandler.gameObject.GetInstanceID ()) {
				if (Vector3.Dot (lookingActorForwardDir.normalized, diffVec.normalized) >= angle) {
					return true;
				}
			}
		}
		return false;
	}

	public WindowTextController GetWindowControllerFromWindow(GameObject windowP) {
		return windowP.transform.Find ("WindowAnim").transform.Find ("windowText").gameObject.GetComponent<WindowTextController> ();
	}

	public Animator GetAnimFromWindow(GameObject windowP) {
		return windowP.transform.Find ("WindowAnim").gameObject.GetComponent<Animator> ();
	}

	public GameObject ObjectInHandCheckLeft() {
		return this.getLeftController().GetComponent<ControllerGrab> ().objectInHand;
	}

	public GameObject ObjectInHandCheckRight() {
		return this.getRightController().GetComponent<ControllerGrab> ().objectInHand;
	}

	public int CheckInstanceID(GameObject obj) {
		return obj == null
			? -1
			: obj.GetInstanceID ();
	}

}
