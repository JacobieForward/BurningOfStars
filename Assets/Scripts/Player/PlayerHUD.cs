using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Stats))]
public class PlayerHUD : MonoBehaviour {
    Stats playerStats;
    Equipment playerEquipment;

    Slider playerHealthBar;
    Text playerAmmunitionCounter;

    void Awake() {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        playerEquipment = GameObject.FindGameObjectWithTag("Player").GetComponent<Equipment>();

        playerHealthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        playerAmmunitionCounter = GameObject.Find("CurrentWeaponAmmunitionText").GetComponent<Text>();
    }

    void Update() {
        try {
            playerHealthBar.maxValue = playerStats.GetMaxHealth();
            playerHealthBar.value = playerStats.GetCurrentHealth();
            playerAmmunitionCounter.text = playerEquipment.GetCurrentWeaponAmmunition().ToString();
            // TODO: Get rid of all this dirty try catching and just attach a script to the health bars to set their gameObject names automatically?
            // Then again how would we know the script was always attached?
        } catch (NullReferenceException) {
            Debug.Log("One of the healthbar gameObject names has been changed, this makes the PlayerHUD script unable to use them." +
                "Or an unknown issue is causing a NullReference exception in PlayerHUD. The constants file may also have been affected.");
        }
    }
}
