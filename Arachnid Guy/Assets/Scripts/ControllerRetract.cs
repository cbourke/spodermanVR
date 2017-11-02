using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRetract : MonoBehaviour {
	public GameObject laser;
	public GameObject laserPrefab;
	public Transform laserTransform;
	private Vector3 hitPoint; 
    private SteamVR_TrackedObject trackedObj; 
	private Vector3 retractobj;	
	public GameObject objectInHand;
	private int layerMask;

	public void Awake()
    {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		laserPrefab = (GameObject)Resources.Load ("Prefabs/Laser");
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
		laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
        hit.distance);

	}

	public GameObject retract(){
		if (this.GetComponent<FunctionController> ().currentMode.ToString () == "RetractShot") {
			RaycastHit hit;	
			if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, 100,layerMask)) {
				if (!hit.collider.gameObject.GetComponent<Rigidbody> ().isKinematic && hit.collider.gameObject.GetComponent<Rigidbody> ().useGravity) {
					hit.collider.gameObject.transform.position = trackedObj.transform.position;
					return hit.collider.gameObject;
				}
			}
		}
		return null;
	}
	

	public void Update() {

		if (this.GetComponent<FunctionController> ().currentMode.ToString () == "RetractShot") {
			RaycastHit hit;	
			if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, 100,layerMask)) {
				hitPoint = hit.point;
				ShowLaser (hit);

			} else {
				laser.SetActive (false);
			}
		} else
			laser.SetActive (false);

	}

}

