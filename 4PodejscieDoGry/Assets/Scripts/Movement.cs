using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 7;
    public float jumpSpeed = 15;
    public float gravity = -0.25f; //-0.25f
    float speedMulti = 10;
    float speedDiv = 2;
    public float mouseSensitivity = 1.5f;
    float cameraPitch = 0.0f;
    [SerializeField] bool lockCursor = true;
    public bool jumping = false;

    [SerializeField] Transform playerCamera = null;
    private CharacterController cc;
    public Vector3 moveDirection = Vector3.zero;
    private bool groundedPlayer;
    public float click = 2;

    public void Start()
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

        if (click <= 0 && groundedPlayer)
        {
            click = 2;           
        }
    }

    public void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        cameraPitch -= mouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);
    }

    public void UpdateMovement()
    {
        //------------------------------WALKING-------------------------------\\
        groundedPlayer = cc.isGrounded;

        //moveDirection = (Input.GetAxisRaw("Horizontal") * speed) + (Input.GetAxisRaw("Vertical") * speed);
        moveDirection.x = Input.GetAxisRaw("Horizontal") * speed;
        moveDirection.z = Input.GetAxisRaw("Vertical") * speed;

        
        //------------------------------JUMP------------------------------//
        if (Input.GetButtonDown("Jump") && click >= 1)
        {
            jumping = true;
            moveDirection.y = jumpSpeed;
            click--;           
        }
        //------------------------------GRAVITY---------------------------//

        if (!groundedPlayer)
        {
            moveDirection.y += gravity;
        }
        else if (groundedPlayer && !jumping)
        {
            moveDirection.y = -21f;
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
        //-----------------------RETURN TO NORMAL SPEED-------------------//
        if (!Input.GetKey("left ctrl") && groundedPlayer && !Input.GetKey("left shift"))
        {
            speed = 5;
        }
        //--------------------------PORUSZANIE GRACZA---------------------//
        moveDirection = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);       
        cc.Move(transform.rotation * moveDirection * Time.deltaTime);

        jumping = false;

        Debug.Log(moveDirection.y);
        Debug.Log(jumping);
    }
}
