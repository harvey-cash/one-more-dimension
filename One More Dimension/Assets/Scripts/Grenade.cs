using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : GrabbableObject {

    private const float EXPLODE_DELAY = 1, FRAG_VELOCITY = 20;
    private const int FRAGMENTS = 50;

    public override void released(ViveController controller) {
        base.released(controller);
        StartCoroutine(explode());
    }

    private IEnumerator explode() {
        yield return new WaitForSeconds(EXPLODE_DELAY);
        gameObject.GetComponent<Collider>().enabled = false;

        for (int i = 0; i < FRAGMENTS; i++) {
            Vector3 randVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            randVector = randVector / randVector.magnitude;

            GameObject bullet = Instantiate(Resources.Load("Prefabs/Bullet", typeof(GameObject))) as GameObject;
            bullet.transform.position = gameObject.transform.position + (0.05f * randVector);
            bullet.GetComponent<Rigidbody>().velocity = FRAG_VELOCITY * randVector;
            StartCoroutine(bullet.GetComponent<Bullet>().selfDestruct());
        }

        Destroy(gameObject);
    }
}
