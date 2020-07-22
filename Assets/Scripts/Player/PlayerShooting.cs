using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    Shooting shootingScript;

    void Awake() {
        shootingScript = GetComponent<Shooting>();
    }
    void Update() {
        if (Input.GetMouseButton(0)) {
            if (shootingScript.AbleToShoot()) {
                shootingScript.ShootPrimaryWeapon();
            }
        }
    }
}
