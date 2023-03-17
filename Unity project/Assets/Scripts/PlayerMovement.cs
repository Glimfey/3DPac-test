using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]private float playerSpeed = 6.0f;
    private float sprintSpeed = 12.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = Physics.gravity.y;
    private bool isCrouching = false;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 movement = Vector3.zero;
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        movement += transform.forward * v * playerSpeed * Time.deltaTime;
        movement += transform.right * h * playerSpeed * Time.deltaTime;
        movement += Physics.gravity;
        controller.Move(movement);

        //if (move != Vector3.zero)
        //{
        //    gameObject.transform.forward = move;
        //}

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (Input.GetButtonDown("Crouch") && isCrouching == false)
        {
            Vector3 vector = new Vector3(1, 0.5f, 1);
            player.transform.localScale = vector;
            isCrouching = true;
            StartCoroutine((string)CrouchTime());
            
        }
        IEnumerable CrouchTime()
        {
            yield return new WaitForSeconds(5);
        }
        if (Input.GetButtonDown("Crouch") && isCrouching == true)
        {
            Vector3 vector = new Vector3(1, 1, 1);
            player.transform.localScale = vector;
            isCrouching = false;
        }
    }
}