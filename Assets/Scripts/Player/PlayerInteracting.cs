using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class PlayerInteracting : MonoBehaviour {
    [SerializeField] float interactionRadius;

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            InteractableObject objectToInteract = SearchForInteractableObject();
            if (objectToInteract != null) {
                InteractWithObject(objectToInteract);
            }
        }
    }

    InteractableObject SearchForInteractableObject() {
        // Either return null or an object, always the nearest to the center of the player
        // TODO: Later during polish phase add code to make the object be the nearest to the player, for now it is just whichever collider has the InteractableObject script and is last in the array
        Collider2D[] overlapColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, interactionRadius, LayerMask.GetMask("Default"), -100000.0f, 100000.0f);
        Collider2D nearestInteractableCollider = overlapColliders[0]; // Since the overlapCircle is centered on the player this default will always be the player, so if nothing else is picked up this function will always return null
        foreach (Collider2D collider in overlapColliders) {
            // There might be a more efficient way than getting component for each object, this does not run in update so it shouldn't be too bad as far as processing usage goes
            if (collider.GetComponent<InteractableObject>() != null) {
                // TODO: Add distance comparison during polish, for now no distance calculation for whichever collider is closest to the center point of the overlapCircle
                nearestInteractableCollider = collider;
            }
        }
        return nearestInteractableCollider.gameObject.GetComponent<InteractableObject>();
    }

    void InteractWithObject(InteractableObject obj) {
        obj.Interact();
    }
}
