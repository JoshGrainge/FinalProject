#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS IS THE INITIAL MOUSE LOOK SCRIPT THAT UNITY SPAWNS CHARACTER WITH. only slightly modified
public class LookWithMouse : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float xRotation = 0f;

    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponentInParent<Inventory>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Do not rotate camera when inventory is open
        if (inventory.inventoryOpen)
            return;


#if ENABLE_INPUT_SYSTEM
        float mouseX = 0, mouseY = 0;

        if (Mouse.current != null)
        {
            var delta = Mouse.current.delta.ReadValue() / 15.0f;
            mouseX += delta.x;
            mouseY += delta.y;
        }
        if (Gamepad.current != null)
        {
            var value = Gamepad.current.rightStick.ReadValue() * 2;
            mouseX += value.x;
            mouseY += value.y;
        }

        mouseX *= mouseSensitivity * Time.deltaTime;
        mouseY *= mouseSensitivity * Time.deltaTime;
#else
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
#endif

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
