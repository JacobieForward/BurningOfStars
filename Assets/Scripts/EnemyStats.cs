using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {
    [SerializeField] float health;

    void Update() {
        if (health <= 0) {
            Die();
        }
    }
    
    public void ProjectileImpact(Projectile projectileHit) {
        health -= projectileHit.GetDamage();
    }

    void Die() {
        Destroy(gameObject);
    }
}
