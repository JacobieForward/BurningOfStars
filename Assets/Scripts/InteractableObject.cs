﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
[RequireComponent(typeof(Collider2D))]
public class InteractableObject : MonoBehaviour {
    // TODO: During polish create different script types that Inherit InteractableObject or use it as an interface
    // For now lazy enum method
    [SerializeField] InteractableType type;
    [SerializeField] Weapon weapon;
    [SerializeField] int ammunition;
    [SerializeField] float healthGained;

    enum InteractableType {
        WeaponPickup,
        HealthPickup,
        Door
    }

    void Awake() {
        if (weapon != null) {
            ammunition = weapon.GetMaxAmmunition();
        }
    }

    public void Interact() {
        if (type == InteractableType.WeaponPickup) {
            if (weapon == null) {
                print("No weapon scriptable object on weapon pickup InteractableObject component.");
                return;
            }
            Equipment playerEquipmentScript = GameObject.FindWithTag("Player").GetComponent<Equipment>();
            Weapon playerWeaponBeforePickup = playerEquipmentScript.GetCurrentWeapon();
            Weapon playerOffHandWeaponBeforePickup = playerEquipmentScript.GetOffHandWeapon();
            if (playerEquipmentScript.ShouldPickupSwapWithCurrentWeapon(playerWeaponBeforePickup, playerOffHandWeaponBeforePickup)) {
                SwapWeaponPickupWithPlayerWeapon(playerWeaponBeforePickup);
            } else {
                Destroy(gameObject);
            }
            playerEquipmentScript.EquipWeapon(weapon);
            playerEquipmentScript.SetCurrentWeaponAmmunition(ammunition);
        }
        if (type == InteractableType.HealthPickup) {
            GameObject.FindWithTag("Player").GetComponent<Stats>().GainHealth(healthGained);
            Destroy(gameObject);
        } else if(type == InteractableType.Door) {

        }
    }

    void SwapWeaponPickupWithPlayerWeapon(Weapon weaponToSwapWith) {
        Equipment playerEquipmentScript = GameObject.FindWithTag("Player").GetComponent<Equipment>();
        GameObject newPickup = Instantiate(weaponToSwapWith.GetWeaponPickupPrefab(), gameObject.transform.position, gameObject.transform.rotation);
        newPickup.GetComponent<InteractableObject>().SetAmmunition(playerEquipmentScript.GetCurrentWeaponAmmunition());
        newPickup.GetComponent<Rigidbody2D>().velocity = Random.onUnitSphere * Random.Range(1, 2);
        Destroy(gameObject);
    }

    public void SetAmmunition(int ammunitionAmount) {
        ammunition = ammunitionAmount;
    }
}
