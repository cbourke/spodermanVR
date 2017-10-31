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
	public Mode currentMode = Mode.Climb;	//TODO: Check if this is proper Enum usage.
    private Vector2 touchPadAxis;


    private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input ((int)trackedObj.index); }
	}

	void Awake() {
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
		GameObject ind = GameObject.CreatePrimitive (PrimitiveType.Plane);
		Texture tex;
		ind.GetComponent<Renderer> ().material.shader = Shader.Find ("Unlit/Transparent Cutout");
		ind.transform.parent = trackedObj.transform;
		//TODO: adjust rotation, position, and scale for suitable controller view
		ind.transform.localPosition = new Vector3 (0f,0.1f,0.1f);
		ind.transform.forward = -trackedObj.transform.forward;
		ind.transform.Rotate (90f,0f,0f);

		switch (currMode) {
		case Mode.WebShot:
			tex = (Texture)Resources.Load ("webShot");
			break;
		case Mode.RetractShot:
			tex = (Texture)Resources.Load ("retractShot");
			break;
		case Mode.Rope:
			tex = (Texture)Resources.Load ("rope");
			break;
		case Mode.Fist:
			tex = (Texture)Resources.Load ("fist");
			break;
		default:
			tex = (Texture)Resources.Load ("climb");
			break;
		}
		ind.GetComponent<Renderer> ().material.mainTexture = tex;
		ind.transform.localScale = new Vector3 (0.2f,1.0f,0.2f);
		StartCoroutine (destroyPrimitive(ind));
	}

	private IEnumerator destroyPrimitive(GameObject prim) {
		yield return new WaitForSeconds (2);
		Destroy (prim);
	}


    void Update()
    {

		//This block controls Trigger Down input: Grabbing an object.
		if (Controller.GetHairTriggerDown ()) {
			currentMode = Mode.Climb;
			isClimbing = this.GetComponent<ControllerGrab> ().Grab ();
		}

		//This block controls Trigger Up input: Releasing an object. 
		if (Controller.GetHairTriggerUp ()) {
			this.GetComponent<ControllerGrab> ().UnGrab ();
            isClimbing = false;
		}

		//This block begins handling all TouchpadDown inputs. Inputs are initiated when the pad is pressed, and each if block checks
		//	where the user's finger is on the touchpad, checked via the 2d axis of the touchpad.
		//The axis boundary values of 0.7f ensure that only one input can be taken at a time. Theoretically...
		if (Controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {

			Vector2 touchPadAxis = Controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_Axis0);

			//Handles TouchpadDown input Up: Rope Spawning
			if (touchPadAxis.y > 0.7f) {
				currentMode = Mode.Rope;
                Debug.Log(trackedObj.name + " TouchpadDown Up");

				//This block doesn't have any relevant calls because Rope only needs the currentMode to be Rope in order to function in 
				//preview mode. Preview mode operates in the update method of the Rope script, so as long as it is in Rope mode, it is 
				//running. 

            }

			//Handles Touchpad input Down: WebShot
			else if (touchPadAxis.y < -0.7f) {
				if (currentMode != Mode.WebShot) {
					currentMode = Mode.WebShot;
					//handle first input while controller is in different mode
				}
				else if (currentMode == Mode.WebShot) {
					//handle any subsequent inputs beyond first input
				}
                Debug.Log(trackedObj.name + " TouchpadDown Down");
            }

			//Handles Touchpad input Right: Retract
			if (touchPadAxis.x > 0.7f) {
				if (currentMode != Mode.RetractShot) {
					currentMode = Mode.RetractShot;
					//handle first input while controller is in different mode
				}
				else if (currentMode == Mode.RetractShot) {
					//handle any subsequent inputs beyond first input
				}
                Debug.Log(trackedObj.name + " TouchpadDown Right ");
            }

			//Handles Touchpad input Left: Fist
			else if (touchPadAxis.x < -0.7f) {
				if (currentMode != Mode.Fist) {
					currentMode = Mode.Fist;
					//handle first input while controller is in different mode
				}
				else if (currentMode == Mode.Fist) {
					//handle any subsequent inputs beyond first input
				}
                Debug.Log(trackedObj.name + " TouchpadDown Left ");
            }

		}

        //This block handles all TouchpadUp inputs, checking via the same method as the TouchpadDown block. 
        //Note that each method here
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {

            touchPadAxis = Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

            //Handles TouchpadUp input Up: Rope Spawning
			//so the trick is to only take the up input if you're already in the mode for the respective mode
			//if you aren't in the respective mode and you get called as a touchpadUp input, just go back to default climbing mode!
			//Note that we don't need to tell this method to exit Rope mode because you automatically switch when using another function. 
			if (touchPadAxis.y > 0.7f) {
				if (currentMode == Mode.Rope) {
					if (this.GetComponent<Rope> ().isValidNode ()) {
						Vector3 validSpot = this.GetComponent<Rope> ().getValidNodePosition ();
						this.GetComponent<Rope> ().spawnNodeBridge (validSpot); //this method call makes it so we don't have to reference WorldNodeTracker in this script
					}
				} else {
					currentMode = Mode.Climb;
				}
			
                Debug.Log(trackedObj.name + " TouchpadUp Up ");


            }

            //Handles TouchpadUp input Down: WebShot
            else if (touchPadAxis.y < -0.7f)
            {
                if (currentMode != Mode.WebShot)
                {
                    currentMode = Mode.WebShot;
                    //handle first Up input while controller is in different mode
                }
                else if (currentMode == Mode.WebShot)
                {
                    //handle any subsequent inputs beyond first input
                }
                Debug.Log(trackedObj.name + " TouchpadUp Down ");
            }

            //Handles TouchpadUp input Right: Retract
            if (touchPadAxis.x > 0.7f)
            {
                if (currentMode != Mode.RetractShot)
                {
                    currentMode = Mode.RetractShot;
                    //handle first Up input while controller is in different mode
                }
                else if (currentMode == Mode.RetractShot)
                {
                    //handle any subsequent inputs beyond first input
                }
                Debug.Log(trackedObj.name + " TouchpadUp Right ");
            }

            //Handles TouchpadUp input Left: Fist
            else if (touchPadAxis.x < -0.7f)
            {
                if (currentMode != Mode.Fist)
                {
                    currentMode = Mode.Fist;
                    //handle first Up input while controller is in different mode
                }
                else if (currentMode == Mode.Fist)
                {
                    //handle any subsequent inputs beyond first input
                }
                Debug.Log(trackedObj.name + " TouchpadUp Left ");
            }

        }


        //This block moves the CameraRig, solely based on the isClimbing value. Executes every frame regardless of mode.
        if (isClimbing) {
			this.GetComponent<ControllerGrab> ().MoveCameraRig ();
		}
	



    }

}
