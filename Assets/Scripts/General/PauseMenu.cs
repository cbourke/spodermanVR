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
	public GameObject worldTracker;
	public LayerMask pauseLayer;
	private Vector3 hitPoint; 
    private SteamVR_TrackedObject trackedObj; 
	private bool climbTemp;
	private bool ropeTemp;
	private bool retractTemp;
	private bool fistTemp;
	private bool shotTemp;
	public bool pause;


	
	
	private SteamVR_Controller.Device Controller { get { return SteamVR_Controller.Input((int) trackedObj.index); } } 

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
//		PMenu = GameObject.Find ("PauseMenu");
//		laser = GameObject.Find ("Laser");
		laserPrefab = (GameObject)Resources.Load("Prefabs/pauseLaser");
		head = GameObject.Find ("Camera (eye)");
		worldTracker = GameObject.Find ("WorldNodeTracker");
		if (this.name.Equals("Controller (left)")) 
			otherController = GameObject.Find ("[CameraRig]").transform.Find ("Controller (right)").gameObject;
		else 
			otherController = GameObject.Find ("[CameraRig]").transform.Find ("Controller (left)").gameObject;
		
	

	}

	void Start()
	{
		laser = Instantiate(laserPrefab);
		laserTransform = laser.transform;
		if (!head)
			Debug.Log ("Head object not found!");
		pauseLayer = LayerMask.GetMask ("PauseMenu");


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
		if ((Controller.GetPressDown(3) || Controller.GetPressDown (1)) && !otherController.GetComponent<PauseMenu>().pause) {
			if (!pause && validDistanceChecker()) {
				worldTracker.GetComponent<PauseMenuWorld>().ShowMenu (this.gameObject);
			}
			else {
				worldTracker.GetComponent<PauseMenuWorld> ().HideMenu (this.gameObject);
				laser.SetActive (false);
			}
		}


		if (pause) {
			RaycastHit hit;	
			if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, 100, pauseLayer)) {
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
						worldTracker.GetComponent<PauseMenuWorld> ().HideMenu (this.gameObject);
					}
					
				} else if (hitCheck("Restart",hit)) {
					if (hit.collider.gameObject.GetComponent<Renderer> ().material.mainTexture != texture[3]) {
						hit.collider.gameObject.GetComponent<Renderer> ().material.mainTexture = texture[3];
					}
								
					if (Controller.GetHairTriggerDown ()) {
						worldTracker.GetComponent<PauseMenuWorld> ().HideMenu (this.gameObject);
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
			}
		}

	}	

//	public void hideMenuLocal() {
//		
//		this.GetComponent<FunctionController>().climbEnabled = climbTemp;
//		this.GetComponent<FunctionController>().ropeEnabled = ropeTemp;
//		this.GetComponent<FunctionController>().retractEnabled = retractTemp;
//		this.GetComponent<FunctionController>().fistEnabled = fistTemp;
//		this.GetComponent<FunctionController>().shotEnabled = shotTemp;
//		otherController.GetComponent<FunctionController>().climbEnabled = climbTemp;
//		otherController.GetComponent<FunctionController>().ropeEnabled = ropeTemp;
//		otherController.GetComponent<FunctionController>().retractEnabled = retractTemp;
//		otherController.GetComponent<FunctionController>().fistEnabled = fistTemp;
//		otherController.GetComponent<FunctionController>().shotEnabled = shotTemp;
//		worldTracker.GetComponent<PauseMenuWorld> ().HideMenu ();
//		//StartCoroutine (setPauseFalse());
//
//	}

	public bool hitCheck(string name , RaycastHit hit) {
		if (hit.collider.gameObject.name == name) {
			return true;
		}
		else return false;

	}

	public bool validDistanceChecker () {
		RaycastHit hit;
		if (Physics.Raycast (head.transform.position, head.transform.forward, out hit, worldTracker.GetComponent<PauseMenuWorld> ().headOffset)) { //if raycast hits an object
			GetComponent<AudioSource>().clip = (AudioClip)Resources.Load("Audio/windowAudio/error");
			GetComponent<AudioSource> ().Play();
			return false;
		} else {
			return true;
		}
	}
}