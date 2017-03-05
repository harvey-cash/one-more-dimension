using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private const float DESTRUCT_DELAY = 10;

	public IEnumerator selfDestruct() {
        yield return new WaitForSeconds(DESTRUCT_DELAY);
        Destroy(gameObject);
    }
}
