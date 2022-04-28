using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Rigidbody projectile;
    public Transform Spawnpoint;
    public float TimeToLive = 0.1f;

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Rigidbody clone;
            clone = (Rigidbody)Instantiate(projectile, Spawnpoint.position, projectile.rotation);

            clone.velocity = Spawnpoint.TransformDirection(Vector3.forward * 20);
            Destroy(clone.gameObject, TimeToLive);
        }       
    }


}
