using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5;
    float jumpSpeed = 10;
    float gravity = -9.81f;
    float speedMulti = 10;

    private CharacterController cc;
    private Vector3 velocity = Vector3.zero;
    private bool groundedPlayer;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    public void Update()
    {
        groundedPlayer = cc.isGrounded;
              
        velocity.x = Input.GetAxis("Horizontal") * speed;
        velocity.z = Input.GetAxis("Vertical") * speed;
                      
        //------------------------------JUMP------------------------------//
        if (Input.GetButton("Jump") && groundedPlayer)
        {
            velocity.y = (jumpSpeed) * Time.deltaTime;
        }
        //------------------------------JUMP------------------------------//

        //------------------------------GRAVITY---------------------------//
        velocity += Physics.gravity;
        //------------------------------GRAVITY---------------------------//

        //------------------------------SPRINT----------------------------//
        if (Input.GetKey("left shift"))
        {
            speed = speedMulti;
        }
        //------------------------------SPRINT----------------------------//

        velocity = new Vector3(velocity.x, velocity.y, velocity.z) * Time.deltaTime;
        cc.Move(velocity);
    }
}
