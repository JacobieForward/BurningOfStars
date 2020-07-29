using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryWaypoint : MonoBehaviour {
    float whenInstantiated;

    private void Awake() {
        whenInstantiated = Time.timeSinceLevelLoad;
    }

    void Update() {
        float timeSinceInstantiated = Time.timeSinceLevelLoad - whenInstantiated;
        if (timeSinceInstantiated > 10.0f) {
            Destroy(gameObject);
        }
    }
}
