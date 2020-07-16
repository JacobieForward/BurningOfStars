using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class PlayerStats : MonoBehaviour {
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    void Awake() {
        currentHealth = maxHealth;
    }

    void Update() {
        if (currentHealth <= 0.0f) {
            Die();
        }
    }

    void Die() {
        print("You lose.");
    }
}
