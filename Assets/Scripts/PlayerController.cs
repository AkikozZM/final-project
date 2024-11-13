using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    public Transform cameraTransform;
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    // Ground check variables
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reset velocity if grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -10f;
        }

        // Get input for horizontal and vertical movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Get the direction of camera
        Vector3 forward = cameraTransform.forward; ;
        Vector3 right = cameraTransform.right;

        // Ignore the y component to keep movement on the horizontal plane
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Move the player
        Vector3 move = (forward * z + right * x).normalized;
        controller.Move(move * speed * Time.deltaTime);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //Debug.Log("jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
