using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_B : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
//Down���ڼ���ָ�뱻���µĽӿڣ�UP���ڼ���ָ���ͷŵĽӿ�
{
    [HideInInspector]
    public bool pressed;//����    
    public void OnPointerDown(PointerEventData eventData)
    //���� �����B����������ʱ��B�¼���Ӧ���¼�
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    //��Ӧ���¼���ǰ����A������Ӧ��OnPointerDown�¼�
    //���� �����B���ͷ�����ʱ��B�¼���Ӧ���¼�
    {
        pressed = false;
    }
}
