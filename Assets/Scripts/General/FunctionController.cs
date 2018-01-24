/*
 * This script is to be used as the primary form of changing functionalities for each controller. 
 * Each function that is referenced here will be, for the time being, a reference to another script written seperately of this one. 
 * Eventually, once we have semi-successfully implemented each of the functions, we may possibly merge all the functionality into this one script. 
 * 
 * The Enumerable class, Mode, may be used by each function to ensure edge cases for switching functionalities can be handled appropriately. 
 * 
 * For R2, we will be dividing responsibilities into 4 different scripts, one for each of the functionalities besides climbing. 
 * Each person can implement their script using the control if blocks down below, in the Update method. 
 * Follow the usage of the ControllerGrab scripts as example for how to implement your assigned script. Note that integration of the 
 * 	ControllerGrab script into this FunctionController script was done in, like, one night, so errors may be abound....
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionController : MonoBehaviour {

	public enum Mode {
		Climb,
		Rope,
		WebShot,
		RetractShot,
		Fist,
	};


    public SteamVR_TrackedObject trackedObj;
	public bool isClimbing;
	public Mode currentMode = Mode.Climb;
    public bool climbEnabled;
    public bool ropeEnabled;
    public bool retractEnabled;
    public bool fistEnabled;
    public bool shotEnabled;
	private GameObject ind;
    private Vector2 touchPadAxis;
	private AudioSource controllerSpeaker;
	private AudioClip modeChangePing;
	private AudioClip webShot;



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
		controllerSpeaker = GetComponent<AudioSource> ();
		modeChangePing = (AudioClip)Resources.Load ("Audio/General/changeHandMode");
		webShot = (AudioClip)Resources.Load ("Audio/General/webShot");


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
		//ind.transform.localRotation = Quaternion.identity;
		ind.transform.Rotate (90f,0f,0f);
		//ind.transform.Rotate (90f,trackedObj.transform.rotation.eulerAngles.y - 180f,0f);
		//ind.transform.localRotation = Quaternion.EulerAngles(new Vector3(90f,0,0));
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
		default:	//TODO: find a more succinct way to differentiate left vs right controller
			if (this.name == "Controller (left)") {
				tex = (Texture)Resources.Load ("Textures/General/newLeftClimb");
				indScale = new Vector3 (0.02f, 1.0f, 0.02f);
			} else {
				tex = (Texture)Resources.Load ("Textures/General/newRightClimb");
				indScale = new Vector3 (0.02f, 1.0f, 0.02f);
			} 
			break;
		}
		ind.GetComponent<Renderer> ().material.mainTexture = tex;
		ind.transform.localScale = indScale;
		controllerSpeaker.clip = modeChangePing;
		controllerSpeaker.Play ();
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
                                this.GetComponent<WebShot>().Shoot();
								controllerSpeaker.clip = webShot;
								controllerSpeaker.Play ();
                                //handle first input while controller is in different mode
                            } else {
                                //handle any subsequent inputs beyond first input
                                this.GetComponent<WebShot>().Shoot();
								controllerSpeaker.clip = webShot;
								controllerSpeaker.Play ();
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
							if (this.GetComponent<Rope> ().isValidNode ()) {
								Vector3 validSpot = this.GetComponent<Rope> ().getValidNodePosition ();
								this.GetComponent<Rope> ().spawnNodeBridge (validSpot , this.gameObject); //this method call makes it so we don't have to reference WorldNodeTracker in this script
							}
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
							if (!this.GetComponent<ControllerGrab> ().objectInHand) {
                                //this.GetComponent<ControllerGrab> ().GrabPhysicsObjectWithParam (this.GetComponent<ControllerRetract> ().retract ());
                                this.GetComponent<ControllerRetract>().retractNew();

                            }

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