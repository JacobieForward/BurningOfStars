﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
[CreateAssetMenu(menuName = ("CustomObjects/Weapon"))]
public class Weapon : ScriptableObject {
    [SerializeField] GameObject sprite;
    [SerializeField] GameObject projectile;
    [SerializeField] float fireRate; // The lower this number the faster the weapon will fire. I.E. If fireRate == 1 then the weapon will fire 1 projectile every 1 second
    [SerializeField] float spread;
    [SerializeField] int numberSpawned;
    [SerializeField] int maxAmmunition;

    [SerializeField] GameObject weaponPickupPrefab;

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

    public Sprite GetSideViewSprite() {
        return weaponPickupPrefab.gameObject.GetComponent<SpriteRenderer>().sprite;
    }
}
