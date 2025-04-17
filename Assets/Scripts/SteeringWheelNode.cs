using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SteeringWheelNode : MonoBehaviour
{
    [SerializeField]
    RectTransform m_rotateImageRT;//轮盘跟着手指方向的图
    [SerializeField]
    RectTransform m_fixedBgImageRT;//轮盘背景图
    [SerializeField]
    Transform m_CameraTr;   //相机，用于获取朝向
    [SerializeField]
    Model m_Model;//控制动画播放
    private bool m_isDraging = false;
    private Vector2 m_uiPosInScreen;
    private float m_curRotateAngle = 0;

    private int m_fingerIdId;
    // Start is called before the first frame update
    void Start()
    {
        //获取ui的屏幕坐标
        RectTransform rt = this.gameObject.GetComponent<RectTransform>();
        m_uiPosInScreen = rt.position;
        EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry_touchDown = new EventTrigger.Entry();
        entry_touchDown.eventID = EventTriggerType.PointerDown;
        entry_touchDown.callback.AddListener(onTouchDown);
        EventTrigger.Entry entry_touchUp = new EventTrigger.Entry();
        entry_touchUp.eventID = EventTriggerType.PointerUp;
        entry_touchUp.callback.AddListener(onTouchUp);
        EventTrigger.Entry entry_touchDrag = new EventTrigger.Entry();
        entry_touchDrag.eventID = EventTriggerType.Drag;
        entry_touchDrag.callback.AddListener(onTouchDrag);

        eventTrigger.triggers.Add(entry_touchDown);
        eventTrigger.triggers.Add(entry_touchUp);
        eventTrigger.triggers.Add(entry_touchDrag);
    }
    void Update()
    {
        if(m_isDraging)
        {
            m_rotateImageRT.rotation = Quaternion.Euler(0,0, m_curRotateAngle);
            //m_Model.PlayRunAnimation();

            //根据相机朝向与轮盘角度计算角色的移动方向
            Vector3 camForward = m_CameraTr.forward;
            Vector3 moveForward = Quaternion.AngleAxis(-m_curRotateAngle, Vector3.up) * camForward;
            moveForward.y = 0;
            Vector3 modelOffsetPos = m_Model.Move(moveForward.normalized, 0.1f);
            Vector3 camCurPos = m_CameraTr.position;
            m_CameraTr.position = camCurPos + modelOffsetPos;
        }
        //else
        //{
        //    m_Model.PlayStandAnimation();
        //}
    }

    private void onTouchDown(BaseEventData data)
    {
        Debug.Log("touch down");
        //移动平台需要确定触摸轮盘的手指id
        float minDis = float.MaxValue;
        if (Utils.IsIphonePlatform())
        {
            foreach (Touch touch in Input.touches)
            {
                if((touch.position - m_uiPosInScreen).sqrMagnitude < minDis)
                {
                    m_fingerIdId = touch.fingerId;
                }
            }
        }

        m_isDraging = true;
        this.UpdateDirection();
    }
    private void onTouchUp(BaseEventData data)
    {
        Debug.Log("touch up");

        this.UpdateDirection();

        m_rotateImageRT.localScale = new Vector3(0.5f, 0.5f, 1.0f);
        m_fixedBgImageRT.localScale = new Vector3(0.5f, 0.5f, 1.0f);
        m_rotateImageRT.rotation = Quaternion.Euler(0, 0, 0);
        m_isDraging = false;
    }
    private void onTouchDrag(BaseEventData data)
    {
        this.UpdateDirection();
    }

    private void UpdateDirection()
    {
        Vector3 curPos = Input.mousePosition;
        if (Utils.IsIphonePlatform())
        {
            foreach (Touch touch in Input.touches)
            {
                if(m_fingerIdId == touch.fingerId)
                {
                    curPos = touch.position;
                    break;
                }
            }
        }

        //得到相对坐标
        Vector2 relativePos = new Vector2(curPos.x - m_uiPosInScreen.x, curPos.y - m_uiPosInScreen.y);
        //计算距离中心位置
        float dis2Center_pingfang = relativePos.sqrMagnitude;
        if (dis2Center_pingfang <= 5625)
        {
            m_rotateImageRT.localScale = new Vector3(0.5f, 0.5f, 1.0f);
            m_fixedBgImageRT.localScale = new Vector3(0.5f, 0.5f, 1.0f);
        }
        else
        {
            m_rotateImageRT.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            m_fixedBgImageRT.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        //计算旋转值
        float angle = Vector2.Angle(relativePos, new Vector2(0, 1));
        if(relativePos.x > 0)
        {
            angle = 360 - angle;
        }
        m_curRotateAngle = angle;
    }
    
}
