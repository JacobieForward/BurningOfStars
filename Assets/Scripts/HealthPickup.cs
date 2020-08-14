using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class HealthPickup : MonoBehaviour {
    [SerializeField] float healingAmount;
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Stats>().GainHealth(healingAmount);
            Destroy(gameObject);
        }
    }
}
