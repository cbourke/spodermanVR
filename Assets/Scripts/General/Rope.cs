/*
 * This script strictly handles the preview mode of the Rope mode. It continuously checks to see if the controller is in 
 * Rope mode. If it is, preview mode is enabled. If it is not, preview mode is disabled and the preview node is destroyed. 
 * To be specific, preview mode just spawns a transparent primitive sphere that moves wherever the player is pointing. If the raycast isn't pointing
 * in a valid spot, the sphere doesn't move to it. 
 * 
 * To setup, an empty gameObject in the scene with the WorldNodeTracker script attached to it must be assigned to the public GameObject of 
 * worldNodeTracker here. I should make this less confusing, but oh well. This script must also be attached to both controllers. The public Material 
 * fields of valid and notValid must also be preset, using the ValidPreviewNode and NonValidPreviewNode Materials in the Materials folder. 
 * TODO: handle what happens when the player is in preview mode, but not pointing at a valid hit spot. (Valid as in raycasting and colliding.)
 * 
 * Additional setup also includes adding a global layer to the scene. Specifically, layer8 = PreviewNode.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour {

	public SteamVR_TrackedObject trackedObj;
	private GameObject previewNode;
	public GameObject worldNodeTracker;
	public Material notValid;
	public Material valid;
	public LayerMask layerMask;
	public float shotDistance;
	private int layerMaskInt;
	private GameObject laser;


	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input ((int)trackedObj.index); }
	}

	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		valid = (Material)Resources.Load ("Materials/General/ValidPreviewNode");
		notValid  = (Material)Resources.Load("Materials/General/NonValidPreviewNode");
		worldNodeTracker = GameObject.Find("WorldNodeTracker");
		layerMaskInt = ~layerMask.value;
		laser = Instantiate ((GameObject)Resources.Load("Prefabs/TrailingLaser"));
//		layerMask = 1 << 8;
//		layerMask = ~layerMask;
	}

	// Use this for initialization
	void Start () {
		laser.GetComponent<DottedLineRenderer> ().outward = true;
	}


		

	public void createPreviewNode () {
		previewNode = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		previewNode.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);
		previewNode.GetComponent<Renderer> ().material = notValid;
		Destroy (previewNode.GetComponent<SphereCollider>());
//		previewNode.layer = 8;
//		previewNode.AddComponent<IgnoreCollisions> ();
	}

	public Vector3 getValidNodePosition () {

		RaycastHit hit;
		Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, 30, layerMaskInt);
		return hit.point;

	}

	public bool isValidNode () {

		RaycastHit hit;
		if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, shotDistance, layerMaskInt)) { //if raycast hits an object
			if (hit.collider.gameObject.CompareTag ("Climbable") || hit.collider.gameObject.CompareTag ("Rope")) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}

	}

	public void spawnNodeBridge (Vector3 point , GameObject callingController) {
		worldNodeTracker.GetComponent<WorldRopeNodeTracker> ().spawnNode (point , callingController);
	}
	
	// Update is called once per frame
	void Update () {

		if (this.GetComponent<FunctionController> ().currentMode.ToString () == "Rope" && !worldNodeTracker.GetComponent<PauseMenuWorld>().paused) {
			if (!previewNode) {
				createPreviewNode ();

			}

			RaycastHit hit;

			if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, shotDistance  , layerMaskInt)) { //if raycast hits an object

				previewNode.transform.position = hit.point;
				ShowLaser (hit);

				if (hit.collider.gameObject.CompareTag ("Climbable") || hit.collider.gameObject.CompareTag ("Rope")) {
					previewNode.GetComponent<Renderer> ().material = valid;
					worldNodeTracker.GetComponent<WorldRopeNodeTracker> ().ropePreview (hit.point);
					laser.GetComponent<DottedLineRenderer> ().valid = true;

				} else {
					previewNode.GetComponent<Renderer> ().material = notValid;
					worldNodeTracker.GetComponent<WorldRopeNodeTracker> ().destroyPreviewRope();
					laser.GetComponent<DottedLineRenderer> ().valid = false;
				}
					
			} else {
				DestroyImmediate (previewNode,false);
				worldNodeTracker.GetComponent<WorldRopeNodeTracker> ().destroyPreviewRope();
				laser.SetActive (false);

			}


		} else {
			DestroyImmediate (previewNode,false);
			laser.SetActive (false);
//			worldNodeTracker.GetComponent<WorldRopeNodeTracker> ().destroyPreviewRope();

		}
		
	}

	private void ShowLaser(RaycastHit hit)
	{
		laser.SetActive(true);
		//		laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
		//		laserTransform.LookAt(hitPoint); 
		//		laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
		laser.transform.position = trackedObj.transform.position;
		laser.GetComponent<LineRenderer> ().SetPosition (0 , trackedObj.transform.position);
		laser.GetComponent<LineRenderer> ().SetPosition (1 , hit.point);

	}
}
