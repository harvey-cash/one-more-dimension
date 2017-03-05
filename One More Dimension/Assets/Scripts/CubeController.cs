using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {

    private Vector3 SPAWN_POS = new Vector3(0, 1.5f, 20);
    private const float SPAWN_DIST = 0.6f;
    private float cubeVel = 1;

    void Start() {
        StartCoroutine(spawner());
    }

    private IEnumerator spawner() {
        spawnCubes();
        yield return new WaitForSeconds((SPAWN_DIST * 10 / 2) / cubeVel);
        StartCoroutine(spawner());
    }

	private void spawnCubes() {
        for (int i = -2; i < 2; i++) {
            for (int j = -2; j < 2; j++) {
                if(Random.Range(0f, 1f) > 0.5f) {
                    GameObject cube = Instantiate(Resources.Load("Prefabs/Cube", typeof(GameObject))) as GameObject;
                    cube.transform.position = SPAWN_POS + (Vector3.right * i) + (Vector3.up * j);
                    cube.GetComponent<Rigidbody>().velocity = Vector3.back * cubeVel;
                }
            }
        }
    }
}
