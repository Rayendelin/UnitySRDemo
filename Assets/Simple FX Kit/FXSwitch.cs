using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXSwitch : MonoBehaviour
{
    public ParticleSystem particleSystem0;
    public ParticleSystem particleSystem1;
    public ParticleSystem particleSystem2;
    public ParticleSystem particleSystem3;
    public ParticleSystem particleSystem4;
    public int FX0;
    public int FX1;
    public int FX2;
    public int FX3;
    public int FX4;

     void Awake()
    {
       particleSystem0.enableEmission = false;
       particleSystem1.enableEmission = false;
       particleSystem2.enableEmission = false;
       particleSystem3.enableEmission = false; // 在开始时暂停粒子系统
       particleSystem4.enableEmission = false;
       FX0 = 1;
       FX1 = 1;
       FX2 = 1;
       FX3 = 1;
       FX4 = 1;
    }

    void OnTriggerEnter(Collider other)
    {
        string otherObjectName = other.name;
        if (otherObjectName == "Collider")
        {
            Debug.Log("trigger enter" + other.transform.name);
            FX0 += 1;
        }
        if (otherObjectName == "Collider1")
        {
            Debug.Log("trigger enter" + other.transform.name);
            FX1 += 1;
        }
        if (otherObjectName == "Collider2")
        {
            Debug.Log("trigger enter" + other.transform.name);
            FX2 += 1;
        }
        if (otherObjectName == "Collider3")
        {
            Debug.Log("trigger enter" + other.transform.name);
            FX3 += 1;
        }
        if (otherObjectName == "Collider4")
        {
            Debug.Log("trigger enter" + other.transform.name);
            FX4 += 1;
        }
        
    }
    void Update() 
    {
        if (FX0 % 2 == 0)
        {
            particleSystem0.enableEmission = true;
        }
        else
        {
            particleSystem0.enableEmission = false;
        }

        if (FX1 % 2 == 0)
        {
            particleSystem1.enableEmission = true;
        }
        else
        {
            particleSystem1.enableEmission = false;
        }

        if (FX2 % 2 == 0)
        {
            particleSystem2.enableEmission = true;
        }
        else
        {
            particleSystem2.enableEmission = false;
        }

        if (FX3 % 2 == 0)
        {
            particleSystem3.enableEmission = true;
        }
        else
        {
            particleSystem3.enableEmission = false;
        }

        if (FX4 % 2 == 0)
        {
            particleSystem4.enableEmission = true;
        }
        else
        {
            particleSystem4.enableEmission = false;
        }
    }
}
