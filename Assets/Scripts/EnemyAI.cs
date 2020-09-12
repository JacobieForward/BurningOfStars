using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// TODO: If I bother this whole class should be cleaned up and maybe spread throughout multiple classes. At the moment it is straight up spaghetti.
#pragma warning disable CS0649
public class EnemyAI : MonoBehaviour {
    // TODO: During polish/refactor potentially take a lot of the utility functions out of here and put them in a script that many scripts can use or inherit from
    [SerializeField] BehaviorType type;
    [SerializeField] float attackRange;
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;

    [Range(0, 360)]
    public float firingArc;
    Transform lastKnownTargetTransform;
    AIDestinationSetter aiDestinationSetter;
    [SerializeField] GameObject temporaryWaypoint;

    GameObject weaponHolder;
    [SerializeField] GameObject target;
    GameObject currentWaypoint;

    Shooting shooting;

    // TODO: During polish, as with InteractableObject, try to find a more elegant solution instead of lazy enum switch implementation.
    enum BehaviorType {
        Dummy, // Stands and does not move or do any actions
        Turret, // Stands still and rotates to track targets, fires at the target
        Pursuer, // Stands still until seeing the player, then rotates and tracks and fires, if the target goes out of range then follow and fire when in range
        Seeker, // Immediately pursues the player for good
        Patroller, // Patrols back and forth on a path, upon seeing a target then assume pursuer behavior
        StrictPatroller // Patrols back and forth on a path, upon seeing a target assume turret behavior, when no target in sight resume patrolling
    }

    void Awake() {
        shooting = GetComponent<Shooting>();
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        if (type == BehaviorType.Seeker) {
            aiDestinationSetter.target = GameObject.FindWithTag("Player").transform;
        }
    }

    void Update() {
        // TODO: Rethink the architecture of the AI, possibly add a state machine or implement decision trees and make all methods called here individualistic
        switch (type) {
            case BehaviorType.Dummy:
                break;
            case BehaviorType.Turret:
                LookForClosestVisibleTargetInRange();
                RotateToTarget();
                Fire();
                break;
            case BehaviorType.Pursuer:
                LookForClosestVisibleTargetInRange();
                RotateToTarget();
                IfNoTargetMoveToLastTargetPosition();
                Fire();
                break;
            case BehaviorType.Seeker:
                // Note in awake Seeker types have the player's transform added so outside of these methods they will use the A* pathfinding Seeker script to go towards the player
                LookForClosestVisibleTargetInRange();
                RotateToTarget();
                Fire();
                break;
            case BehaviorType.Patroller:
                break;
            case BehaviorType.StrictPatroller:
                break;
        }
    }

    void LookForClosestVisibleTargetInRange() {
        List<GameObject> newTargets = GetTargetsInRange();
        List<GameObject> newVisibleTargets = GetVisibleGameObjectsFromList(newTargets);
        GameObject closestVisibleTarget = GetClosestGameObjectFromList(newVisibleTargets);

        if (closestVisibleTarget == null && target != null) {
            lastKnownTargetTransform = target.transform;
            if (currentWaypoint != null) {
                Destroy(currentWaypoint);
            }
        }

        target = closestVisibleTarget;
    }

    // TODO: Add list/array validation methods for guard clauses to clean up and shorten code
    List<GameObject> GetTargetsInRange() {
        Collider2D[] overlapColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, attackRange, LayerMask.GetMask("Actor"), -100000.0f, 100000.0f);
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
            RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(0).GetChild(0).position, heading, LayerMask.GetMask("Actor"));
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
        RotateToLookAtPosition(target.transform.position);
    }

    void IfNoTargetMoveToLastTargetPosition() {
        if (target != null || lastKnownTargetTransform == null) {
            aiDestinationSetter.target = null;
            return;
        }
        if (currentWaypoint == null) {
            SetTargetAtNewWaypoint(lastKnownTargetTransform);
        }
    }

    void Fire() {
        if (target == null) {
            return;
        }
        if (CheckIfTargetInFiringArc()) {
            if (shooting.AbleToShoot()) {
                shooting.ShootPrimaryWeapon();
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    bool CheckIfTargetInFiringArc() {
        Vector3 dirToTarget = transform.position - target.transform.position;
        // Somehow -transform.up = green axis
        float angle = Mathf.Abs(Vector2.Angle(-transform.up, dirToTarget));
        if (angle < firingArc) {
            return true;
        }
        return false;
    }

    void MoveToPosition(Vector3 newPosition) {
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
    }
    
    void RotateToLookAtPosition(Vector3 positionToLookAt) {
        Vector3 dir = positionToLookAt - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // angle - 90 so the enemy rotates along the y axis
        angle -= 90.0f;
        Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
    }

    public void OnProjectileImpact() {
        SetTargetAtNewWaypoint(GameObject.Find("Player").transform);
    }

    void SetTargetAtNewWaypoint(Transform waypointTransform) {
        if (type != BehaviorType.Pursuer) {
            return;
        }
        GameObject newWaypoint = Instantiate(temporaryWaypoint, waypointTransform.transform.position, waypointTransform.transform.rotation);
        aiDestinationSetter.target = newWaypoint.transform;
        Destroy(currentWaypoint);
        currentWaypoint = newWaypoint;
    }
}