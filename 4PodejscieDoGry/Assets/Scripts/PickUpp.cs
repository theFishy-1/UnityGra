using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpp : MonoBehaviour
{
    public TextMeshProUGUI ammunitionError;
    public bool HasCollected = false;

    private void OnTriggerEnter(Collider collision)
    {
        Shooting shooting = collision.gameObject.GetComponentInChildren<Shooting>();
        if (shooting)
        {
            if (shooting.currentAmmo < shooting.maxAmmoSize)
            {
                shooting.AddAmmo(shooting.maxAmmoSize);
                ammunitionError.SetText("Collected Ammo");
                ammunitionError.enabled = true;
                Seconds();                
                IEnumerator Seconds()
                {
                    yield return new WaitForSeconds(3.0f);
                    ammunitionError.enabled = false;
                    
                }
                HasCollected = true;
            }
            else
            {
                ammunitionError.SetText("Full Magazine!");
                ammunitionError.enabled = true;               
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Shooting shooting = collision.gameObject.GetComponentInChildren<Shooting>();
        if (shooting)
        {
            ammunitionError.enabled = false;
            if (HasCollected == true)
            {
                Destroy(this.gameObject);
            }
        }       
    }  
}
