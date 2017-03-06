using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedCube : MonoBehaviour {

    private const float PASTEL = 0.3f;
    private bool coloured = false;
    private float MAX_HEALTH = 5, health = 5;
    private Color myColor;


    void OnCollisionEnter(Collision collision) {
        if (!coloured) {
            myColor = new Color(Random.Range(PASTEL, 1f), Random.Range(PASTEL, 1f), Random.Range(PASTEL, 1f));
            gameObject.GetComponent<Renderer>().material.color = myColor;

            coloured = true;
        }

        if (collision.gameObject.GetComponent<Bullet>() != null && collision.gameObject.GetComponent<Bullet>().getHit()) {
            collision.gameObject.GetComponent<Bullet>().setHit(false);
            health--;
            if(health < 0) { Destroy(gameObject); }
            else { gameObject.GetComponent<Renderer>().material.color = Color32.Lerp(Color.black, myColor, health / MAX_HEALTH); }
        }
    }
}
