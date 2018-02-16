using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuWorld : MonoBehaviour {

	public GameObject PMenu;
	//public GameObject pmenuInst;
	public GameObject head;
	public float headOffset;
	public bool paused;
	public List<GameObject> list = new List<GameObject> ();
	public GameObject pMenuInst;
	private AudioClip openNoise;
	private AudioClip closeNoise;
	public GameObject leftController;
	public GameObject rightController;
	private bool hideLock;
	private bool[] rightStatus = new bool[5];		//[climbEnabled , ropeEnabled , retractEnabled , fistEnabled , shotEnabled]
	private bool[] leftStatus = new bool[5];

	void Awake() {
		PMenu = (GameObject)Resources.Load ("Prefabs/PauseMenu");
		openNoise = (AudioClip)Resources.Load ("Audio/General/pauseMenuOpen");
		closeNoise = (AudioClip)Resources.Load ("Audio/General/pauseMenuClose");
	}

	void Start() {
		head = GameObject.Find ("Camera (eye)");
		paused = false;
		hideLock = false;
		leftController = GameObject.Find ("[CameraRig]").transform.Find("Controller (left)").gameObject;
		rightController = GameObject.Find ("[CameraRig]").transform.Find("Controller (right)").gameObject;
	}

	public void ShowMenu(GameObject sendingController)
	{
		if (pMenuInst == null) {
			pMenuInst = Instantiate (PMenu);
			paused = true;
			sendingController.GetComponent<PauseMenu> ().pause = true;
			StoreFunctions ();
			DisableFunctions ();
			pMenuInst.transform.position = head.transform.position;
			pMenuInst.transform.position += head.transform.forward * headOffset;
			pMenuInst.transform.position -= new Vector3 (0,0.3f,0);
			pMenuInst.transform.rotation = Quaternion.identity;
			pMenuInst.transform.Rotate (90f,head.transform.rotation.eulerAngles.y - 180f,0f);
			pMenuInst.GetComponent<AudioSource> ().clip = openNoise;
			pMenuInst.GetComponent<AudioSource> ().Play ();
			Time.timeScale = 0.00000001F;
			Time.fixedDeltaTime = 0.00000001F;
		}
			

	}

	public void revertButtons (Texture[] textures) {
		if (pMenuInst != null) {
			foreach (Transform child in pMenuInst.transform) {
				if (child.name.Equals("Continue"))  
					child.GetComponent<Renderer> ().material.mainTexture = textures [0];
				if (child.name.Equals("Restart")) 
					child.GetComponent<Renderer> ().material.mainTexture = textures [2];
				if (child.name.Equals("Quit"))
					child.GetComponent<Renderer> ().material.mainTexture = textures [4];
			}
		}
	}



	public void HideMenu(GameObject sendingController)
	{
		if (pMenuInst != null && !hideLock) {
			hideLock = true;
			pMenuInst.GetComponent<AudioSource> ().clip = closeNoise;
			pMenuInst.GetComponent<AudioSource> ().Play ();
			pMenuInst.GetComponent<MeshRenderer> ().enabled = false;
			foreach (Transform child in pMenuInst.transform) {
				child.GetComponent<MeshRenderer> ().enabled = false;
			}

			StartCoroutine (DestroyMenu(sendingController));
	
		}

	}

	public void DisableFunctions() {
		leftController.GetComponent<FunctionController>().climbEnabled = false;
		leftController.GetComponent<FunctionController>().ropeEnabled = false;
		leftController.GetComponent<FunctionController>().retractEnabled = false;
		leftController.GetComponent<FunctionController>().fistEnabled = false;
		leftController.GetComponent<FunctionController>().shotEnabled = false;

		rightController.GetComponent<FunctionController>().climbEnabled = false;
		rightController.GetComponent<FunctionController>().ropeEnabled = false;
		rightController.GetComponent<FunctionController>().retractEnabled = false;
		rightController.GetComponent<FunctionController>().fistEnabled = false;
		rightController.GetComponent<FunctionController>().shotEnabled = false;
	}

	public void StoreFunctions() {
		leftStatus[0] = leftController.GetComponent<FunctionController>().climbEnabled;
		leftStatus[1] = leftController.GetComponent<FunctionController>().ropeEnabled;
		leftStatus[2] = leftController.GetComponent<FunctionController>().retractEnabled;
		leftStatus[3] = leftController.GetComponent<FunctionController>().fistEnabled;
		leftStatus[4] = leftController.GetComponent<FunctionController>().shotEnabled;

		rightStatus[0] = rightController.GetComponent<FunctionController>().climbEnabled;
		rightStatus[1] = rightController.GetComponent<FunctionController>().ropeEnabled;
		rightStatus[2] = rightController.GetComponent<FunctionController>().retractEnabled;
		rightStatus[3] = rightController.GetComponent<FunctionController>().fistEnabled;
		rightStatus[4] = rightController.GetComponent<FunctionController>().shotEnabled;

	}

	public void RestoreFunctions() {
		leftController.GetComponent<FunctionController>().climbEnabled = leftStatus[0];
		leftController.GetComponent<FunctionController>().ropeEnabled = leftStatus[1];
		leftController.GetComponent<FunctionController>().retractEnabled = leftStatus[2];
		leftController.GetComponent<FunctionController>().fistEnabled = leftStatus[3];
		leftController.GetComponent<FunctionController>().shotEnabled = leftStatus[4];

		rightController.GetComponent<FunctionController>().climbEnabled = rightStatus[0];
		rightController.GetComponent<FunctionController>().ropeEnabled = rightStatus[1];
		rightController.GetComponent<FunctionController>().retractEnabled = rightStatus[2];
		rightController.GetComponent<FunctionController>().fistEnabled = rightStatus[3];
		rightController.GetComponent<FunctionController>().shotEnabled = rightStatus[4];
	}



	private IEnumerator DestroyMenu(GameObject sendCont) {
		yield return new WaitForSecondsRealtime(.201f);
		Time.timeScale = 1F;
		Time.fixedDeltaTime = 1f;
		RestoreFunctions ();
		paused = false;
		sendCont.GetComponent<PauseMenu> ().pause = false;
		sendCont.GetComponent<PauseMenu> ().laser.SetActive (false);
		Destroy (pMenuInst);
		hideLock = false;
		//pmenuInst = null;
	}

}
