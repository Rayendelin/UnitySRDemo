using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprint : MonoBehaviour
{
    public float ClickInterval = 0.5f;//点击间隔
    public float DownTime = 0;//鼠标按下计时
    public bool DownState = false;//鼠标按下
    public int DownCount;//鼠标按下次数
    public float HoldActivateTime = 0.5f;//长按生效时间
    public float HoldTime = 0;//长按计时
    public GameObject Player;

    Animator m_Animator;
    Rigidbody m_Rigidbody;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void 设置鼠标按下(bool Down) 
    {
        DownState = Down;
        if (DownState)
        {
            if(DownTime <= ClickInterval)
            {
                DownCount++;
                //Debug.Log("单击");
            }
            else
            {
                DownCount = 0;
            }
            DownTime = 0;
            HoldTime = 0;
        }
        else 
        {
            if(HoldTime >= HoldActivateTime)
            {
                print("长按完成");
            }
            switch (DownCount)
            {
                case 0:
                    print("单击");
                    Player.transform.Translate(0, 0, 1.5f);
                    m_Animator.SetTrigger("Dash");
                    break;
            }
        }
    }
    void Update()
    {
        DownTime += Time.deltaTime;
        if (DownState)
        {
            HoldTime += Time.deltaTime;
            if (HoldTime >= HoldActivateTime)
            {
                print("长按生效");
                m_Animator.SetBool("FastRun",true);
            }
        }
    }
}
