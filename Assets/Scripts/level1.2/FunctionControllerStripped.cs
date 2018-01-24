
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionControllerStripped : MonoBehaviour {

	public enum Mode {
		Climb,
		Rope,
		WebShot,
		RetractShot,
		Fist,
		Nothing,
	};


	public SteamVR_TrackedObject trackedObj;
	public bool isClimbing;
	public Mode currentMode = Mode.Nothing;
	public bool climbEnabled;
	public bool ropeEnabled;
	public bool retractEnabled;
	public bool fistEnabled;
	public bool shotEnabled;
	private GameObject ind;
	private Vector2 touchPadAxis;


	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input ((int)trackedObj.index); }
	}

	void Awake() {
		climbEnabled = true;
		ropeEnabled = true;
		retractEnabled = true;
		fistEnabled = true;
		shotEnabled = true;
		trackedObj = GetComponent<SteamVR_TrackedObject> ();

	}


	private void Start()
	{
		isClimbing = false;
		changeIndicator (currentMode);
	}

	/*
	 * Indicator should only be visibly changed when switching to an input for the first time, not on subsequent switches!
	 * This method may become obsolete after application of hand models down the road. This is good for early releases however. 
	*/
	private void changeIndicator (Mode currMode) { 
		if (ind) {
			Destroy (ind);
		}
		ind = GameObject.CreatePrimitive (PrimitiveType.Plane);
		Texture tex;
		Vector3 indScale;
		ind.GetComponent<Renderer> ().material = (Material)Resources.Load ("Materials/General/indMaterial");
		ind.GetComponent<Renderer> ().material.shader = Shader.Find ("Unlit/Transparent Cutout");
		ind.transform.parent = trackedObj.transform;
		//TODO: adjust rotation, position, and scale for suitable controller view
		ind.transform.localPosition = new Vector3 (0f,0.1f,0.1f);
		ind.transform.forward = -trackedObj.transform.forward;
		ind.transform.Rotate (90f,0f,0f);
		//ind.transform.localRotation = Quaternion.Euler(new Vector3(90f,0,0));
		//ind.transform.localEulerAngles = new Vector3(90f,0f,0f);
		ind.layer = 8;

		switch (currMode) {
		case Mode.WebShot:
			tex = (Texture)Resources.Load ("Textures/General/newShot");
			indScale = new Vector3 (0.02f,1.0f,0.02f);
			break;
		case Mode.RetractShot:
			tex = (Texture)Resources.Load ("Textures/General/newRetract");
			indScale = new Vector3 (0.02f,1.0f,0.02f);
			break;
		case Mode.Rope:
			tex = (Texture)Resources.Load ("Textures/General/newRope");
			indScale = new Vector3 (0.02f,1.0f,0.02f);
			break;
		case Mode.Fist:
			tex = (Texture)Resources.Load ("Textures/General/newFist");
			indScale = new Vector3 (0.02f,1.0f,0.02f);
			break;
		case Mode.Climb://TODO: find a more succinct way to differentiate left vs right controller
			if (this.name == "Controller (left)") {
				tex = (Texture)Resources.Load ("Textures/General/newClimbLeft");
				indScale = new Vector3 (0.02f, 1.0f, 0.02f);
			} else {
				tex = (Texture)Resources.Load ("Textures/General/newClimbRight");
				indScale = new Vector3 (0.02f, 1.0f, 0.02f);
			} 
			break;
		default: 
			destroyPrimitive (ind);
			return;
		}
		ind.GetComponent<Renderer> ().material.mainTexture = tex;
		ind.transform.localScale = indScale;
		StartCoroutine (destroyPrimitive(ind));
	}

	private IEnumerator destroyPrimitive(GameObject prim) {
		yield return new WaitForSeconds (2);
		if (prim) {
			Destroy (prim);
		}
	}


	void Update()
	{

		//This block controls Trigger Down input: Grabbing an object.
		if (Controller.GetHairTriggerDown () && climbEnabled) {
			if (currentMode != Mode.Climb) {
				currentMode = Mode.Climb;
				changeIndicator (currentMode);
				isClimbing = this.GetComponent<ControllerGrab> ().Grab ();
			} else {
				isClimbing = this.GetComponent<ControllerGrab> ().Grab ();
			}
		}

		//This block controls Trigger Up input: Releasing an object. 
		if (Controller.GetHairTriggerUp () && climbEnabled) {
			this.GetComponent<ControllerGrab> ().UnGrab ();
			isClimbing = false;
		}

		//This block moves the CameraRig, solely based on the isClimbing value. Executes every frame regardless of mode.
		if (isClimbing) {
			this.GetComponent<ControllerGrab> ().MoveCameraRig ();
		} else {
			//This block begins handling all TouchpadDown inputs. Inputs are initiated when the pad is pressed, and each if block checks
			//	where the user's finger is on the touchpad, checked via the 2d axis of the touchpad.
			if (Controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {

				Vector2 touchPadAxis = Controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_Axis0);

				if (touchPadAxis.y > touchPadAxis.x) {	//up or left

					if (touchPadAxis.y > -touchPadAxis.x && ropeEnabled) {		//Handles TouchpadDown input Up: Rope Spawning

						if (currentMode != Mode.Rope) {
							currentMode = Mode.Rope;
							changeIndicator (currentMode);
						} else
							currentMode = Mode.Rope;
						//This block doesn't have any relevant calls because Rope only needs the currentMode to be Rope in order to function in 
						//preview mode. Preview mode operates in the update method of the Rope script, so as long as it is in Rope mode, it is 
						//running. 
					} else {	//Handles Touchpad input Left: Fist
						if (fistEnabled) {
							if (currentMode != Mode.Fist) {
								currentMode = Mode.Fist;
								changeIndicator(currentMode);
								//handle first input while controller is in different mode
							} else {
								//handle any subsequent inputs beyond first input
							}
						}
					}

				} else {	//down or right

					if (touchPadAxis.y > -touchPadAxis.x && retractEnabled) {		//Handles TouchpadUp input Right: Retract
						if (currentMode != Mode.RetractShot) {
							currentMode = Mode.RetractShot;
							changeIndicator (currentMode);
							//handle first input while controller is in different mode
						} else {
							//handle any subsequent inputs beyond first input
						}

					} else {	//Handles Touchpad input Down: WebShot
						if (shotEnabled) {
							if (currentMode != Mode.WebShot) {
								currentMode = Mode.WebShot;
								changeIndicator(currentMode);
								//handle first input while controller is in different mode
							} else {
								//handle any subsequent inputs beyond first input
							}
						}
					}
				}
			}

			//This block handles all TouchpadUp inputs, checking via the same method as the TouchpadDown block. 
			//Note that each method here
			if (Controller.GetPressUp (SteamVR_Controller.ButtonMask.Touchpad)) {

				touchPadAxis = Controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_Axis0);

				//Handles TouchpadUp input Up: Rope Spawning
				//so the trick is to only take the up input if you're already in the mode for the respective mode
				//if you aren't in the respective mode and you get called as a touchpadUp input, just go back to default climbing mode!
				//Note that we don't need to tell this method to exit Rope mode because you automatically switch when using another function. 
				if (touchPadAxis.y > touchPadAxis.x) {	//left or up

					if (touchPadAxis.y > -touchPadAxis.x && ropeEnabled) {		//Handles TouchpadUp input Up: Rope Spawning
						if (currentMode == Mode.Rope) {

						} else {
							currentMode = Mode.Climb;
						}
					} else {	//Handles TouchpadUp input Left: Fist
						if (fistEnabled) {
							if (currentMode != Mode.Fist) {
								currentMode = Mode.Climb;
								//handle first Up input while controller is in different mode
							} else {
								//handle any subsequent getpressup inputs beyond first input
							}
						}
					}
				} else {	//down or right

					if (touchPadAxis.y > -touchPadAxis.x && retractEnabled) {	//Handles TouchpadUp input Right: Retract
						if (currentMode != Mode.RetractShot) {
							currentMode = Mode.Climb;
							//handle first Up input while controller is in different mode
						} else if (currentMode == Mode.RetractShot) {
							//handle any subsequent inputs beyond first input

						}
					} else {	//Handles TouchpadUp input Down: WebShot
						if (shotEnabled)
						{
							if (currentMode != Mode.WebShot)
							{
								currentMode = Mode.Climb;
								//handle first Up input while controller is in different mode
							}
							else
							{
								//handle any subsequent inputs beyond first input
							}
						}
					}

				}
			}
		}
	}

}