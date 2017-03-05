using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveController : MonoBehaviour {

    //~~~~~~~~~~~~~~~~~~~~ KEYBINDINGS ~~~~~~~~~~~~~~~~~~~~//

    private enum Action { GRAB, ACTIVATE, MENU }
    private enum Input { CLICK, LIFT, HOLD }
    private bool getInput (Action action, Input press) {
        switch (press) {
            case Input.CLICK:
                return controller.GetPressDown(getViveKey(action));
            case Input.LIFT:
                return controller.GetPressUp(getViveKey(action));
            case Input.HOLD:
                return controller.GetPress(getViveKey(action));
                
            default:
                return false;
        }
    }
    //returns the requested SteamVR ButtonMask.
    //EDIT KEYBINDINGS HERE
    private ulong getViveKey(Action action) {
        switch (action) {
            case Action.GRAB:
                return SteamVR_Controller.ButtonMask.Trigger;
            case Action.ACTIVATE:
                return SteamVR_Controller.ButtonMask.Touchpad;
            case Action.MENU:
                return SteamVR_Controller.ButtonMask.ApplicationMenu;

            //THIS SHOULD NEVER BE RETURNED.
            default:
                return SteamVR_Controller.ButtonMask.Trigger;
        }
    }

    //~~~~~~~~~~~~~~~~~~~~ INITIALISATION ~~~~~~~~~~~~~~~~~~~~//

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private void controllerInit() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        myModel = gameObject.transform.GetChild(0);

        //give ourselves some trigger colliders
        SphereCollider triggerCollider = gameObject.AddComponent<SphereCollider>();
        triggerCollider.isTrigger = true;
        triggerCollider.radius = 0.08f;
        triggerCollider.center = new Vector3(0f, -0.05f, 0.03f);        
    }    

    //~~~~~~~~~~~~~~~~~~~~ GRABBING ~~~~~~~~~~~~~~~~~~~~//

    private const float GRAB_TOGGLE_TIME = 0.3f;
    private bool releaseOnNextLift = true;

    private GrabbableObject couldGrab, haveGrabbed;
    void OnTriggerEnter (Collider collider) {
        if (collider.gameObject.GetComponent<GrabbableObject>() != null) {
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(500);
            couldGrab = collider.gameObject.GetComponent<GrabbableObject>();
        }
    }
    void OnTriggerExit (Collider collider) {
        if (couldGrab != null && collider.gameObject == couldGrab.gameObject) {
            couldGrab = null;
        }
    }	
    //called by an object when it is grabbed by something new
    public void forgetGrabbed() {
        haveGrabbed = null;
        releaseOnNextLift = true;        
    }

    private void checkButtonInputs () {
        //if already grabbing something in toggle grab mode, release on the following lift
        if (getInput(Action.GRAB, Input.CLICK) && haveGrabbed != null && !releaseOnNextLift) {
            releaseOnNextLift = true;
        }
        //grab (only if not holding something already)
        if (getInput(Action.GRAB, Input.CLICK) && couldGrab != null && haveGrabbed == null) {
            StartCoroutine(waitForGrabToggle());
            haveGrabbed = couldGrab;
            haveGrabbed.grabbed(this);
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(1000);
        }
        //release, if the grab mode allows
        if (getInput(Action.GRAB, Input.LIFT) && haveGrabbed != null && releaseOnNextLift) {
            haveGrabbed.released(this);
            haveGrabbed = null;
        }

        //activate
        if (getInput(Action.ACTIVATE, Input.CLICK) && haveGrabbed != null) {
            haveGrabbed.activate();
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(3000);
        }
    }

    //if I grab and hold, assume that I want to throw!
    private IEnumerator waitForGrabToggle() {
        releaseOnNextLift = false;
        yield return new WaitForSeconds(GRAB_TOGGLE_TIME);
        if (haveGrabbed != null) { releaseOnNextLift = true; }
    }

    //~~~~~~~~~~~~~~~~~~~~ TOOL SPECIFIC METHODS ~~~~~~~~~~~~~~~~~~~~//

    //hides the controller and its collider
    private Transform myModel;
    public void setVisible(bool visibility) {
        myModel.gameObject.SetActive(visibility);
        //gameObject.GetComponent<SphereCollider>().enabled = visibility;
    }


    //~~~~~~~~~~~~~~~~~~~~ METHOD CALLS ~~~~~~~~~~~~~~~~~~~~//

    void Start() {
        controllerInit();
    }

    void Update () {
        checkButtonInputs();
	}


}
