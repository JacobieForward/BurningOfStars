using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Use this in Projectile (and I think somewhere else) instead of implementing it seperately in those scripts
public class DelayedDestroy : MonoBehaviour {
    [SerializeField] float timeToDestroy;
    float destroyTimer;

    void Update() {
        destroyTimer += Time.deltaTime;
        if (destroyTimer >= timeToDestroy) {
            Destroy(gameObject);
        }
    }
}
