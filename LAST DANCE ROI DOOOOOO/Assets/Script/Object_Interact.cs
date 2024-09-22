using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Object_Interact : MonoBehaviour
{
    public GameObject offset;
    private PlayerInput _playerInput;
    GameObject targetObject;

    public bool isExamining = false;

    public Canvas _canva;

    public GameObject tableObject;

    private Vector3 lastMousePosition;

    private Transform examinedObject; // Store the currently examined object

    // List of position and rotation of the interactable objects 
    private Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Quaternion> originalRotations = new Dictionary<Transform, Quaternion>();

    void Start()
    {
        _canva.enabled = false;
        targetObject = GameObject.Find("PlayerCapsule");
        _playerInput = targetObject.GetComponent<PlayerInput>();
        // Enable the input actions for OnFoot
        _playerInput.OnFoot.Enable();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Object"))
            {
                ToggleExamination();

                if (isExamining)
                {
                    // Store the currently examined object and its original position and rotation
                    examinedObject = hit.transform;
                    originalPositions[examinedObject] = examinedObject.position;
                    originalRotations[examinedObject] = examinedObject.rotation;

                    // Enable the canvas when starting examination
                    _canva.enabled = true;
                    StartExamination();
                }
                else
                {
                    // Disable the canvas when stopping examination
                    _canva.enabled = false;
                    StopExamination();
                }
            }
        }

        if (isExamining)
        {
            Examine();
        }
        else if (CheckUserClose())
        {
            _canva.enabled = true;
        }
        else
        {
            _canva.enabled = false;
        }
    }


    public void ToggleExamination()
    {
        isExamining = !isExamining;
    }

    void StartExamination()
    {
        lastMousePosition = Input.mousePosition;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _playerInput.OnFoot.Disable(); // Disable player movement input

        if (examinedObject != null)
        {
            // Move the examined object to the offset position relative to the player
            examinedObject.position = offset.transform.position;
            examinedObject.SetParent(null); // Unparent the object to freely rotate it
        }
    }

    void NonExamine()
    {
        if (examinedObject != null)
        {
            // Reset the position and rotation of the examined object to its original values
            if (originalPositions.ContainsKey(examinedObject))
            {
                examinedObject.position = originalPositions[examinedObject];
            }
            if (originalRotations.ContainsKey(examinedObject))
            {
                examinedObject.rotation = originalRotations[examinedObject];
            }

            // Set the parent back to its original parent if needed
            // For example:
            // examinedObject.SetParent(originalParent); // where originalParent is the Transform of the object's original parent
        }
    }

    void Examine()
    {
        if (examinedObject != null)
        {
            // Rotate the examined object based on mouse movement
            Vector3 deltaMouse = Input.mousePosition - lastMousePosition;
            float rotationSpeed = 1.0f;

            // Rotate the object
            examinedObject.Rotate(Vector3.up, deltaMouse.x * rotationSpeed, Space.World);
            examinedObject.Rotate(Vector3.right, -deltaMouse.y * rotationSpeed, Space.World); // Invert y rotation for natural feel

            lastMousePosition = Input.mousePosition;
        }
    }

  
    void StopExamination()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _playerInput.OnFoot.Enable(); // Re-enable player movement input
    }
    bool CheckUserClose()
    {
        // Calculate the distance between the two GameObjects
        float distance = Vector3.Distance(targetObject.transform.position, tableObject.transform.position);
        // Check if they are close based on the threshold
        return (distance < 2);
    }
}
