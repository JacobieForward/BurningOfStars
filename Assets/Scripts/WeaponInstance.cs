using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class WeaponInstance {
    GameObject sprite;
    GameObject projectile;
    float fireRate; // The lower this number the faster the weapon will fire. I.E. If fireRate == 1 then the weapon will fire 1 projectile every 1 second
    float spread;
    int numberSpawned;
    int maxAmmunition;
    int currentAmmunition;

    bool isInitialized = false;

    GameObject weaponPickupPrefab;

    public void InitWeaponInstance(Weapon weaponForInstance) {
        sprite = weaponForInstance.GetSprite();
        projectile = weaponForInstance.GetProjectile();
        fireRate = weaponForInstance.GetFireRate();
        spread = weaponForInstance.GetSpread();
        numberSpawned = weaponForInstance.GetNumberSpawned();
        maxAmmunition = weaponForInstance.GetMaxAmmunition();
        currentAmmunition = maxAmmunition;
        isInitialized = true;
    }

    public GameObject GetSprite() {
        return sprite;
    }

    public GameObject GetProjectile() {
        return projectile;
    }

    public float GetFireRate() {
        return fireRate;
    }

    public float GetSpread() {
        return spread;
    }

    public GameObject GetWeaponPickupPrefab() {
        return weaponPickupPrefab;
    }

    public int GetNumberSpawned() {
        return numberSpawned;
    }

    public int GetMaxAmmunition() {
        return maxAmmunition;
    }

    public void UseAmmunition() {
        currentAmmunition -= 1;
    }

    public bool IsInitialized() {
        return isInitialized;
    }
}
