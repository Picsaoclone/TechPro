using UnityEngine;
using UnityEngine.EventSystems;  // Important for UI detection

public class CameraController : MonoBehaviour
{
    public float sensitivity = 2f;
    public Transform playerBody;
    private bool isCameraActive = true;

    void Update()
    {
        // Check if the user is interacting with a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // If the pointer is over a UI element, stop the camera follow
            isCameraActive = false;
        }
        else if (Input.GetMouseButton(1)) // Right-click to re-enable camera follow
        {
            isCameraActive = true;
        }

        // Camera follow logic only if camera control is active
        if (isCameraActive)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            playerBody.Rotate(Vector3.up * mouseX);
            transform.localRotation = Quaternion.Euler(-mouseY, 0f, 0f);
        }
    }
}
