using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraConter : MonoBehaviour
{
    public Slider slider;
    public float text;
    public GameObject manicamera;
    public GameObject player;

    void Update()
    {
        text = slider.value; // 获取Slider的数值
        //Debug.Log("Slider的数值为：" + text);
        slider.transform.Find("ValueText").gameObject.GetComponent<Text>().text = text.ToString();

        manicamera.transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * text * 200f);
    }

    public void StopNavigation()
    {
        //navMeshAgent = this.GetComponent<NavMeshAgent>(); //获取挂在该脚本的对象上的代理器组件
    }
}
