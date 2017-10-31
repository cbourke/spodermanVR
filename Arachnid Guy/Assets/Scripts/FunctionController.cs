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
				if (currentMode != Mode.Rope) {
					currentMode = Mode.Rope;
					//handle first input while controller is in different mode
				}
				else if (currentMode == Mode.Rope) {
					//handle any subsequent inputs beyond first input
				}
                Debug.Log(trackedObj.name + " TouchpadDown Up");
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
            if (touchPadAxis.y > 0.7f)
            {
                if (currentMode != Mode.Rope)
                {
                    currentMode = Mode.Rope;
                    //handle first input while controller is in different mode
                }
                else if (currentMode == Mode.Rope)
                {
                    //handle any subsequent inputs beyond first input
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
