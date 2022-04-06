using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private AbilitySystem abilitySystem;

    private CharacterController controller;
    private Vector3 playerVelocity = Vector3.zero;
    private float gravity = Physics.gravity.y;
    private float turnSmoothVelocity;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float runningSpeed = 4.0f;
    [SerializeField] private float jumpHeight = 5.0f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float rotationSpeed = 240f;
    [SerializeField] private Transform cam;
    [SerializeField] private GameObject aimTargetFollow;
    [SerializeField] private Image crosshair;

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        abilitySystem = GetComponent<AbilitySystem>();
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
            aimTargetFollow.SetActive(true);
            aimVirtualCamera.gameObject.SetActive(true);
            crosshair.gameObject.SetActive(true);
        }
        else
        {
            aimTargetFollow.SetActive(false);
            aimVirtualCamera.gameObject.SetActive(false);
            crosshair.gameObject.SetActive(false);
            aimTargetFollow.transform.rotation = transform.rotation;
        }

        if (aimVirtualCamera.gameObject.activeInHierarchy)
        {
            float v = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
            float h = Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed;

            if ((v < 0 || v > 0) && h == 0)
            {
                Vector3 dir = new Vector3(0f, v);

                transform.Rotate(dir.normalized * Time.deltaTime * rotationSpeed);
            }

            if ((h < 0 || h > 0) && v == 0)
            {
                Vector3 dir = new Vector3(-h, 0f);

                aimTargetFollow.transform.Rotate(dir.normalized * Time.deltaTime * rotationSpeed);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            abilitySystem.UseAbility(abilitySystem.selectedAbility);
        }

    }
}
