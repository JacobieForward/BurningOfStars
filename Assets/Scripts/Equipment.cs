using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour {
    [SerializeField] Weapon currentWeapon;
    [SerializeField] Weapon offhandWeapon;

    [SerializeField] int currentWeaponAmmunition; // Ammunition should really be stored in an instance object but that was too much refactor for my taste, maybe another time
    [SerializeField] int offhandWeaponAmmunition;

    GameObject weaponHolder;

    void Awake() {
        weaponHolder = gameObject.transform.GetChild(0).gameObject;
        InitializeWeapons();
    }

    void InitializeWeapons() {
        // Assumes the starting weapon to never be null
        GameObject newWeaponSprite = Instantiate(currentWeapon.GetSprite(), weaponHolder.transform); // Instantiate new sprite
        currentWeaponAmmunition = currentWeapon.GetMaxAmmunition();
        if (offhandWeapon != null) {
            offhandWeaponAmmunition = offhandWeapon.GetMaxAmmunition();
        }
    }

    public void EquipWeapon(Weapon newWeapon) {
        if (newWeapon == null) {
            return;
        }
        // Assumes currentWeapon to never be null
        if (currentWeapon != null && offhandWeapon == null) {
            offhandWeapon = currentWeapon;
            offhandWeaponAmmunition = currentWeaponAmmunition;
            currentWeapon = newWeapon;
            currentWeaponAmmunition = newWeapon.GetMaxAmmunition();
            SwapCurrentWeaponSprite(newWeapon);
        } else {
            currentWeapon = newWeapon;
            currentWeaponAmmunition = newWeapon.GetMaxAmmunition();
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
        int tempAmmunition = currentWeaponAmmunition;
        currentWeapon = offhandWeapon;
        offhandWeapon = temp;
        currentWeaponAmmunition = offhandWeaponAmmunition;
        offhandWeaponAmmunition = tempAmmunition;

        SwapCurrentWeaponSprite(offhandWeapon);
    }

    public bool ShouldPickupSwapWithCurrentWeapon(Weapon mainWeaponBeforePickup, Weapon offHandWeaponBeforePickup) {
        if (mainWeaponBeforePickup != null && offHandWeaponBeforePickup != null) {
            return true;
        }
        return false;
    }

    public int GetCurrentWeaponAmmunition() {
        return currentWeaponAmmunition;
    }

    public void SetCurrentWeaponAmmunition(int newWeaponAmmunition) {
        currentWeaponAmmunition = newWeaponAmmunition;
    }

    public void UseAmmunition() {
        currentWeaponAmmunition -= 1;
    }
}
