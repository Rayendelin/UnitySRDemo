using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerControl02 : MonoBehaviour
{
    public float speed = 0.75f;
    public float rotationSpeed = 100f;
    public VariableJoystick variableJoystick;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    protected Button_A Joybutton;
    protected Button_B Joybutton2;
    public bool DownState = false;//鼠标按下
    public float initYSpeed = 10;//初始跳跃时的竖直上抛速度
    private float nowYSpeed;//跳跃当中实时的速度
    public float G = 9.8f;//重力加速度
    private float nowPlatformY = 10f;//当前平台的Y
    public int jumpIndxex;
    public bool isAttack;


    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        Joybutton = FindObjectOfType<Button_A>();
        Joybutton2 = FindObjectOfType<Button_B>();
    }


    void FixedUpdate()
    {
        GetComponentInChildren<Animator>().SetFloat("speed", speed);
        float horizontal = Input.GetAxis("Horizontal");   //定义一个横轴的浮点数
        float vertical = Input.GetAxis("Vertical");  //定义一个竖轴的浮点数

        horizontal = horizontal == 0 ? variableJoystick.Horizontal : horizontal;
        vertical = vertical == 0 ? variableJoystick.Vertical : vertical;

        //判断行走状态
        if(!(horizontal == 0 && vertical == 0))
        {
            m_Animator.SetBool("isWalk", true);
        }
        else
        {
            m_Animator.SetBool("isWalk", false);
        }


        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        Vector3 moveDirection = (cameraForward * vertical + Camera.main.transform.right * horizontal).normalized;
        transform.Translate(moveDirection * speed * Time.deltaTime * 5f, Space.World);

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime * 200f);
        }
    }

    public void attackA()
    {
        m_Animator.SetTrigger("Attack");
    }
    public void Jump()
    {
        if(jumpIndxex !=1 )
        {
            m_Animator.SetBool("isJump", true);
            nowYSpeed = initYSpeed;  //当前Y速度等于上抛的初始速度
            ++jumpIndxex;
        }
    }

    void Update()
    {
        //跳跃时Y的变化
        if (m_Animator.GetBool("isJump"))
        {
            this.transform.Translate(Vector3.up * nowYSpeed * Time.deltaTime);  //Y的初始速度
            nowYSpeed -= G * Time.deltaTime;  //重力加速度
            speed = 0.3f;

            m_Animator.SetBool("isFall", nowYSpeed <= 0);  //当竖直方向的速度小于等于0时就播放下落动画

            if (this.transform.position.y <= nowPlatformY)
            {
                m_Animator.SetBool("isJump", false);  //停止跳跃动画
                m_Animator.SetBool("isFall", false);  //停止播放下落动画
                Vector3 pos = this.transform.position;
                pos.y = nowPlatformY;
                this.transform.position = pos;
                jumpIndxex = 0;
            }
        }

        AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
	    if (stateInfo.IsName("attack"))
	    {
	    	//若播放的我需要的特定动画，设置状态为true
            isAttack = true;
	        speed = 0.3f;
	    }
        if (stateInfo.IsName("Idle"))
	    {
	        speed = 0.75f;
	    }
    }


    public void FastRun(bool Down)
    {
        DownState = Down;
        if (DownState)
        {
            Debug.Log("长按");
            speed = 1.5f;
            GetComponentInChildren<Animator>().SetFloat("speed", speed);
        }
        else
        {
            speed = 0.75f;
            Debug.Log("长按结束");
        }
    }
}
