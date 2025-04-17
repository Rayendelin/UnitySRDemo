using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEditor;

public class PlayerControl03 : MonoBehaviour
{
    public float speed = 0.5f;
    public float rotationSpeed = 100f;
    public VariableJoystick variableJoystick;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    protected Button_A Joybutton;
    protected Button_B Joybutton2;
    public bool DownState = false;//鼠标按下
    public float Gravity = -9.8f;//重力加速度
    public Vector3 Velocity = Vector3.zero;
    public bool isAttack;
    private CharacterController controller;
    public float CheckRadius = 0.1f;
    public bool isGround;
    public bool isStairs;
    public LayerMask groundLayer;//地面碰撞筛选器
    public LayerMask stairsLayer;//阶梯碰撞筛选器
    public float walkSpeed = 0.5f; //设置走路速率
    public float runSpeed = 1.5f; //设置跑步速率
    public float stairsSpeed = 0.17f; //设置阶梯速度
    public float JumpHeight = 1.5f;
    public ParticleSystem particleEffect;
    public GameObject ParticleObject;
    public Button triggerButton;
    public GameObject weapon;
    public bool isCanControl = true;
    protected AnimatorStateInfo currentStateInfo;    // Information about the base layer of the animator cached.
    private int DeathHash = Animator.StringToHash("Death");
    private int RespawnHash = Animator.StringToHash("Respawn");
    public Vector3 respawnPosition;//玩家重置的位置
    public GameObject Body02;

    public void GainControl()//获得控制权
    {
        isCanControl = true;
    }

    public void ReleaseControl()//失去控制权
    {
        isCanControl = false;
    }


    private void Awake()
    {
        var pipelineAsset = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;
        if (pipelineAsset != null)
        {
            // 关闭UseSRPBatcher
            pipelineAsset.useSRPBatcher = true;
            //Debug.Log("SRP Batcher enabled.");
        }
    }
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        Joybutton = FindObjectOfType<Button_A>();
        Joybutton2 = FindObjectOfType<Button_B>();
        controller = transform.GetComponent<CharacterController>();
        // Application.targetFrameRate = 61;
        triggerButton.onClick.AddListener(PlayParticleEffect02);
    }

    public void OnHurt(Damageable damageable, DamageMessage data)
    {
        m_Animator.SetTrigger("Hurt");
    }

    public void OnDeath(Damageable damageable, DamageMessage data)
    {
        m_Animator.SetTrigger("Death");
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn()//重生
    {
        //判断是不是死亡动画正在播放
        while (currentStateInfo.shortNameHash != DeathHash)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2);

        //屏幕要变黑
        yield return StartCoroutine(BlackMaskView.Instance.FadeOut());

        //重置玩家位置
        transform.position = respawnPosition;

        //播放重生的动画
        m_Animator.SetTrigger("Respawn");

        //屏幕变亮
        Body02.gameObject.SetActive(true);
        yield return StartCoroutine(BlackMaskView.Instance.FadeIn());

        //重置血量
        transform.GetComponent<Damageable>().ResetDamage();

        //给玩家控制权
        yield return new WaitForSeconds(5);
        Body02.gameObject.SetActive(false);
        GainControl();
        
    }

    public void SetRespawnPosition(Vector3 position)//设置出生位置
    {
        respawnPosition = position;
    }

    public void attackA()
    {
        if (!isCanControl) {return; }

        m_Animator.SetTrigger("Attack");
    }

    void PlayParticleEffect02()
    {
        if (!isCanControl) {return; }
        StartCoroutine(PlayParticleEffect03());
    }

    IEnumerator PlayParticleEffect03()
    {
        yield return new WaitForSeconds(1);//延迟一秒

        Vector3 PlayerForward = this.transform.forward;
        Vector3 direction = this.transform.TransformDirection(Vector3.forward);
        ParticleObject.transform.forward = PlayerForward;
        ParticleObject.transform.position = this.transform.position + direction * 0.5f;
        particleEffect.Play();
    }

    private void FixedUpdate()
    {
        currentStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (m_Animator.GetBool("isJump"))
        {
            speed = walkSpeed;
        }
        
        isGround = Physics.CheckSphere(this.transform.position, CheckRadius, groundLayer);
        isStairs = Physics.CheckSphere(this.transform.position, CheckRadius, stairsLayer);
        if(isGround && Velocity.y <0)
        {
            m_Animator.SetBool("isFall", false);
            m_Animator.SetBool("isJump", false);
            Velocity.y = 0;
        } 

        GetComponentInChildren<Animator>().SetFloat("speed", speed);
        float horizontal = Input.GetAxis("Horizontal");   //定义一个横轴的浮点数
        float vertical = Input.GetAxis("Vertical");  //定义一个竖轴的浮点数

        horizontal = horizontal == 0 ? variableJoystick.Horizontal : horizontal;
        vertical = vertical == 0 ? variableJoystick.Vertical : vertical;

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        Vector3 moveDirection = (cameraForward * vertical + Camera.main.transform.right * horizontal).normalized;

        if (moveDirection != Vector3.zero)
        {
            if (!isCanControl) {return; }


            Quaternion toRotation = Quaternion.LookRotation(moveDirection.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime * 200f);
        }

        //判断行走状态
        if(!(horizontal == 0 && vertical == 0))
        {
            if(isGround)
            {
                m_Animator.SetBool("isWalk", true);
            }
            if(isStairs)
            {
                m_Animator.SetBool("isStairs", true);
                m_Animator.SetBool("isWalk", false);
                speed = stairsSpeed;
            }
        }
        else
        {
            if(isGround)
            {
                m_Animator.SetBool("isWalk", false);
            }
            if(isStairs)
            {
                m_Animator.SetBool("isStairs", false);
            }
            
        }

        AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
	    if (stateInfo.IsName("attack"))
	    {
	    	//若播放的我需要的特定动画，设置状态为true
            isAttack = true;
	        speed = walkSpeed;
	    }
        if (stateInfo.IsName("Idle"))
	    {
	        speed = walkSpeed;
	    }

        if (Velocity.y < 0)
        {
            m_Animator.SetBool("isFall", true);
        }
    }


    public void FastRun(bool Down)
    {
        DownState = Down;
        if (DownState)
        {
            Debug.Log("长按");
            speed = runSpeed;
            GetComponentInChildren<Animator>().SetFloat("speed", speed);
        }
        else
        {
            speed = walkSpeed;
            Debug.Log("长按结束");
        }
    }

    public void Jump()
    {
        if(isGround && isCanControl)
        {
            m_Animator.SetBool("isJump", true);
            Velocity.y += Mathf.Sqrt(JumpHeight * -2 * Gravity);
        }
    }

    private void OnAnimatorMove()
    {
        if (!isCanControl) { return; }

        GetComponentInChildren<Animator>().SetFloat("speed", speed);
        float horizontal = Input.GetAxis("Horizontal");   //定义一个横轴的浮点数
        float vertical = Input.GetAxis("Vertical");  //定义一个竖轴的浮点数

        horizontal = horizontal == 0 ? variableJoystick.Horizontal : horizontal;
        vertical = vertical == 0 ? variableJoystick.Vertical : vertical;

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        Vector3 moveDirection = (cameraForward * vertical + Camera.main.transform.right * horizontal).normalized;
        controller.Move(moveDirection * speed * 0.1f);

        Velocity.y += Gravity * Time.deltaTime;
        controller.Move(Velocity * Time.deltaTime);
    }

    public void MeleeAttackStart()
    {
        weapon.GetComponent<WeaponAttackController>().BeginAttack();
    }

    public void MeleeAttackEnd()
    {
        weapon.GetComponent<WeaponAttackController>().EndAttack();
    }

    public void OnIdleStart()
    {
        m_Animator.SetInteger("RandomIdle", -1);
    }

    public void OnIdleEnd()
    {
        m_Animator.SetInteger("RandomIdle", Random.Range(0, 3));
    }
}
