using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
[RequireComponent(typeof(Equipment))]
public class Stats : MonoBehaviour {
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;

    Equipment equipment;
    void Awake() {
        equipment = GetComponent<Equipment>();
        currentHealth = maxHealth;
    }

    // Using LateUpdate as a lazy answer to the player death causing the health bar to have a value above 0 because they die before the bar is updated
    void LateUpdate() {
        if (currentHealth <= 0.0f) {
            Instantiate(equipment.GetCurrentWeapon().GetWeaponPickupPrefab(), gameObject.transform.position, gameObject.transform.rotation);
            Die();
        }
    }

    public void ProjectileImpact(Projectile projectileHit) {
        currentHealth -= projectileHit.GetDamage();
    }

    void Die() {
        Destroy(gameObject);
    }

    public float GetMaxHealth() {
        return maxHealth;
    }

    public float GetCurrentHealth() {
        return currentHealth;
    }
}
