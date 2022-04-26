using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5;
    float jumpSpeed = 10;
    float gravity = -0.25f;
    float speedMulti = 10;
    float speedDiv = 2;
    [SerializeField] float mouseSensitivity = 3.5f;
    float cameraPitch = 0.0f;
    [SerializeField] bool lockCursor = true;

    [SerializeField] Transform playerCamera = null;
    private CharacterController cc;
    private Vector3 velocity = Vector3.zero;
    private bool groundedPlayer;

    void Start()
    {
        cc = GetComponent<CharacterController>();

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;        
        }
    }

    public void Update()
    {
        UpdateMovement();
        UpdateMouseLook();
    }

    void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        cameraPitch -= mouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    {
        //------------------------------WALKING-------------------------------\\
        groundedPlayer = cc.isGrounded;

        //Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //dir.Normalize();
        velocity.x = Input.GetAxisRaw("Horizontal") * speed;
        velocity.z = Input.GetAxisRaw("Vertical") * speed;

        //------------------------------JUMP------------------------------//
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            velocity.y = jumpSpeed;
        }
        //------------------------------GRAVITY---------------------------//
        if (!groundedPlayer)
        {
            velocity.y += gravity;
        }

        //------------------------------SPRINT----------------------------//
        if (Input.GetKey("left shift"))
        {
            speed = speedMulti;
        }

        //------------------------------SNEAK-----------------------------//
        if (Input.GetKey("left ctrl") && groundedPlayer)
        {
            speed = speedDiv;
        }
        if (!Input.GetKey("left ctrl") && groundedPlayer && !Input.GetKey("left shift"))
        {
            speed = 5;
        }

        //--------------------------PORUSZANIE GRACZA---------------------//
        velocity = new Vector3(velocity.x, velocity.y, velocity.z);
        cc.Move(velocity * Time.deltaTime);
    }
}
