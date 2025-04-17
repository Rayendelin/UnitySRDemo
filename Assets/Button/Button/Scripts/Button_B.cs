using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_B : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
//Down用于监听指针被按下的接口，UP用于监听指针释放的接口
{
    [HideInInspector]
    public bool pressed;//按下    
    public void OnPointerDown(PointerEventData eventData)
    //比如 鼠标点击B，按下鼠标的时候，B事件相应此事件
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    //响应此事件的前提是A对象响应过OnPointerDown事件
    //比如 鼠标点击B，释放鼠标的时候，B事件相应此事件
    {
        pressed = false;
    }
}
