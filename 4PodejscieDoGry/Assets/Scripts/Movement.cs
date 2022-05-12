using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 8; //8 not normalized
    public float jumpSpeed = 15; //15 not normalized
    public float gravity = -0.25f; // -0.25f not normalized
    float speedMulti = 12; //12 not normalized
    float speedDiv = 5;  //5 not normalized
    public static float mouseSensitivity = 0.5f;
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

        moveDirection.x = Input.GetAxisRaw("Horizontal") * speed;
        moveDirection.z = Input.GetAxisRaw("Vertical") * speed;
       
        //------------------------------JUMP------------------------------//
        if (Input.GetButtonDown("Jump") && click >= 1)
        {
            jumping = true;
            moveDirection.y = jumpSpeed;
            speed = 18;
            click--;           
        }
        //------------------------------GRAVITY---------------------------//

        if (!groundedPlayer)
        {
            moveDirection.y += gravity;
        }
        else if (groundedPlayer && !jumping)
        {
            moveDirection.y = -0.25f;
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
            speed = 8;  //8 not normalized
        }
        //--------------------------PORUSZANIE GRACZA---------------------//
        moveDirection = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);
        //moveDirection = Vector3.ClampMagnitude(moveDirection, speed);
        cc.Move(transform.rotation * moveDirection * Time.deltaTime); //* Time.deltaTime);

        jumping = false;

        Debug.Log(moveDirection);
    }
}
