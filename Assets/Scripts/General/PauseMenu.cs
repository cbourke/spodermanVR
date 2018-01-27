using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
//	public GameObject PMenu;
	public Texture[] texture = new Texture[6] ;
	public GameObject otherController;
	public GameObject laser;
	public GameObject laserPrefab;
	public Transform laserTransform;
	public GameObject head;
	public float headOffset;
	public GameObject worldTracker;
	public LayerMask defaultLayer;
	private Vector3 hitPoint; 
    private SteamVR_TrackedObject trackedObj; 
	private GameObject cameraRig;
	//private int layerMask;
	private bool climbTemp;
	private bool ropeTemp;
	private bool retractTemp;
	private bool fistTemp;
	private bool shotTemp;
	public bool pause = false;


	
	
	private SteamVR_Controller.Device Controller { get { return SteamVR_Controller.Input((int) trackedObj.index); } } 

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
//		PMenu = GameObject.Find ("PauseMenu");
//		laser = GameObject.Find ("Laser");
		laserPrefab = (GameObject)Resources.Load("Prefabs/pauseLaser");
		head = GameObject.Find ("Camera (eye)");
		cameraRig = GameObject.Find ("[CameraRig]");
		worldTracker = GameObject.Find ("WorldNodeTracker");

	}

	void Start()
	{
//		if (!PMenu)
//			Debug.Log ("Pause Menu object not found!");
//		else
//			PMenu.SetActive (false);

//		if (!laser)
//			Debug.Log ("Laser object not found!");
//		else {
//			laserTransform = laser.transform;
//			laser.SetActive (false);
//		}
		laser = Instantiate(laserPrefab);
		laserTransform = laser.transform;
		if (!head)
			Debug.Log ("Head object not found!");
		
//		layerMask = 1 << 8;
//		layerMask = ~layerMask;
	}

	




	public void ShowLaser(RaycastHit hit)
	{
		laser.SetActive(true);
		laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
		laserTransform.LookAt(hitPoint); 
		laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);

	}
	



	void Update()
	{			
		if ((Controller.GetPressDown (3) || Controller.GetPressDown (1)) && !otherController.GetComponent<PauseMenu>().pause) {
			if (!pause) {	
				pause = true;	
				climbTemp = this.GetComponent<FunctionController> ().climbEnabled;
				ropeTemp = this.GetComponent<FunctionController> ().ropeEnabled;
				retractTemp = this.GetComponent<FunctionController> ().retractEnabled;
				fistTemp = this.GetComponent<FunctionController> ().fistEnabled;
				shotTemp = this.GetComponent<FunctionController> ().shotEnabled;
				worldTracker.GetComponent<PauseMenuWorld>().ShowMenu (this.gameObject, otherController);
				Time.timeScale = 0.00000001F;
				Time.fixedDeltaTime = 0.00000001F;

			}
		
			else
			{
				pause = false;
				hideMenuLocal();
				Time.timeScale = 1F;
				Time.fixedDeltaTime = 1f;
			}
		}

		if (pause) {
			RaycastHit hit;	

			if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, 100, defaultLayer)) {
				hitPoint = hit.point;
				ShowLaser (hit);
			} else {
				laser.SetActive (false);
			}

			if (hit.collider && (hitCheck("Restart",hit) || hitCheck("Quit",hit) || hitCheck("Continue",hit))) {
				if (hitCheck("Continue",hit)) {
					if (hit.collider.gameObject.GetComponent<Renderer> ().material.mainTexture != texture[1])
						hit.collider.gameObject.GetComponent<Renderer> ().material.mainTexture = texture[1];

					if (Controller.GetHairTriggerDown ()) {
						hideMenuLocal ();
						Time.timeScale = 1F;
						Time.fixedDeltaTime = 1f;
					}
					
				} else if (hitCheck("Restart",hit)) {
					if (hit.collider.gameObject.GetComponent<Renderer> ().material.mainTexture != texture[3]) {
						hit.collider.gameObject.GetComponent<Renderer> ().material.mainTexture = texture[3];
					}
								
					if (Controller.GetHairTriggerDown ()) {
						hideMenuLocal ();
						Time.timeScale = 1F;
						Time.fixedDeltaTime = 1f;
						Scene loadedLevel = SceneManager.GetActiveScene ();
						SceneManager.LoadScene (loadedLevel.buildIndex);
					}
				} else if (hitCheck("Quit",hit)) {
					if (hit.collider.gameObject.GetComponent<Renderer> ().material.mainTexture != texture[5])
						hit.collider.gameObject.GetComponent<Renderer> ().material.mainTexture = texture[5];
						
					if (Controller.GetHairTriggerDown ()) {
						Time.timeScale = 1F;
						Time.fixedDeltaTime = 1f;
						Application.Quit ();
					}
				}
			}
			 else {
				worldTracker.GetComponent<PauseMenuWorld> ().revertButtons(texture);
//				//if (GameObject.Find ("Continue").GetComponent<Renderer> ().material.mainTexture != texture [0]) 
//					GameObject.Find ("Continue").GetComponent<Renderer> ().material.mainTexture = texture [0];
//
//				//if (GameObject.Find ("Restart").GetComponent<Renderer> ().material.mainTexture != texture[2])
//					GameObject.Find ("Restart").GetComponent<Renderer> ().material.mainTexture = texture[2];
//				//if (GameObject.Find ("Quit").GetComponent<Renderer> ().material.mainTexture != texture[4])
//					GameObject.Find ("Quit").GetComponent<Renderer> ().material.mainTexture = texture[4];
			}
		}

	}	

	public void hideMenuLocal() {
		laser.SetActive (false);
		this.GetComponent<FunctionController>().climbEnabled = climbTemp;
		this.GetComponent<FunctionController>().ropeEnabled = ropeTemp;
		this.GetComponent<FunctionController>().retractEnabled = retractTemp;
		this.GetComponent<FunctionController>().fistEnabled = fistTemp;
		this.GetComponent<FunctionController>().shotEnabled = shotTemp;
		otherController.GetComponent<FunctionController>().climbEnabled = climbTemp;
		otherController.GetComponent<FunctionController>().ropeEnabled = ropeTemp;
		otherController.GetComponent<FunctionController>().retractEnabled = retractTemp;
		otherController.GetComponent<FunctionController>().fistEnabled = fistTemp;
		otherController.GetComponent<FunctionController>().shotEnabled = shotTemp;
		worldTracker.GetComponent<PauseMenuWorld> ().HideMenu ();
		StartCoroutine (setPauseFalse());

	}

	public bool hitCheck(string name , RaycastHit hit) {
		if (hit.collider.gameObject.name == name) {
			return true;
		}
		else return false;

	}

	private IEnumerator setPauseFalse() {
		yield return new WaitForSecondsRealtime(0.201f);
		pause = false;
	}
}