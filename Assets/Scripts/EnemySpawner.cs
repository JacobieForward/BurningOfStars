using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] float spawnTime;
    float spawnTimer;

    [SerializeField] GameObject enemyToSpawn;

    Transform spawnPoint;

    void Awake() {
        spawnPoint = transform.GetChild(0);
    }

    void Update() {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnTime) {
            SpawnEnemy();
            spawnTimer = 0.0f;
        }
    }

    void SpawnEnemy() {
        if (enemyToSpawn != null && spawnPoint != null) {
            Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
        } else {
            print("Spawn point missing enemy to spawn or spawnpoint child gameobject.");
        }
    }
}
