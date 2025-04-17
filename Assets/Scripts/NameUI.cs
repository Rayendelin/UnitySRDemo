using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NameUI : MonoBehaviour
{

    public Transform tras;
    public Transform cam;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = tras.transform.position;
        this.transform.rotation = cam.transform.rotation;
    }
}
