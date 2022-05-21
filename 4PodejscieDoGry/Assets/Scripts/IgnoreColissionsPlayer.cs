using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreColissionsPlayer : MonoBehaviour
{
    public Collider col;

    private void Start() {
        col = gameObject.GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision) {
        Physics.IgnoreCollision(col, collision.collider);
    }
}
