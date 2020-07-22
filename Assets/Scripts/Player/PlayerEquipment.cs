using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour {
    Equipment equipment;

    void Awake() {
        equipment = GetComponent<Equipment>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            equipment.SwitchMainAndOffHandWeapons();
        }
    }
}
