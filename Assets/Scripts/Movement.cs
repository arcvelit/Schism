using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float jumpHeight;

    public Transform spawnPoint;

    float WALKING_SPEED = 6;
    float RUNNING_SPEED = 9;

    [SerializeField] CharacterController controller;

    Vector3 velocity;
    float gravity = 2 * -9.81f;
    [SerializeField] bool isGrounded;

    [HideInInspector] public bool isRunning;
    private bool canSprint = true;
    public float MAX_STAMINA = 5;
    public float stamina = 0;
    public int staminaPercent => (int)(100 * stamina/MAX_STAMINA);

    private Vector3 lastPosition;

    void Start()
    {
        stamina = MAX_STAMINA;
        speed = WALKING_SPEED;
        isRunning = false;
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 + 0.3f);

        if(isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
        }

        if(Input.GetButtonDown("Jump") && isGrounded) 
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movementDelta = transform.position - lastPosition;
        lastPosition = transform.position;
        isRunning = Input.GetKey(KeyCode.LeftShift) && z != 0 && canSprint && movementDelta.magnitude > 0.01f;
        Debug.Log(movementDelta.magnitude);

        if(isRunning)
        {
            stamina = Mathf.Clamp(stamina - Time.deltaTime, 0, MAX_STAMINA);
            if(stamina <= 0) canSprint = false;
            // UIManager.Instance.UpdateStaminaBar(staminaPercent);
        }

        if(!isRunning)
        {
            stamina = Mathf.Clamp(stamina + Time.deltaTime, 0, MAX_STAMINA);
            if(!canSprint && stamina >= 2) canSprint = true;
            // UIManager.Instance.UpdateStaminaBar(staminaPercent);
        }

        speed = isRunning ? RUNNING_SPEED : WALKING_SPEED;
        Vector3 move = transform.right * x + transform.forward * z;
        if (move.magnitude > 1)
        {
            move = move.normalized;
        }
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    public void BackToSpawn()
    {
        gameObject.transform.position = spawnPoint.position;
    }


}
