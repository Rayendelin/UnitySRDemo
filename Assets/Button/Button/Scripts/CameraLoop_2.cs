using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLoop_2 : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    protected Joystick Joystick;
    protected Button_A Joybutton;
    protected Button_B Joybutton2;
    //protected Button_C Joybutton3;
    //protected Button_D Joybutton4;
    protected bool canJump;

    public float speed = 1f;
    public float rotateSpeed = 1f;
    public GameObject TheCamera;
    public GameObject CameraControl;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Joystick = FindObjectOfType<Joystick>();
        Joybutton = FindObjectOfType<Button_A>();
        Joybutton2 = FindObjectOfType<Button_B>();
        //Joybutton3 = FindObjectOfType<Button_C>();
        //Joybutton4 = FindObjectOfType<Button_D>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        Vector3 moveDir = Vector3.zero;
        Vector3 _camForward = Vector3.Scale(TheCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveDir_vertical = _camForward * Joystick.Vertical;
        Vector3 moveDir_horizontal = TheCamera.transform.right * Joystick.Horizontal;

        moveDir = moveDir_horizontal + moveDir_vertical;
        moveDir.Normalize();

        //Vector3 direction = transform.forward * Joystick.Vertical + transform.right * Joystick.Horizontal;
        base.transform.position += moveDir * 8f * speed * Time.fixedDeltaTime;
        if (moveDir == Vector3.zero)
        {
            moveDir = transform.forward;
            float newspeed = Mathf.Lerp(anim.GetFloat("vertical"), 10, Time.deltaTime * 5);
            anim.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
        }
        else 
        {
            float newspeed = Mathf.Lerp(anim.GetFloat("vertical"), 3f, Time.deltaTime * 5);
            anim.SetFloat("vertical", newspeed,0.005f,Time.deltaTime);
        }

        Quaternion qua_moveDir = Quaternion.LookRotation(moveDir);
        Quaternion wantToRot = Quaternion.Slerp(transform.rotation, qua_moveDir, rotateSpeed * Time.deltaTime);
        transform.rotation = wantToRot;

        //if (Joybutton3.pressed)
        //{
        //    CameraControl.transform.Rotate(0, -50f * rotateSpeed * Time.deltaTime, 0, Space.Self);
        //}
        //if (Joybutton4.pressed)
        //{
        //    CameraControl.transform.Rotate(0, 50f * rotateSpeed * Time.deltaTime, 0, Space.Self);
        //}


    }
    public void attackA()
    {
        anim.SetTrigger("attackA");
    }
    public void attackB()
    {
        anim.SetTrigger("attackB");
    }
}