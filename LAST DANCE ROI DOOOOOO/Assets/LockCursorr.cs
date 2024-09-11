using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCursorr : MonoBehaviour
{
    public static bool isCursorLocked = true;

    void Update()
    {
        ToggleCursorState(isCursorLocked);
    }

    public static void SetCursorState(bool locked)
    {
        isCursorLocked = locked;
    }

    private void ToggleCursorState(bool locked)
    {
        Cursor.visible = !locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
