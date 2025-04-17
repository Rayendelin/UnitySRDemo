using UnityEngine;
using UnityEngine.EventSystems;


public class TouchOnUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool Test = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        CamControl.Instance.isFingerTouchOnUI = true;
        Test = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CamControl.Instance.isFingerTouchOnUI = false;
        Test = false;
    }
}
