using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float jumpHeight;

    float WALKING_SPEED = 6;
    float RUNNING_SPEED = 9;

    [SerializeField] CharacterController controller;
    [SerializeField] Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    Vector3 velocity;
    float gravity = 2 * -9.81f;
    bool isGrounded;

    [HideInInspector] public bool isRunning;
    public float MAX_STAMINA = 5;
    public float stamina = 0;
    public int staminaPercent => (int)(100 * stamina/MAX_STAMINA);

    void Start()
    {
        stamina = MAX_STAMINA;
        speed = WALKING_SPEED;
        isRunning = false;
    }


    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        if(Input.GetButtonDown("Jump") && isGrounded) 
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            speed = RUNNING_SPEED;
            stamina -= Time.deltaTime;
            // UIManager.Instance.UpdateStaminaBar(staminaPercent);
        }
        else 
        {
            isRunning = false;
            speed = WALKING_SPEED;
            stamina = stamina < 0 ? 0 : (stamina > MAX_STAMINA ? MAX_STAMINA : stamina + Time.deltaTime);
            // UIManager.Instance.UpdateStaminaBar(staminaPercent);
        }

        Vector3 move = transform.right * x + transform.forward * z;
        if (move.magnitude > 1)
        {
            move = move.normalized;
        }
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
