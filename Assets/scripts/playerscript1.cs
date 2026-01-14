using System;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.Mathematics;
using Unity.Collections;

[RequireComponent(typeof(CharacterController))]
public class playerscript1 : MonoBehaviour
{
    private CharacterController Charcon;

    public Camera playerCamera;
    [SerializeField] private GameObject fireballprefab;
    [SerializeField] private GameObject Character;
    [SerializeField] private float movementspeed = 0.1f;
    [SerializeField] private float jumpforce = 0.4f;
    [SerializeField] private float runspeed = 0.5f;
    [SerializeField] private float lookspeed = 0.1f;
    [SerializeField] private float gravity = -10f;
    [SerializeField] private float yVelocity = 0f;
    public Vector2 turn;

    void Start()
    {
        Charcon = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        playerCamera = Camera.main;
    }

    void Update()
    {
        // Mouse Look
        turn.x += Input.GetAxis("Mouse X") * lookspeed;
        turn.y += Input.GetAxis("Mouse Y") * lookspeed;

        transform.localRotation = quaternion.Euler(0, turn.x, 0);
        playerCamera.transform.localRotation = quaternion.Euler(-turn.y, 0, 0);

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * moveVertical + right * moveHorizontal;

        float currentSpeed = Input.GetKey(KeyCode.Q) ? runspeed : movementspeed;

        Charcon.Move(move * Time.deltaTime * currentSpeed);

        if (Charcon.isGrounded && yVelocity < 0)
        {
            Debug.Log(Charcon.isGrounded + " " + yVelocity);
            yVelocity = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Charcon.isGrounded)
        {
            Debug.Log(Charcon.isGrounded + " " + ",space is pressed");
            yVelocity = jumpforce;
        }

        yVelocity += gravity * Time.deltaTime;
        Charcon.Move(new Vector3(0f, yVelocity, 0f) * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.I))
        {
            Vector3 spawnpos = fireballprefab.transform.position + transform.forward;
            Instantiate(fireballprefab, spawnpos, quaternion.identity);
        }
        
    }

}


