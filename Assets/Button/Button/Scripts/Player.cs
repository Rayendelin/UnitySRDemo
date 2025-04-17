using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float Blend;
    private Animator ani;
    private void Start()
    {
        ani = this.GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Blend = Mathf.Lerp(Blend, 0.5f, 0.01f);
        }
        if (!Input.GetKey(KeyCode.W))
        {
            Blend = Mathf.Lerp(Blend, 0, 0.01f);
        }
        ani.SetFloat("Blend", Blend);
    }
}
