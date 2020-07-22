using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class Projectile : MonoBehaviour {
    [SerializeField] float speed;
    [SerializeField] float damage;

    //float whenInstantiated;

    private void Awake() {
        //whenInstantiated = Time.timeSinceLevelLoad;
    }

    void Update() {
        transform.position += transform.right * Time.deltaTime * speed;
        //float timeSinceInstantiated = Time.timeSinceLevelLoad - whenInstantiated;
        //if (timeSinceInstantiated > 1.5f) {
        //    Destroy(gameObject);
        //}
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        Stats collisionScript = other.gameObject.GetComponent<Stats>();
        if (collisionScript != null) {
            collisionScript.ProjectileImpact(this);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Projectile") {
            return;
        }
        Destroy(gameObject);
    }

    public float GetDamage() {
        return damage;
    }
}
