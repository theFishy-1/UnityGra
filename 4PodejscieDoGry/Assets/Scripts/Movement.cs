using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Player Movement Variables")]
    public float speed = 6;
    public float jumpSpeed = 3;
    public float gravity = -19.62f;
    float speedMulti = 8;
    float speedDiv = 4;
    public float click = 2;
    [Header("Mouse Look Variables")]
    public static float mouseSensitivity = 0.5f;
    float cameraPitch = 0.0f;
    [SerializeField] bool lockCursor = true;
    public bool jumping = false;
    [Header("Private Variables")]
    [SerializeField] Transform playerCameraLook = null;
    private CharacterController cc;
    public Vector3 moveDirection = Vector3.zero;
    public bool groundedPlayer;
    public void Start() {
        cc = GetComponent<CharacterController>();
        if (lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;               
    }
    public void Update() {
        UpdateMovement();
        UpdateMouseLook();
        if (click <= 0 && groundedPlayer)
            click = 2;                   
    }
    public void UpdateMouseLook() {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        cameraPitch -= mouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);    
        playerCameraLook.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);
    }
    public void UpdateMovement() {
        //------------------------------WALKING-------------------------------\\
        groundedPlayer = cc.isGrounded;
        if(groundedPlayer && moveDirection.y < 0) 
            moveDirection.y = -19.62f;       
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;      
        //------------------------------SPRINT----------------------------//
        if (Input.GetKey("left shift")) {
            speed = speedMulti;        
        }
        //------------------------------SNEAK-----------------------------//
        if (Input.GetKey("left ctrl") && groundedPlayer) {
            speed = speedDiv;
            cc.height = 0.5f;        
        }            
        //-----------------------RETURN TO NORMAL-------------------------//
        if (!Input.GetKey("left ctrl") && groundedPlayer && !Input.GetKey("left shift")) { 
            speed = 6;
            cc.height = 2f;        
        }
        //--------------------------PORUSZANIE GRACZA---------------------//
        cc.Move(move * speed * Time.deltaTime);
        moveDirection.y += gravity * Time.deltaTime;
        //------------------------------JUMP----------------------------//
        if (Input.GetButtonDown("Jump") && click >= 1) {
            jumping = true;
            moveDirection.y = Mathf.Sqrt(jumpSpeed * -1f * gravity);
            speed = 8;
            click--;
        }
        cc.Move(moveDirection * Time.deltaTime);
        jumping = false;
    }   
}
