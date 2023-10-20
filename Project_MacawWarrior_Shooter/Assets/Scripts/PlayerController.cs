using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    [Header ("General")]
    public float gravityScale = -20f;

    [Header ("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    [Header ("Jump")]
    public float jumpHeight = 1.9f;

    [Header("Rotation")]
    public float rotationSensibility=10;

    private float cameraVerticalAngle;
    Vector3 moveInput = Vector3.zero;
    Vector3 rotationinput = Vector3.zero;
    CharacterController characterController;



    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
       
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        Move();

        Look();
    }

    private void Move()
    {
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //moveInput = transform.TransformDirection(moveInput) * walkSpeed;
        moveInput = Vector3.ClampMagnitude(moveInput, 1f);

        /*
        if (Input.GetButton("Sprint"))
        {
            moveInput = transform.TransformDirection(moveInput) * runSpeed;
        }*/

        if (Input.GetButton("Sprint"))
        {
            moveInput = transform.TransformDirection(moveInput) * runSpeed;
        }
        else
        {
            moveInput = transform.TransformDirection(moveInput) * walkSpeed;
        }

        if (Input.GetButtonDown("Jump"))
        {
            moveInput.y = Mathf.Sqrt(jumpHeight * -2f * gravityScale);

            characterController.Move(moveInput * Time.deltaTime);
        }

        moveInput.y += gravityScale * Time.deltaTime;
        characterController.Move(moveInput * Time.deltaTime);
    }

    private void Look()
    {
        rotationinput.x = Input.GetAxis("Mouse X") * rotationSensibility * Time.deltaTime;
        rotationinput.y = Input.GetAxis("Mouse Y") * rotationSensibility * Time.deltaTime;

        cameraVerticalAngle = cameraVerticalAngle + rotationinput.y;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -70, 70);

        transform.Rotate(Vector3.up * rotationinput.x);
        playerCamera.transform.localRotation = Quaternion.Euler(-cameraVerticalAngle, 0f, 0f);
    }
}
