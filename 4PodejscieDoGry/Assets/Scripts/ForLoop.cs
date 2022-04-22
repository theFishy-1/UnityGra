using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForLoop : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        rb.AddForce(-transform.forward * 500f);
        rb.useGravity = true;
    }

}
