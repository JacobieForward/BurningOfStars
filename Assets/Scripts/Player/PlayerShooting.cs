using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class PlayerShooting : MonoBehaviour {
    [SerializeField] Weapon currentWeapon;

    GameObject weaponHolder;

    float fireTimer = 0.0f;

    void Awake() {
        weaponHolder = gameObject.transform.GetChild(0).gameObject;
        EquipWeapon(currentWeapon);
    }

    void Update() {
        fireTimer += Time.deltaTime;
        if (Input.GetMouseButton(0)) {
            if (currentWeapon != null && fireTimer >= currentWeapon.GetFireRate()) {
                FirePrimaryWeapon();
            }
        }
    }

    void FirePrimaryWeapon() {
        GameObject projectileInstance = Instantiate(currentWeapon.GetProjectile(), gameObject.transform.position, gameObject.transform.rotation);
        projectileInstance.transform.Rotate(0.0f, 0.0f, Random.Range(-currentWeapon.GetSpread(), currentWeapon.GetSpread()));
        Physics2D.IgnoreCollision(projectileInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        fireTimer = 0.0f;
    }

    public void EquipWeapon(Weapon newWeapon) {
        if (newWeapon == null) {
            return;
        }
        currentWeapon = newWeapon;
        // TODO: During polish possibly just make sprites visible/invisible rather than removing and instantiating, should be more efficient
        try { Destroy(weaponHolder.transform.GetChild(0).gameObject); } catch (UnityException e) { } // First remove old sprite
        GameObject newWeaponSprite = Instantiate(currentWeapon.GetSprite(), weaponHolder.transform); // Instantiate new sprite
    }

    public Weapon GetCurrentWeapon() {
        return currentWeapon;
    }
}
