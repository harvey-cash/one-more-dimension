using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : GrabbableObject {

    public override void grabbed(ViveController controller) {
        base.grabbed(controller);
        transform.rotation = controller.transform.rotation;
        transform.position = controller.transform.position;
        controller.setVisible(false);
    }

    public override void released(ViveController controller) {
        base.released(controller);
        controller.setVisible(true);
    }

    public override void activate(ViveController controller) {
        StartCoroutine(shoot(controller));
        base.activate(controller);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

    public Transform aimVector;
    private int bullets = 100;
    private const float SHOT_INTERVAL = 0.1f, SHOT_VELOCITY = 10f;
    
    private IEnumerator shoot(ViveController controller) {
        for(int i = 0; i < 5; i++) {
            bullets--;

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            sphere.transform.position = aimVector.position + (0.05f * aimVector.forward);
            sphere.GetComponent<Renderer>().material.color = Color.white;
            Rigidbody sphereBody = sphere.AddComponent<Rigidbody>();
            sphereBody.useGravity = false;
            sphereBody.velocity = SHOT_VELOCITY * aimVector.forward;
            SteamVR_Controller.Input(controller.getTrackedIndex()).TriggerHapticPulse(1000);

            yield return new WaitForSeconds(SHOT_INTERVAL);
        }        
    }

    public void setBullets(int count) {
        bullets = count;
    }

}
