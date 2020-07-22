using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour {
    [SerializeField] Weapon currentWeapon;
    [SerializeField] Weapon offhandWeapon;
    // [SerializeField] 

    GameObject weaponHolder;

    void Awake() {
        weaponHolder = gameObject.transform.GetChild(0).gameObject;
        InitializeWeapons();
    }

    void InitializeWeapons() {
        GameObject newWeaponSprite = Instantiate(currentWeapon.GetSprite(), weaponHolder.transform); // Instantiate new sprite
    }

    public void EquipWeapon(Weapon newWeapon) {
        if (newWeapon == null) {
            return;
        }
        // Assumes currentWeapon to never be null
        if (currentWeapon != null && offhandWeapon == null) {
            offhandWeapon = currentWeapon;
            currentWeapon = newWeapon;
            SwapCurrentWeaponSprite(newWeapon);
        } else {
            currentWeapon = newWeapon;
            SwapCurrentWeaponSprite(newWeapon);
        }
    }

    public Weapon GetCurrentWeapon() {
        return currentWeapon;
    }

    public Weapon GetOffHandWeapon() {
        return offhandWeapon;
    }

    void SwapCurrentWeaponSprite(Weapon newWeapon) {
        // TODO: During polish possibly just make sprites visible/invisible rather than removing and instantiating, should be more efficient
        try { Destroy(weaponHolder.transform.GetChild(0).gameObject); } catch (UnityException e) { print(e.ToString()); } // First remove old sprite
        GameObject newWeaponSprite = Instantiate(currentWeapon.GetSprite(), weaponHolder.transform); // Instantiate new sprite
    }

    public void SwitchMainAndOffHandWeapons() {
        if (offhandWeapon == null) {
            return;
        }
        Weapon temp = currentWeapon;
        currentWeapon = offhandWeapon;
        offhandWeapon = temp;

        SwapCurrentWeaponSprite(offhandWeapon);
    }

    public bool ShouldPickupSwapWithCurrentWeapon(Weapon mainWeaponBeforePickup, Weapon offHandWeaponBeforePickup) {
        if (mainWeaponBeforePickup != null && offHandWeaponBeforePickup != null) {
            return true;
        }
        return false;
    }
}
