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

    // Placeholder for the object details
    [SerializeField]
    private Sprite object2Image;
    [SerializeField]
    private string object2Description = "A rare diamond sphere with mystical powers.";
    [SerializeField]
    private float object2Price = 100f;

    private Camera cameraControl; // Reference to the camera control script

    void Start()
    {
        cameraControl = GetComponent<PlayerLook>().cam;
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
        shopPanel.SetActive(false);  // Make sure the shop panel is hidden at the start

        // Assign the Buy button click listener
        buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    void Update()
    {
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
                    additionalMessage = "Press 'E' to open the shop!";
                }
                else if (hitInfo.collider.gameObject.CompareTag("Object2"))
                {
                    promptMessage = interactable.promptMessage;
                    additionalMessage = "Rare diamond Sphere - Press 'E' to view details!";
                }

                playerUI.UpdateText(promptMessage, additionalMessage);

                if (inputManager.onFoot.Interact.triggered)
                {
                    interactable.BaseInteract();

                    if (hitInfo.collider.gameObject.CompareTag("Object2"))
                    {
                        OpenShopPanel();
                    }
                }
            }
        }
    }

    private void OpenShopPanel()
    {
        objectImage.sprite = object2Image;
        descriptionText.text = object2Description;
        priceText.text = "Price: $" + object2Price.ToString("F2");

        // Open the shop panel
        shopPanel.SetActive(true);

        // Disable camera movement and unlock the cursor
        LockCursorr.SetCursorState(false);
        cameraControl.enabled = false;
    }

    private List<string> shoppingCart = new List<string>();

    private void OnBuyButtonClick()
    {
        // Add the item to the shopping cart
        shoppingCart.Add("Object2 - Rare Diamond Sphere");

        // Log to console
        Debug.Log("Object2 added to the shopping cart!");

        // Close the shop panel after purchase
        shopPanel.SetActive(false);

        // Re-enable camera movement and lock the cursor
        LockCursorr.SetCursorState(true);
        cameraControl.enabled = true;
    }
}
