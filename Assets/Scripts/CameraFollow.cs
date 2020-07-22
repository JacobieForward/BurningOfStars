using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;

    Vector3 cameraOffset = new Vector3(0, 0, -2);

    void LateUpdate() {
        try {
            this.transform.position = target.position + cameraOffset;
        } catch (MissingReferenceException e) {
            print("Player object is destroyed." + e.ToString());
        }
    }
}
