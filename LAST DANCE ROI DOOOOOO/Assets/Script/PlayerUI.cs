using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;

    [SerializeField]
    private TextMeshProUGUI additionalMessageText; 

    // Update both the prompt text and the additional message text
    public void UpdateText(string promptMessage, string additionalMessage)
    {
        promptText.text = promptMessage;
        additionalMessageText.text = additionalMessage;
    }
}


