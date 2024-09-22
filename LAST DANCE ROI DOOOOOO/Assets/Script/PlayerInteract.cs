using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteract : Interactable
{
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private InputManager inputManager;

    // References for the shop panel UI
    [SerializeField]
    private GameObject shopPanel;
    [SerializeField]
    private Image objectImage;
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    [SerializeField]
    private TextMeshProUGUI priceText;
    [SerializeField]
    private Button buyButton;

    // Shopping cart icon and counter
    [SerializeField]
    private Image shoppingCartIcon;
    [SerializeField]
    private TextMeshProUGUI cartItemCountText;
    private int cartItemCount = 0;

    private Camera cameraControl;

    [System.Serializable]
    public struct ObjectDetails
    {
        public Sprite image;
        public string description;
        public float price;
        public string name;
    }

    [SerializeField]
    private ObjectDetails[] objects;

    private ObjectDetails currentObject;

    [SerializeField]
    private GameObject notificationTextObject;
    [SerializeField]
    private TextMeshProUGUI notificationText;

    // Examination variables
    private bool isExamining = false;
    private GameObject examinedObject;
    private Vector3 lastMousePosition;
    [SerializeField]
    private GameObject offset; // The position where the object moves when examined
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        cameraControl = GetComponent<PlayerLook>().cam;
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
        shopPanel.SetActive(false);

        // Buy button setup
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyButtonClick);

        // Initialize cart UI
        UpdateCartUI();
    }

    void Update()
    {
        // Handle examination
        if (isExamining)
        {
            RotateExaminedObject();
            if (Input.GetKeyDown(KeyCode.F)) // Press "F" to stop examining
            {
                StopExamination();
            }
            return; // Skip raycast and other interactions while examining
        }

        playerUI.UpdateText(string.Empty, string.Empty);

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                string promptMessage = "";
                string additionalMessage = "";

                if (hitInfo.collider.gameObject.CompareTag("Object1"))
                {
                    promptMessage = interactable.promptMessage;
                    additionalMessage = "Press 'E' to view details, Press 'F' to examine";
                    currentObject = objects[0];
                }
                else if (hitInfo.collider.gameObject.CompareTag("Object2"))
                {
                    promptMessage = interactable.promptMessage;
                    additionalMessage = "Press 'E' to view details, Press 'F' to examine";
                    currentObject = objects[1];
                }
                else if (hitInfo.collider.gameObject.CompareTag("Object3"))
                {
                    promptMessage = interactable.promptMessage;
                    additionalMessage = "Press 'E' to view details, Press 'F' to examine";
                    currentObject = objects[2];
                }

                playerUI.UpdateText(promptMessage, additionalMessage);

                if (inputManager.onFoot.Interact.triggered)
                {
                    interactable.BaseInteract();
                    OpenShopPanel();
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    StartExamination(hitInfo.collider.gameObject);
                }
            }
        }
    }

    private void OpenShopPanel()
    {
        objectImage.sprite = currentObject.image;
        descriptionText.text = currentObject.description;
        priceText.text = "Price: " + currentObject.price.ToString("N0") + " VND";

        shopPanel.SetActive(true);

        LockCursorr.SetCursorState(false);
        cameraControl.enabled = false;
    }

    private bool hasBoughtItem = false;

    private void OnBuyButtonClick()
    {
        if (!hasBoughtItem)
        {
            hasBoughtItem = true;

            cartItemCount++;
            UpdateCartUI();

            ShowPurchaseNotification("Item purchased successfully!");

            shopPanel.SetActive(false);
            LockCursorr.SetCursorState(true);
            cameraControl.enabled = true;

            // Reset mouse position to the center of the screen
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(ResetBuyFlag());
        }
    }

    private void ShowPurchaseNotification(string message)
    {
        notificationTextObject.SetActive(true);
        notificationText.text = message;

        StartCoroutine(HideNotification());
    }

    private IEnumerator HideNotification()
    {
        yield return new WaitForSeconds(2f);
        notificationTextObject.SetActive(false);
    }

    private IEnumerator ResetBuyFlag()
    {
        yield return new WaitForSeconds(0.5f);
        hasBoughtItem = false;
    }

    private void UpdateCartUI()
    {
        // Update the shopping cart icon with the item count
        cartItemCountText.text = cartItemCount.ToString();
    }

    // Examination methods
    private void StartExamination(GameObject obj)
    {
        isExamining = true;
        examinedObject = obj;
        lastMousePosition = Input.mousePosition;

        // Save original position and rotation
        originalPosition = examinedObject.transform.position;
        originalRotation = examinedObject.transform.rotation;

        // Move the object to the offset position
        examinedObject.transform.position = offset.transform.position;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void StopExamination()
    {
        isExamining = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reset object position and rotation
        if (examinedObject != null)
        {
            examinedObject.transform.position = originalPosition;
            examinedObject.transform.rotation = originalRotation;
        }

        examinedObject = null;
    }

    private void RotateExaminedObject()
    {
        if (examinedObject != null)
        {
            float rotationSpeed = 150f; // Adjust rotation speed
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

            // Rotate object based on mouse movement
            examinedObject.transform.Rotate(Vector3.up, mouseDelta.x * rotationSpeed * Time.deltaTime, Space.World);
            examinedObject.transform.Rotate(Vector3.right, -mouseDelta.y * rotationSpeed * Time.deltaTime, Space.World);

            lastMousePosition = Input.mousePosition;
        }
    }
}
