using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform orientation;
    public Transform animalTrans;
    public Transform playerObj;
    public float rotationSpeed;

    void Update()
    {
        // Get horizontal camera direction (ignoring tilt)
        Vector3 flatForward = transform.forward;
        flatForward.y = 0f;
        flatForward.Normalize();

        // Set orientation to match horizontal camera direction
        orientation.forward = flatForward;

        // Get input direction relative to orientation
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Face the direction of movement
        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
