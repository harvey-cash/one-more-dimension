using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private const float DESTRUCT_DELAY = 10;
    private bool canDamage = true, destroyed = false;

	public IEnumerator selfDestruct() {
        yield return new WaitForSeconds(DESTRUCT_DELAY);
        if (!destroyed) { destroyed = true; Destroy(gameObject); }
    }

    public bool getHit() {
        return canDamage;
    }
    public void setHit(bool damage) {
        canDamage = damage;
    }
    public void setDestroyed(bool destroy) {
        destroyed = true;
    }
}
