using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreColissions : MonoBehaviour
{
    private Collider col;
    private Rigidbody rigidBody;
    private Vector3 vel;
    private Vector3 angularVel;
    private Vector3 position;

    private void Start() {
        col = gameObject.GetComponent<Collider>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        vel = rigidBody.velocity;
        angularVel = rigidBody.angularVelocity;
        position = transform.position;
    }

    private void OnCollisionEnter(Collision collision) {
        Physics.IgnoreCollision(col, collision.collider);
        rigidBody.velocity = vel;
        rigidBody.angularVelocity = angularVel;
        transform.position = position;
    }
}
