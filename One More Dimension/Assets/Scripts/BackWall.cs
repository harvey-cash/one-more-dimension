using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackWall : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.GetComponent<Bullet>() != null) {
            collider.gameObject.GetComponent<Bullet>().setDestroyed(true);
        }
        Destroy(collider.gameObject);
    }
}
