﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class Stats : MonoBehaviour {
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    [SerializeField] GameObject deathParticles;
    [SerializeField] GameObject deathSprite;

    [SerializeField] List<GameObject> bodyPieces; 

    Equipment equipment;
    void Awake() {
        equipment = GetComponent<Equipment>();
        currentHealth = maxHealth;
    }

    // Using LateUpdate as a lazy answer to the player death causing the health bar to have a value above 0 because they die before the bar is updated
    void LateUpdate() {
        if (currentHealth <= 0.0f) {
            Die();
        }
    }

    public void ProjectileImpact(Projectile projectileHit) {
        currentHealth -= projectileHit.GetDamage();
    }

    void Die() {
        if (equipment != null) {
            GameObject droppedWeapon = Instantiate(equipment.GetCurrentWeapon().GetWeaponPickupPrefab(), gameObject.transform.position, gameObject.transform.rotation);
            droppedWeapon.GetComponent<InteractableObject>().SetAmmunition(GetComponent<Equipment>().GetCurrentWeaponAmmunition());
        }
        if (deathParticles != null) {
            Instantiate(deathParticles, gameObject.transform.position, gameObject.transform.rotation);
        }
        if (deathSprite != null) {
            Instantiate(deathSprite, gameObject.transform.position, gameObject.transform.rotation);
        }
        SpawnBodyPieces();
        Destroy(gameObject);
    }

    public float GetMaxHealth() {
        return maxHealth;
    }

    public float GetCurrentHealth() {
        return currentHealth;
    }

    void SpawnBodyPieces() {
        int numberOfBodyPieces = Random.Range(1, bodyPieces.Count);

        if (bodyPieces.Count != 0) {
            List<GameObject> tempBodyPieces = bodyPieces;
            GameObject piece = tempBodyPieces[0];
            for (int i = 0; i < numberOfBodyPieces; i++) {
                if (tempBodyPieces.Count == 0) {
                    piece = bodyPieces[0];
                } else {
                    piece = tempBodyPieces[Random.Range(0, tempBodyPieces.Count)];
                }
                if (piece != null) {
                    Instantiate(piece, transform.position, transform.rotation);
                }
                tempBodyPieces.Remove(piece);
            }
        } else {
            return;
        }
    }
}
