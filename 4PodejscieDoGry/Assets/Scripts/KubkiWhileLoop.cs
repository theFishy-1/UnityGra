using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KubkiWhileLoop : MonoBehaviour
{
    int cups = 4;

    void Start()
    {
        while (cups > 2)
        {
            Debug.Log("umylem kubek");
            cups--;
            print("pozostaly " + cups + " kubki");      
        }

        if (cups == 2)
        {
            Debug.Log("umylem kubek");
            cups--;
            print("pozostal " + cups + " kubek");

        }

        if (cups == 1)
        {
            Debug.Log("umylem kubek");
            cups--;
            print("pozostalo " + cups + " kubkow");

        }
    }
}
