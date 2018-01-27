using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRetract : MonoBehaviour {
	public GameObject laser;
	public GameObject laserPrefab;
	public Transform laserTransform;
	public GameObject retractobj;	
	public GameObject objectInHand;
	public float retractSpeed;
	public bool retracting;

	private int layerMask;
	private Vector3 hitPoint; 
	private SteamVR_TrackedObject trackedObj;
	private GameObject worldTracker;
//	private GameObject ropePreview;


	public void Awake()
    {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		laserPrefab = (GameObject)Resources.Load ("Prefabs/Laser");
		retracting = false;
		worldTracker = GameObject.Find ("WorldNodeTracker");
    }
	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	public void Start() {
		laser = Instantiate (laserPrefab);
		laserTransform = laser.transform;
		layerMask = 1 << 8;
		layerMask = ~layerMask;
	}
	
	private void ShowLaser(RaycastHit hit)
	{
		laser.SetActive(true);
		laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
		laserTransform.LookAt(hitPoint); 
		laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);

	}

	public GameObject retract(){

		if (this.GetComponent<ControllerGrab> ().objectInHand) {
			laser.SetActive (false);
			return null;
		}

		if (this.GetComponent<FunctionController> ().currentMode.ToString () == "RetractShot") {
			RaycastHit hit;	
			if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, 100,layerMask)) {
				if (hit.collider.gameObject.GetComponent<Rigidbody>() && !hit.collider.gameObject.GetComponent<Rigidbody> ().isKinematic && hit.collider.gameObject.GetComponent<Rigidbody> ().useGravity) {
					hit.collider.gameObject.transform.position = trackedObj.transform.position;
					return hit.collider.gameObject;
				}
			}
		}
		return null;
	}

	public void retractNew() {
		
		if (this.GetComponent<ControllerGrab> ().objectInHand) {
			laser.SetActive (false);
			return;
		}
		if (this.GetComponent<FunctionController> ().currentMode.ToString () == "RetractShot" && !retracting) {
			RaycastHit hit;	
			if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, 100,layerMask)) {
				if (hit.collider.gameObject.GetComponent<Rigidbody>() && !hit.collider.gameObject.GetComponent<Rigidbody> ().isKinematic && hit.collider.gameObject.GetComponent<Rigidbody> ().useGravity) {
                    //hit.collider.gameObject.transform.position = trackedObj.transform.position;
                    //return hit.collider.gameObject;
                    StartCoroutine(pull(hit.collider.gameObject));
				}
			}
		}
	}


	public IEnumerator pull(GameObject pullObj) {   //from here, the OnTriggerEnter in ControllerGrab handles when the object contacts the controller. This coroutine just keeps running until then.
		pullObj.GetComponent<Rigidbody>().useGravity = false;
		pullObj.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		pullObj.GetComponent<Rigidbody>().isKinematic = true;

		retracting = true;
		retractobj = pullObj;
		//float step = retractSpeed * Time.deltaTime;
		while (retracting) {
			pullObj.transform.position = Vector3.MoveTowards (pullObj.transform.position , this.transform.position , retractSpeed * Time.deltaTime);
			yield return null;
		}
	}
	

	public void Update() {

		if (this.GetComponent<FunctionController> ().currentMode.ToString () == "RetractShot" && !retracting && !worldTracker.GetComponent<PauseMenuWorld>().paused) {

			if (this.GetComponent<ControllerGrab> ().objectInHand) {
				laser.SetActive (false);
				return;
			}

			RaycastHit hit;	
			if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, 100, layerMask)) {
				hitPoint = hit.point;
				ShowLaser (hit);

			} else {
				laser.SetActive (false);
			}
		} else {
			laser.SetActive (false);
			if (this.GetComponent<ControllerGrab>().objectInHand && !(this.GetComponent<FunctionController>().currentMode.ToString() == "Climb")) {
				this.GetComponent<ControllerGrab> ().UnGrab ();
			}
//			if (ropePreview) {
//
//			} else {
//
//			}
		}

	}

}

