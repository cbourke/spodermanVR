using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRetract : MonoBehaviour {
	public GameObject laser;
	public GameObject laserPrefab;
	public Transform laserTransform;
	public GameObject retractobj;	
	public float retractSpeed;
	public bool retracting;
	public float shotDistance;
	public LayerMask layerMask;
	private Vector3 hitPoint; 
	private SteamVR_TrackedObject trackedObj;
	private GameObject worldTracker;
//	private GameObject ropePreview;


	public void Awake()
    {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		laserPrefab = (GameObject)Resources.Load ("Prefabs/TrailingLaser");
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
		laser.GetComponent<DottedLineRenderer> ().outward = false;
		layerMask = LayerMask.GetMask ("RopeIgnore" , "CameraZoneCollisions");

	}
	
	private void ShowLaser(RaycastHit hit)
	{
		laser.SetActive(true);
		laserTransform.position = trackedObj.transform.position;
		laser.GetComponent<LineRenderer> ().SetPosition (0 , trackedObj.transform.position);
		laser.GetComponent<LineRenderer> ().SetPosition (1 , hit.point);

	}

	public void retractNew() {
		
		if (this.GetComponent<ControllerGrab> ().objectInHand) {
			laser.SetActive (false);
			return;
		}
		if (this.GetComponent<FunctionController> ().currentMode.ToString () == "RetractShot" && !retracting) {
			RaycastHit hit;	
			if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, shotDistance, ~layerMask)) {
				if (hit.collider.gameObject.GetComponent<Rigidbody>() 
					&& ((!hit.collider.gameObject.GetComponent<Rigidbody> ().isKinematic && hit.collider.gameObject.GetComponent<Rigidbody> ().useGravity) || hit.collider.gameObject.CompareTag("Retractable"))) {
                    StartCoroutine(pull(hit.collider.gameObject));
				}
			}
		}
	}


	public IEnumerator pull(GameObject pullObj) {   //from here, the OnTriggerEnter in ControllerGrab handles when the object contacts the controller. This coroutine just keeps running until then.
		pullObj.GetComponent<Rigidbody>().useGravity = false;
		pullObj.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		retracting = true;
		pullObj.GetComponent<Rigidbody> ().isKinematic = !pullObj.CompareTag ("Retractable");

		retractobj = pullObj;
		while (retracting) {
			pullObj.transform.position = Vector3.MoveTowards (pullObj.transform.position , this.transform.position , retractSpeed * Time.deltaTime);
			yield return null;
		}
	}
	

	public void Update() {

		if (retractobj != null) {
			IPersistObject o = retractobj.GetComponent<IPersistObject> ();
			if (o != null) {
				o.Persist ();
			}
		}

		if (this.GetComponent<FunctionController> ().currentMode.ToString () == "RetractShot" && !retracting && !worldTracker.GetComponent<PauseMenuWorld>().paused) {

			if (this.GetComponent<ControllerGrab> ().objectInHand) {
				laser.SetActive (false);
				return;
			}

			RaycastHit hit;	
			if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, shotDistance, ~layerMask)) {
				hitPoint = hit.point;
				ShowLaser (hit);
				if (hit.collider.gameObject.GetComponent<Rigidbody> () && ((!hit.collider.gameObject.GetComponent<Rigidbody> ().isKinematic && hit.collider.gameObject.GetComponent<Rigidbody> ().useGravity) || hit.collider.gameObject.CompareTag ("Retractable"))) {
					laser.GetComponent<DottedLineRenderer> ().valid = true;
				} else {
					laser.GetComponent<DottedLineRenderer> ().valid = false;
				}

			} else {
				laser.SetActive (false);
			}
		} else {
			laser.SetActive (false);
			if (this.GetComponent<ControllerGrab>().objectInHand && !(this.GetComponent<FunctionController>().currentMode.ToString() == "Climb")) {
				this.GetComponent<ControllerGrab> ().UnGrab ();
			}
		}

	}

}

