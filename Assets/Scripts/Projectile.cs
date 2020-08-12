using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class Projectile : MonoBehaviour {
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] float projectileLife;

    float whenInstantiated;

    private void Awake() {
        whenInstantiated = Time.timeSinceLevelLoad;
    }

    void Update() {
        transform.position += transform.right * Time.deltaTime * speed;
        float timeSinceInstantiated = Time.timeSinceLevelLoad - whenInstantiated;
        if (projectileLife > 0 && timeSinceInstantiated > projectileLife) {
            Destroy(gameObject);
        }
    }

    //void OnBecameInvisible() {
    //    Destroy(gameObject);
    //}
    
    private void OnCollisionEnter2D(Collision2D other) {
        Stats collisionScript = other.gameObject.GetComponent<Stats>();
        if (collisionScript != null) {
            collisionScript.ProjectileImpact(this);
            Destroy(gameObject);
        }
        // TODO: Just use the unity Physics2D layer collision matrix for this in Project Settings > Physics2D
        if (other.gameObject.tag == "Projectile" || other.gameObject.tag == "ProjectileIgnoreObject") {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            return;
        }
        Destroy(gameObject);
    }

    public float GetDamage() {
        return damage;
    }
}
