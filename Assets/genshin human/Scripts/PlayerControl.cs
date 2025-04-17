using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    public float speed = 0.75f;
    public float rotationSpeed = 100f;
    public VariableJoystick variableJoystick;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    protected Button_A Joybutton;
    protected Button_B Joybutton2;
    public bool DownState = false;//鼠标按下


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
        m_Animator.SetTrigger("Jump");
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
