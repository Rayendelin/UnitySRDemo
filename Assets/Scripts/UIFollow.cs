using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    public Transform FollowObject;//跟随对象
    public Transform Cam;//相机朝向
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position=FollowObject.position;
        this.transform.rotation=Cam.rotation;
    }
}
