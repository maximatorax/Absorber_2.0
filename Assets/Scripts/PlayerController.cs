using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    private float gravity = Physics.gravity.y;


    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float runningSpeed = 4.0f;
    [SerializeField] private float jumpHeight = 5.0f;
    [SerializeField] private float rotationSpeed = 240f;

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = v * transform.forward + h * transform.right;

        if (move.magnitude > 1f) move.Normalize();

        // Calculate the rotation for the player
        move = transform.InverseTransformDirection(move);

        // Get Euler angles
        float turnAmount = Mathf.Atan2(move.x, move.z);

        transform.Rotate(0, turnAmount * rotationSpeed * Time.deltaTime, 0);

        moveDirection = transform.forward * move.magnitude;

        if (Input.GetButton("Sprint"))
        {
            controller.Move(moveDirection.normalized * Time.deltaTime * runningSpeed);
        }
        else
        {
            controller.Move(moveDirection.normalized * Time.deltaTime * speed);
        }

        if (controller.isGrounded && Input.GetButton("Jump") )
        {
            playerVelocity.y = jumpHeight;
        }
        
        playerVelocity.y += gravity * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);


        if (Input.GetButton("Fire2"))
        {
            aimVirtualCamera.gameObject.SetActive(true);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
        }

    }
}
