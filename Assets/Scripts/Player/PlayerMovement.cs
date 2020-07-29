using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class PlayerMovement : MonoBehaviour {
    [SerializeField] float movementSpeed;
    [SerializeField] float turnSpeed;
    [SerializeField] bool movementRelativeToRotation;

    void Update() {
        InputForMovement();
        InputForRotation();
    }

    void InputForMovement() {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        if (movementRelativeToRotation) {
            // Strafing is half as fast as regular movement
            inputHorizontal /= 2;
            // The player moves backwards at reduced speed
            if (inputVertical < 0) {
                inputVertical /= 2;
            }
            // Movement relative to rotation
            transform.position += transform.up * inputVertical * Time.deltaTime * movementSpeed;
            transform.position += -transform.right * inputHorizontal * Time.deltaTime * movementSpeed;
        } else {
            // Absolute movement
            transform.Translate(Vector3.up * inputVertical * Time.deltaTime * movementSpeed, Space.World);
            transform.Translate(Vector3.right * inputHorizontal * Time.deltaTime * movementSpeed, Space.World);
        }
    }

    void InputForRotation() {
        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // angle - 90 degrees due to weird issue where the angle was offset by 90 and pointed at the x instead of the y axis
        angle -= 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
    }
}
