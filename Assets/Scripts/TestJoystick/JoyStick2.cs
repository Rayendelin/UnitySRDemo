using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStick2 : MonoBehaviour
{
    public Transform center;
    public Transform mouse;

    public int radius = 150;

    //public string msg;

    public Vector3 mousePos;
    private Vector3 mouseRec;

    private Touch touch;

    public void EndDragEvent()//在小圆的身上绑定的event trigger
    {
        mouse.localPosition = Vector3.zero;
        center.localPosition = Vector3.zero;
        //msg = "";
    }

    /// <summary>
    /// 在小圆的身上绑定的event trigger
    /// 这里必须是PointerDown的事件 ，不能是beginDrag事件
    /// beginDrag的事件要晚于这个事件，会检测不到touchphase.began
    /// </summary>
    public void PointerDownEvent()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    touch = Input.GetTouch(i);
                    //这里也必须给mouserec赋值，否则按下的第一帧会跳动
                    mouseRec = touch.position;
                }
            }
        }
        else
        {
            //这里也必须给mouserec赋值，否则按下的第一帧会跳动
            mouseRec = Input.mousePosition;
            mouse.position = mouseRec;
        }
    }

    public void DragEvent()//在小圆的身上绑定的event trigger
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            DragOnAndroid();
        }
        else
        {
            DragOnWindows();
        }
    }

    private void DragOnAndroid()
    {
        //msg = touch.fingerId + "   " + touch.phase;
        //安卓的逻辑和电脑端一样，无非是将鼠标点换成了触摸点
        mousePos = touch.position;
        mouse.position = mousePos;

        Vector3 offset = mousePos - mouseRec;

        //if (offset.magnitude > 100)
        //{
        //    msg += "  " + offset.magnitude + "  " + mousePos + "   " + mouseRec + "   " + center.position;
        //}

        if (center.localPosition.magnitude >= radius - 1)
        {
            center.localPosition = mouse.localPosition.normalized * radius;

            offset *= (center.localPosition.magnitude / mouse.localPosition.magnitude);

            if (mouse.localPosition.magnitude < radius)
            {
                center.position += offset;
            }
        }
        else
        {
            center.position += offset;
        }

        mouseRec = mousePos;
        //根据fingerID寻找移动的触摸点
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (touch.fingerId == Input.GetTouch(i).fingerId)
            {
                touch = Input.GetTouch(i);
            }
        }
    }

    private void DragOnWindows()
    {
        //获取鼠标位置
        mousePos = Input.mousePosition;
        mouse.position = mousePos;
        //获取两帧之间的偏移量
        Vector3 offset = mousePos - mouseRec;

        //这里半径-1，是因为如果不-1，会出现明明等于半径，却执行else里面的代码块
        if (center.localPosition.magnitude >= radius - 1)
        {
            //摇杆小圆的向量等于中心点到鼠标的向量的模乘以半径
            center.localPosition = mouse.localPosition.normalized * radius;
            //当鼠标位置处于摇杆之外，就要将偏移量等比例缩小一下
            offset *= (center.localPosition.magnitude / mouse.localPosition.magnitude);
            //如果鼠标的位置处于半径之内，这是前面的if块无法跳出，就要在这里对向量处理，这样就能够跳出
            if (mouse.localPosition.magnitude < radius)
                center.position += offset;
        }
        else
        {
            center.position += offset;
        }
        //记录一下鼠标位置
        mouseRec = mousePos;
    }
}
