using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_C : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
//Down���ڼ���ָ�뱻���µĽӿڣ�UP���ڼ���ָ���ͷŵĽӿ�
{
    [HideInInspector]
    public bool pressed;//����    
    public void OnPointerDown(PointerEventData eventData)
    //���� �����C����������ʱ��C�¼���Ӧ���¼�
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    //��Ӧ���¼���ǰ����A������Ӧ��OnPointerDown�¼�
    //���� �����C���ͷ�����ʱ��C�¼���Ӧ���¼�
    {
        pressed = false;
    }
}
