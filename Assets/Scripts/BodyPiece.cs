using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPiece : MonoBehaviour {

    Rigidbody2D rb;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        rb.velocity = Random.onUnitSphere * Random.Range(1, 5);
    }
}
