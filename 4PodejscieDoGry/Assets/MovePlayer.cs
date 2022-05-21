using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private float lastSpot = 0;

    private void OnTriggerEnter(Collider collision) 
    {
        if (collision.tag == "Player")
        {
            lastSpot = this.transform.position.x;
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if (other.tag == "Player")
        {
            if (lastSpot == 0)
            {
                lastSpot = this.transform.position.x;
            }
            if (other.GetComponent<CharacterController>().isGrounded)
            {
                float temp = lastSpot - this.transform.position.x;
                other.transform.position = new Vector3(other.transform.position.x - temp, other.transform.position.y, other.transform.position.z);
            }
            lastSpot = this.transform.position.x;
        }
    }
}
