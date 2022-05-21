using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDetector : MonoBehaviour
{
    public Transform player;
    public bool isGrounded;
    public bool check;
    
    void Update()
    {
        isGrounded = player.GetComponent<Movement>().groundedPlayer;

        if(isGrounded != true)
        {
            check = false;
            if (Input.GetAxisRaw("Horizontal") > .25f || Input.GetAxisRaw("Horizontal") < -.25f)
            {
                player.SetParent(null);
            }
            if (Input.GetAxisRaw("Vertical") > .25f || Input.GetAxisRaw("Vertical") < -.25f)
            {
                player.SetParent(null);
            }
        }
        if(check != true)
        {
            RaycastHit hit;

            Vector3 targetPoint; 
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.125f))
            {
                
            }
  
            if(hit.collider != null)
            {
                if (hit.collider.CompareTag("MovingPlatform"))
                {
                    player.SetParent(hit.transform);
                } 
                else
                {
                    player.SetParent(null);
                }     
                check = true;         
            }
        }       
    }
}
