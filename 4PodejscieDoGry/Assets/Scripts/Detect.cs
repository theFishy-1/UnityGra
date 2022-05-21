using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect : MonoBehaviour
{
    bool detected;
    GameObject target;
    public Transform enemy;
    public GameObject bullet;
    public Transform shootPoint;
    public Transform shootPoint2;


    public float shootSpeed = 10f;
    public float timeToShoot = 1.3f;
    float originalTime;

    private void Start() {
        originalTime = timeToShoot;
    }

    private void Update() {
        if (detected){
            enemy.LookAt(target.transform);
        }
    }

    private void FixedUpdate() {
        if(detected)
        {
            timeToShoot -= Time.deltaTime;

            if (timeToShoot < 0)
            {
                ShootPlayer();
                timeToShoot = originalTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            detected = true;
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            detected = false;
            target = null;
        }
    }

    private void ShootPlayer()
    {
        GameObject currentBullet = Instantiate(bullet,  shootPoint.position, shootPoint.rotation);
        GameObject currentBullet2 = Instantiate(bullet, shootPoint2.position, shootPoint2.rotation);
        Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
        Rigidbody rb2 = currentBullet2.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * shootSpeed, ForceMode.VelocityChange);
        rb2.AddForce(transform.forward * shootSpeed, ForceMode.VelocityChange);
    }
}
