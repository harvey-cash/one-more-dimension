using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCube : GrabbableObject {

    private bool coloured = false;
	void OnCollisionEnter(Collision collision) {
        if (!coloured) {
            gameObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            coloured = true;
        }
    }
}
