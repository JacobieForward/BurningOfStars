using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class Projectile : MonoBehaviour {
    [SerializeField] float speed;
    [SerializeField] float damage;

    float whenInstantiated;

    private void Awake() {
        whenInstantiated = Time.timeSinceLevelLoad;
    }

    void Update() {
        transform.position += transform.right * Time.deltaTime * speed;
        float timeSinceInstantiated = Time.timeSinceLevelLoad - whenInstantiated;
        if (timeSinceInstantiated > 1.5f) {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Doing a GetComponent every collision might be resource intensive
        EnemyStats enemyScript = other.GetComponent<EnemyStats>();
        if (enemyScript != null) {
            enemyScript.ProjectileImpact(this);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }

    public float GetDamage() {
        return damage;
    }
}
