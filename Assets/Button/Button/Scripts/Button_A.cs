using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_A : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
//Down���ڼ���ָ�뱻���µĽӿڣ�UP���ڼ���ָ���ͷŵĽӿ�
{
    [HideInInspector]
    public bool pressed;//����    
    public void OnPointerDown(PointerEventData eventData)
    //���� �����A����������ʱ��A�¼���Ӧ���¼�
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    //��Ӧ���¼���ǰ����A������Ӧ��OnPointerDown�¼�
    //���� �����A���ͷ�����ʱ��A�¼���Ӧ���¼�
    {
        pressed = false;
    }
}
