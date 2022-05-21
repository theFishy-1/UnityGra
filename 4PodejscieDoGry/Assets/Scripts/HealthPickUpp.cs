using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUpp : MonoBehaviour
{
    bool uleczono = false;
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
        {
            DestroyPlayer destroy = other.gameObject.GetComponent<DestroyPlayer>();

            if(destroy.health > 0 && destroy.health < destroy.maxHealth)
            {
                destroy.health = destroy.maxHealth;
                destroy.Heal();
                destroy.startInvoke = true;  
                uleczono = true;            
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        DestroyPlayer destroy = other.gameObject.GetComponent<DestroyPlayer>();
        if (uleczono == true)
        {
            Destroy(gameObject);
        }
    }
}
