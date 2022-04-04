using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity = Vector3.zero;
    private float gravity = Physics.gravity.y;
    private float turnSmoothVelocity;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float runningSpeed = 4.0f;
    [SerializeField] private float jumpHeight = 5.0f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private Transform cam;

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, 
                ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (Input.GetButton("Sprint"))
            {
                controller.Move(moveDirection.normalized * runningSpeed * Time.deltaTime);
            }
            else
            {
                controller.Move(moveDirection.normalized * speed * Time.deltaTime);
            }
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
