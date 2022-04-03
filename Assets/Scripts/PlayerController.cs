using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity = Vector3.zero;
    private float gravity = Physics.gravity.y;


    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float runningSpeed = 4.0f;
    [SerializeField] private float jumpHeight = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Input.GetButton("Sprint"))
        {
            controller.Move(move * Time.deltaTime * runningSpeed);
        }
        else
        {
            controller.Move(move * Time.deltaTime * speed);
        }

        if (controller.isGrounded && Input.GetButton("Jump") )
        {
            playerVelocity.y = jumpHeight;
        }
        
        playerVelocity.y += gravity * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);

    }
}
