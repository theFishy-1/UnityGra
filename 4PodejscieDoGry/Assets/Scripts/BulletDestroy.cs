using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Pizda")
        {
            Destroy(other.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
