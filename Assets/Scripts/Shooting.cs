using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class Shooting : MonoBehaviour {
    Equipment equipment;

    float fireTimer = 0.0f;

    void Awake() {
        equipment = GetComponent<Equipment>();
    }

    void Update() {
        fireTimer += Time.deltaTime;
    }

    public bool AbleToShoot() {
        return equipment.GetCurrentWeapon() != null && fireTimer >= equipment.GetCurrentWeapon().GetFireRate();
    }

    public void ShootPrimaryWeapon() {
        for (int i = 0; i < equipment.GetCurrentWeapon().GetNumberSpawned(); i++) {
            GameObject projectileInstance = Instantiate(equipment.GetCurrentWeapon().GetProjectile(), gameObject.transform.GetChild(0).GetChild(0).GetChild(0).transform.position, gameObject.transform.rotation);
            // 90 added to z rotation so that the projectile goes along the y axis
            projectileInstance.transform.Rotate(0.0f, 0.0f, 90.0f + Random.Range(-equipment.GetCurrentWeapon().GetSpread(), equipment.GetCurrentWeapon().GetSpread()));
            Physics2D.IgnoreCollision(projectileInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        fireTimer = 0.0f;
    }
}
