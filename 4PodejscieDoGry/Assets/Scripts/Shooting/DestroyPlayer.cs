using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyPlayer : MonoBehaviour
{
    public float maxHealth;
    public float health = 60f;
    public Image bloodScreen;
    public Slider healthBar;
    Color alphaColor;

    public bool startInvoke = false;

    private void Start() {
        maxHealth = health;
        healthBar.maxValue = maxHealth;
        alphaColor = bloodScreen.color;
    }

    private void Update() {
        healthBar.value = health;

        if (startInvoke == true)
        {
            if (alphaColor.a > 0)
                Invoke("Heal", 0.1f);
            
            if (alphaColor.a <= 0)
            {
                startInvoke = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        alphaColor.a += .1f;
        bloodScreen.color = alphaColor;
        if (health <= 0f)
        {
            Death();
        }
    }

    void Death()
    {
        Debug.Log("Taken Damage");
        Destroy(gameObject);
    }

    public void Heal()
    {
        alphaColor.a -= .1f;
        bloodScreen.color = alphaColor;
    }
}
