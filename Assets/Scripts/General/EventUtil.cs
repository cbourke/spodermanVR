using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventUtil : MonoBehaviour {

	public GameObject headset;
	public GameObject visibleObj;
	public bool lookingBool;
	public float lookRecognitionTime;
	public bool talking;
	public GameObject leftController;
	public GameObject rightController;
	public static readonly Random RAND = new Random();
	private AudioClip sound;
	private AudioClip speechSound;
	private LayerMask layerMask;
	private HeadColliderHandler headColliderHandler;

	public static EventUtil FindMe() {
		return  GameObject.FindObjectOfType<EventUtil>();
	}

	void Awake() {
		headset = GameObject.Find ("Camera (eye)");
		layerMask = ~LayerMask.GetMask ("RopeIgnore" , "CameraZoneCollisions");
		leftController = GameObject.Find ("[CameraRig]").transform.Find("Controller (left)").gameObject;
		rightController = GameObject.Find ("[CameraRig]").transform.Find("Controller (right)").gameObject;
		headColliderHandler = HeadColliderHandler.FindMe ();
	}


	// Update is called once per frame
	void Update () {
		RaycastHit hit;	
		if (Physics.Raycast (headset.transform.position, headset.transform.forward, out hit, 100, layerMask)) {
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

	public IEnumerator faceTalk(GameObject face , GameObject speechBubble , Texture faceTex , Texture text , string pitch) {
		speechBubble.SetActive (true);
		int clipNum = Random.Range (1,4);
		string clipSelect = "Audio/Speech/" + clipNum.ToString () + pitch;
		Texture oldFace = face.GetComponent<Renderer> ().material.mainTexture;
		face.GetComponent<AudioSource> ().clip = (AudioClip)Resources.Load (clipSelect);
		face.GetComponent<Renderer> ().material.mainTexture = faceTex;
		speechBubble.GetComponent<Renderer> ().material.mainTexture = text;
		face.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (1);
		face.GetComponent<Renderer> ().material.mainTexture = oldFace;
		yield return new WaitForSeconds (GetComponent<LevelEvents1_2>().feedDelay);
		speechBubble.SetActive (false);
	}

	public void playClip(GameObject obj , AudioClip clip) {
		obj.GetComponent<AudioSource> ().clip = clip;
		obj.GetComponent<AudioSource> ().Play ();
	}

	public void cancelRetract() {
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

	public bool FieldOfVision(GameObject lookingActor , float angle = 0.5f) {
		RaycastHit hit;
		Vector3 correctedPosition = lookingActor.transform.position + new Vector3 (0 , lookingActor.GetComponent<BoxCollider> ().bounds.size.y - 0.1f , 0);
		Vector3 diffVec = headset.transform.position - correctedPosition;
//		GameObject point = GameObject.CreatePrimitive (PrimitiveType.Sphere);
//		point.transform.position = correctedPosition;
//		point.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
//		Destroy (point.GetComponent<SphereCollider>());
		if (Physics.Raycast (correctedPosition, diffVec.normalized, out hit, diffVec.magnitude, layerMask)) {
			if (hit.collider.gameObject.GetInstanceID () == headColliderHandler.gameObject.GetInstanceID ()) {
				if (Vector3.Dot (lookingActor.transform.right.normalized, diffVec.normalized) >= angle) {
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
		return leftController.GetComponent<ControllerGrab> ().objectInHand;
	}

	public GameObject ObjectInHandCheckRight() {
		return rightController.GetComponent<ControllerGrab> ().objectInHand;
	}

	public int CheckInstanceID(GameObject obj) {
		return obj == null
			? -1
			: obj.GetInstanceID ();
	}
		

}
