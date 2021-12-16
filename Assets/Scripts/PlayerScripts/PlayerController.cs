using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    float currentSpeed = 5;

    [SerializeField]
    float walkSpeed = 5, runSpeed = 12;

    Camera cam;

    [SerializeField]
    float jumpForce = 15f;

    public bool isGrounded;

    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    float groundDistance;
    [SerializeField]
    LayerMask groundMask;

    Inventory inventory;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();

        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask, QueryTriggerInteraction.Ignore);

        UpdateCurrentSpeed();

        // Do not jump when player has inventory open
        if (inventory.inventoryOpen)
            return;

        JumpLogic();
    }

    void FixedUpdate()
    {
        // Do not move when player has inventory open
        if (inventory.inventoryOpen)
            return;

        Movement();
    }

    /// <summary>
    /// Change between walk and run speed based on sprint button input
    /// </summary>
    void UpdateCurrentSpeed()
    {
        if (Input.GetButton("Sprint"))
            currentSpeed = runSpeed;
        else
            currentSpeed = walkSpeed;
    }

    /// <summary>
    /// Players movement
    /// </summary>
    void Movement()
    {
        float hValue = Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        float vValue = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;

        Vector3 move = cam.transform.right * hValue + transform.forward * vValue;

        rb.MovePosition(transform.position + move);

    }

    /// <summary>
    /// Make character jump when jump button is pressed
    /// </summary>
    void JumpLogic()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    
}
