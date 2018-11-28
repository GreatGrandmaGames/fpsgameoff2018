using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHideLock : MonoBehaviour
{
    private CursorLockMode wantedMode;

    // Apply requested cursor state
    void SetCursorState()
    {
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }

    void Start()
    {
        wantedMode = CursorLockMode.Locked;
        SetCursorState();
    }
    void Update()
    {
        // Release cursor on escape keypress
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = wantedMode = CursorLockMode.None;
        }
    }
}
