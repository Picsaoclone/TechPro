// using UnityEngine;
// using UnityEngine.UI;

// public class ShopPanelUI : MonoBehaviour
// {
//     [SerializeField] private Image objectImage;
//     [SerializeField] private Text descriptionText;
//     [SerializeField] private Text priceText;
//     [SerializeField] private Button buyButton;

//     private Interactable currentInteractable;

//     void Start()
//     {
//         buyButton.onClick.AddListener(OnBuyButtonClick);
//     }

//     public void UpdatePanel(Interactable interactable)
//     {
//         currentInteractable = interactable;

//         if (objectImage != null)
//         {
//             objectImage.sprite = interactable.objectImage;
//         }
//         if (descriptionText != null)
//         {
//             descriptionText.text = interactable.description;
//         }
//         if (priceText != null)
//         {
//             priceText.text = $"Price: ${interactable.price}";
//         }
//     }

//     private void OnBuyButtonClick()
//     {
//         if (currentInteractable != null)
//         {
//             // Add the item to the shopping cart
//             // ...

//             // Close the shop panel after purchase
//             gameObject.SetActive(false);

//             // Lock and hide the cursor again
//             LockCursorr.SetCursorState(true);

//             // Enable camera control
//             var playerInteract = FindObjectOfType<PlayerInteract>();
//             if (playerInteract != null)
//             {
//                 playerInteract.EnableCameraControl();
//             }
//         }
//     }
// }
