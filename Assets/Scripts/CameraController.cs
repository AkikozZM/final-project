using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 40f;
    public float mouseSensitivity = 1f;
    public float minYAngle = -20f;
    public float maxYAngle = 80f;
    public float zoomSpeed = 5f;
    public float minDistance = 10f;
    public float maxDistance = 60f;

    private float currentX = 0f;
    private float currentY = 0f;

    void Start()
    {
        // Lock the cursor
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Get mouse movement
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Update rotation angles based on mouse input
            currentX += mouseX;
            currentY -= mouseY;

            // Clamp the vertical rotation angle to avoid flipping over
            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        }
        // Zoom in/out with mouse scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        // Clamp the zoom distance
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate the new position of the camera
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 direction = new Vector3(0, 0, -distance);
        transform.position = target.position + rotation * direction;

        // Look at the target
        transform.LookAt(target);
    }
}
