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
	private EventUtil util;

	void Awake() {
		util = EventUtil.FindMe ();
		PMenu = (GameObject)Resources.Load ("Prefabs/PauseMenu");
		openNoise = (AudioClip)Resources.Load ("Audio/General/pauseMenuOpen");
		closeNoise = (AudioClip)Resources.Load ("Audio/General/pauseMenuClose");
	}

	void Start() {
		head = HeadColliderHandler.FindMe ().transform.parent.gameObject;
		paused = false;
		hideLock = false;
		leftController = util.getLeftController();
		rightController = util.getRightController ();
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
				else if (child.name.Equals("Restart")) 
					child.GetComponent<Renderer> ().material.mainTexture = textures [2];
				else if (child.name.Equals("Quit"))
					child.GetComponent<Renderer> ().material.mainTexture = textures [4];
				else if (child.name.Equals("MainMenu"))
					child.GetComponent<Renderer> ().material.mainTexture = textures [6];
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
		leftController.GetComponent<FunctionController> ().ChangeFunctionStatus (ControllerMode.Mode.Climb , false);
		leftController.GetComponent<FunctionController>().ChangeFunctionStatus (ControllerMode.Mode.Rope , false);
		leftController.GetComponent<FunctionController>().ChangeFunctionStatus (ControllerMode.Mode.RetractShot , false);
		leftController.GetComponent<FunctionController>().ChangeFunctionStatus (ControllerMode.Mode.Fist , false);
		leftController.GetComponent<FunctionController>().ChangeFunctionStatus (ControllerMode.Mode.WebShot , false);

		rightController.GetComponent<FunctionController> ().ChangeFunctionStatus (ControllerMode.Mode.Climb , false);
		rightController.GetComponent<FunctionController>().ChangeFunctionStatus (ControllerMode.Mode.Rope , false);
		rightController.GetComponent<FunctionController>().ChangeFunctionStatus (ControllerMode.Mode.RetractShot , false);
		rightController.GetComponent<FunctionController>().ChangeFunctionStatus (ControllerMode.Mode.Fist , false);
		rightController.GetComponent<FunctionController>().ChangeFunctionStatus (ControllerMode.Mode.WebShot , false);
	}

	public void StoreFunctions() {
		leftStatus[0] = leftController.GetComponent<FunctionController>().GetFunctionStatus(ControllerMode.Mode.Climb);
		leftStatus[1] = leftController.GetComponent<FunctionController>().GetFunctionStatus(ControllerMode.Mode.Rope);
		leftStatus[2] = leftController.GetComponent<FunctionController>().GetFunctionStatus(ControllerMode.Mode.RetractShot);
		leftStatus[3] = leftController.GetComponent<FunctionController>().GetFunctionStatus(ControllerMode.Mode.Fist);
		leftStatus[4] = leftController.GetComponent<FunctionController>().GetFunctionStatus(ControllerMode.Mode.WebShot);

		rightStatus[0] = rightController.GetComponent<FunctionController>().GetFunctionStatus(ControllerMode.Mode.Climb);
		rightStatus[1] = rightController.GetComponent<FunctionController>().GetFunctionStatus(ControllerMode.Mode.Rope);
		rightStatus[2] = rightController.GetComponent<FunctionController>().GetFunctionStatus(ControllerMode.Mode.RetractShot);
		rightStatus[3] = rightController.GetComponent<FunctionController>().GetFunctionStatus(ControllerMode.Mode.Fist);
		rightStatus[4] = rightController.GetComponent<FunctionController>().GetFunctionStatus(ControllerMode.Mode.WebShot);

	}

	public void RestoreFunctions() {
		leftController.GetComponent<FunctionController> ().ChangeFunctionStatus (ControllerMode.Mode.Climb, leftStatus [0]);
		leftController.GetComponent<FunctionController>().ChangeFunctionStatus(ControllerMode.Mode.Rope , leftStatus[1]);
		leftController.GetComponent<FunctionController>().ChangeFunctionStatus(ControllerMode.Mode.RetractShot , leftStatus[2]);
		leftController.GetComponent<FunctionController>().ChangeFunctionStatus(ControllerMode.Mode.Fist , leftStatus[3]);
		leftController.GetComponent<FunctionController>().ChangeFunctionStatus(ControllerMode.Mode.WebShot , leftStatus[4]);

		rightController.GetComponent<FunctionController> ().ChangeFunctionStatus (ControllerMode.Mode.Climb, rightStatus [0]);
		rightController.GetComponent<FunctionController>().ChangeFunctionStatus(ControllerMode.Mode.Rope , rightStatus[1]);
		rightController.GetComponent<FunctionController>().ChangeFunctionStatus(ControllerMode.Mode.RetractShot , rightStatus[2]);
		rightController.GetComponent<FunctionController>().ChangeFunctionStatus(ControllerMode.Mode.Fist , rightStatus[3]);
		rightController.GetComponent<FunctionController>().ChangeFunctionStatus(ControllerMode.Mode.WebShot , rightStatus[4]);
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
	}

}
