using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_D : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
//Down���ڼ���ָ�뱻���µĽӿڣ�UP���ڼ���ָ���ͷŵĽӿ�
{
    [HideInInspector]
    public bool pressed;//����    
    public void OnPointerDown(PointerEventData eventData)
    //���� �����D����������ʱ��D�¼���Ӧ���¼�
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    //��Ӧ���¼���ǰ����A������Ӧ��OnPointerDown�¼�
    //���� �����D���ͷ�����ʱ��D�¼���Ӧ���¼�
    {
        pressed = false;
    }
}
