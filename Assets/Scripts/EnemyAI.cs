using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class EnemyAI : MonoBehaviour {
    // TODO: During polish/refactor potentially take a lot of the utility functions out of here and put them in a script that many scripts can use or inherit from
    [SerializeField] BehaviorType type;
    [SerializeField] float attackRange;
    [SerializeField] float turnSpeed;
    [SerializeField] Weapon currentWeapon;

    [Range(0, 360)]
    public float firingArc;
    [SerializeField] bool playerInSight;
    [SerializeField] Vector3 personalLastSighting;

    GameObject weaponHolder;
    GameObject target = null;

    // TODO: During polish, as with InteractableObject, try to find a more elegant solution instead of lazy enum switch implementation.
    enum BehaviorType {
        Dummy, // Stands and does not move or do any actions
        Turret, // Stands still and rotates to track targets, fires at the target
        Pursuer, // Stands still until seeing the player, then rotates and tracks and fires, if the target goes out of range then follow and fire when in range
        Patroller, // Patrols back and forth on a path, upon seeing a target then assume pursuer behavior
        StrictPatroller // Patrols back and forth on a path, upon seeing a target assume turret behavior, when no target in sight resume patrolling
    }

    void Awake() {
        // TODO: Copy pasted code from PlayerShooting, that just feels wrong, should have an equipment system usable by both
        weaponHolder = gameObject.transform.GetChild(0).gameObject;
        EquipWeapon(currentWeapon);
    }

    // TODO: Copy pasted code from PlayerShooting, that just feels wrong, should have an equipment system usable by both
    public void EquipWeapon(Weapon newWeapon) {
        if (newWeapon == null) {
            return;
        }
        currentWeapon = newWeapon;
        // TODO: During polish possibly just make sprites visible/invisible rather than removing and instantiating, should be more efficient
        try { Destroy(weaponHolder.transform.GetChild(0).gameObject); } catch (UnityException e) { } // First remove old sprite
        GameObject newWeaponSprite = Instantiate(currentWeapon.GetSprite(), weaponHolder.transform); // Instantiate new sprite
    }

    void Update() {
        switch (type) {
            case BehaviorType.Dummy:
                DoNothing();
                break;
            case BehaviorType.Turret:
                LookForClosestVisibleTargetInRange();
                RotateToTarget();
                Fire();
                break;
            case BehaviorType.Pursuer:
                break;
            case BehaviorType.Patroller:
                break;
            case BehaviorType.StrictPatroller:
                break;
        }
    }

    void DoNothing() {
        // EXPERIMENTAL: This method exists solely for english syntax readability
        // TODO: ReEvaluate in refactor/polish
    }

    void LookForClosestVisibleTargetInRange() {
        List<GameObject> newTargets = GetTargetsInRange();
        List<GameObject> newVisibleTargets = GetVisibleGameObjectsFromList(newTargets);
        GameObject closestVisibleTarget = GetClosestGameObjectFromList(newVisibleTargets);
        target = closestVisibleTarget;
    }

    // TODO: Add list/array validation methods for guard clauses to clean up and shorten code
    List<GameObject> GetTargetsInRange() {
        Collider2D[] overlapColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, attackRange, LayerMask.GetMask("Default"), -100000.0f, 100000.0f);
        if (overlapColliders.Length > 0) {
            Collider2D nearestInteractableCollider = overlapColliders[0];
        } else {
            return new List<GameObject>();
        }
        List<GameObject> targetGameObjects = new List<GameObject>();
        foreach (Collider2D collider in overlapColliders) {
            if (collider.gameObject.tag == "Player") {
                targetGameObjects.Add(collider.gameObject);
            }
        }
        return targetGameObjects;
    }

    List<GameObject> GetVisibleGameObjectsFromList(List<GameObject> listOfGameObjects) {
        List<GameObject> visibleGameObjectsFromList = new List<GameObject>();
        if (listOfGameObjects == null || listOfGameObjects.Count == 0) {
            return visibleGameObjectsFromList;
        }
        foreach (GameObject gameObject in listOfGameObjects) {
            Vector2 heading = gameObject.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, heading);
            if (hit.collider.gameObject.tag == "Player") {
                visibleGameObjectsFromList.Add(gameObject);
            }
        }
        return visibleGameObjectsFromList;
    }

    GameObject GetClosestGameObjectFromList(List<GameObject> listOfGameObjects) {
        GameObject nearestGameObjectInList;
        if (listOfGameObjects != null && listOfGameObjects.Count > 0) {
            nearestGameObjectInList = listOfGameObjects[0];
        } else {
            return null;
        }
        foreach (GameObject gameObject in listOfGameObjects) {
            if (Vector2.Distance(gameObject.transform.position, transform.position) < Vector2.Distance(nearestGameObjectInList.transform.position, transform.position)) {
                nearestGameObjectInList = gameObject;
            }
        }
        return nearestGameObjectInList;
    }

    void RotateToTarget() {
        if (target == null) {
            return;
        }
        Vector3 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
    }

    void Fire() {
        if (target == null) {
            return;
        }
        if (CheckIfTargetInFiringArc()) {
            print("Firing");
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    bool CheckIfTargetInFiringArc() {
        // NOTE: Only works for angles around 90 degrees or less, dont use more than 80, maybe add a range to the firingArc variable
        Vector3 dirToTarget = transform.position - target.transform.position;
        // Minus 90 because unity starts with 90 instead of 0 for degrees
        if (((Vector2.Angle(transform.up, dirToTarget) - 90) < firingArc / 2)) {
            return true;
        }
        return false;
    }
}
