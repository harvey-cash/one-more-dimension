using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour {
    protected bool debug = true;
    private const int VEL_ARRAY_SIZE = 3;

	public virtual void grabbed (ViveController controller) {
        //if(debug) { Debug.Log(gameObject.name + " has been grabbed."); }
        gameObject.transform.parent = controller.transform;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        //deal with passing from hand to hand
        if (grabbedBy != null && grabbedBy.gameObject != controller.gameObject && grabbedBy.GetComponent<ViveController>() != null) {
            grabbedBy.GetComponent<ViveController>().forgetGrabbed();
        }
        grabbedBy = controller.transform;
    }
    public virtual void released (ViveController controller) {
        //if (debug) { Debug.Log(gameObject.name + " has been released."); }
        gameObject.transform.parent = null;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        grabbedBy = null;

        gameObject.GetComponent<Rigidbody>().velocity = getReleaseVelocity();
    }

    //for throwing, it's better to base velocity on the controller - rather than
    //the object itself.
    protected Transform grabbedBy = null;
    private Vector3[] positionArray = new Vector3[VEL_ARRAY_SIZE + 1];
    private Vector3[] velocityArray = new Vector3[VEL_ARRAY_SIZE];
    //start off with zero vectors, avoid null errors
    private void initialiseArrays() {
        for (int i = 0; i < positionArray.Length; i++) { positionArray[i] = Vector3.zero; }
        for (int i = 0; i < velocityArray.Length; i++) { velocityArray[i] = Vector3.zero; }
    }
    //take last few frames and determine release velocity.
    private void measureReleaseVelocity() {
        //which transform do we pay attention to?
        if (grabbedBy != null) {
            //shift all vectors in the array up, add current vector to the end
            for (int i = 0; i < positionArray.Length - 1; i++) { positionArray[i] = positionArray[i + 1]; }
            positionArray[positionArray.Length - 1] = grabbedBy.position;
            for (int i = 0; i < velocityArray.Length - 1; i++) {
                velocityArray[i] = velocityArray[i + 1];
            }
            Vector3 deltaPos = positionArray[positionArray.Length - 1] - positionArray[positionArray.Length - 2];
            velocityArray[velocityArray.Length - 1] = deltaPos / Time.deltaTime;
        }
    }

    private Vector3 getReleaseVelocity() {
        //averaging method
        float averageX = 0f, averageY = 0f, averageZ = 0f;
        for(int i = 0; i < velocityArray.Length; i++) {
            averageX += velocityArray[i].x;
            averageY += velocityArray[i].y;
            averageZ += velocityArray[i].z;
        }
        averageX = averageX / velocityArray.Length;
        averageY = averageY / velocityArray.Length;
        averageZ = averageZ / velocityArray.Length;

        Vector3 averageVelocity = new Vector3(averageX, averageY, averageZ);
        return averageVelocity;
    }

    public virtual void activate () {
    }

    //~~~~~~~~~~~~~~~~~~~~ METHOD CALLS ~~~~~~~~~~~~~~~~~~~~//

    void Start() {
        initialiseArrays();
    }

    void Update() {
        measureReleaseVelocity();
    }

}