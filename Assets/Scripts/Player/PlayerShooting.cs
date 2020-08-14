using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    Shooting shootingScript;
    PlayerMovement playerMovement;

    void Awake() {
        shootingScript = GetComponent<Shooting>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    void Update() {
        if (Input.GetMouseButton(0)) {
            if (shootingScript.AbleToShoot() && !playerMovement.isSprinting) {
                shootingScript.ShootPrimaryWeapon();
            }
        }
    }
}
