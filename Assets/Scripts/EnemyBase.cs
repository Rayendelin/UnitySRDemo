using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Damageable))]
public class EnemyBase : MonoBehaviour
{
    #region 字段
    public float checkDistance;//监测距离
    public float maxHeightDiff;//可攻击的最大高度差
    [Range(0, 180)]
    public float lookAngle;//视野范围
    RaycastHit[] results = new RaycastHit[10];//最多监测10个
    public float followDistance;//追踪距离
    public float attackDistance;//攻击距离
    public LayerMask layerMask;//监测层级
    public GameObject Target;
    protected UnityEngine.AI.NavMeshAgent meshAgent;
    protected Vector3 startPosition;
    public float runSpeed = 2;
    public float walkSpeed = 1;
    protected float moveSpeed = 0;
    protected Animator animator;
    protected Rigidbody mRigidbody;
    protected bool isCanAttack = true;
    public float attackTime;//攻击时间间隔
    private float attackTimer;//攻击时间计时

    #endregion


    #region 生命周期
    protected virtual void Start()
    {
        meshAgent = transform.GetComponent<UnityEngine.AI.NavMeshAgent> ();
        startPosition = transform.position;
        animator = transform.GetComponent<Animator> ();
        mRigidbody = transform.GetComponent<Rigidbody> ();
    }

    protected virtual void Update()
    {
        CheckTarget();
        FollowTarget();
        if (!isCanAttack) 
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackTime)
            {
                isCanAttack = true;
                attackTimer = 0;
            }
        }
    }

    private void OnAnimatorMove()
    {
        mRigidbody.MovePosition(transform.position + animator.deltaPosition);
    }
    protected virtual void OnDrawGizmosSelected()//绘制
    {
        //画出监测范围
        Gizmos.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.4f);//绘画颜色
        Gizmos.DrawSphere(transform.position, checkDistance);//绘画监测范围形状
        //画出追踪距离
        Gizmos.color = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.4f);//绘画颜色
        Gizmos.DrawSphere(transform.position, followDistance);//绘画监测范围形状
        //画出攻击距离
        Gizmos.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.4f);//绘画颜色
        Gizmos.DrawSphere(transform.position, attackDistance);//绘画监测范围形状
        //画出高度差
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * maxHeightDiff);
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * maxHeightDiff);
        // //画出视野范围
        // UnityEditor.Handles.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.4f);
        // UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, lookAngle, checkDistance);
        // UnityEditor.Handles.DrawSolidArc(transform.position, -Vector3.up, transform.forward, lookAngle, checkDistance);
    }

    #endregion


    #region 方法
    
    public virtual void CheckTarget()//监测目标
    {
        int count = Physics.SphereCastNonAlloc(transform.position, checkDistance, Vector3.forward, results, 0, layerMask.value);

        for (int i = 0; i < count; i++)  
        {
            //判断是否是可攻击物体
            if (results[i].transform.GetComponent<Damageable>() == null) {continue; }

            //判断高度差
            if (Mathf.Abs(results[i].transform.position.y - transform.position.y) > maxHeightDiff) {continue;}

            //判断是否在视野范围内
            if (Vector3.Angle(transform.forward,results[i].transform.position - transform.position) > lookAngle) {continue;}

            //判断目标是否是存活状态
            if (!results[i].transform.GetComponent<Damageable>().IsAlive) {continue; }

            //找到离自己最近的目标进行攻击
            if (Target != null)
            {
                //攻击目标不为空时判断距离
                float distance = Vector3.Distance(transform.position, Target.transform.position);//判断当前目标距离
                float currentDistance = Vector3.Distance(transform.position,results[i].transform.position);//判断新目标距离
                if (currentDistance < distance)
                {
                    Target = results[i].transform.gameObject;//如果新的目标距离小于当前目标的距离那么就更新新的目标为追击目标
                }
            }
            else
            {
                Target = results[i].transform.gameObject;
            }
            

        }
    }

    public virtual void MoveToTarget()//向目标移动
    {
        if (Target != null)
        {
            if (transform.GetComponent<Damageable>().IsAlive)
            {
                meshAgent.SetDestination(Target.transform.position);
            }
        }
    }

    public virtual void FollowTarget()//追踪目标
    {
        ListenerSpeed();//跟踪目标时需要监听速度
        if (Target != null)
        {
            try
            {
                //向目标进行移动
                MoveToTarget();

                //判断目标是否有效，是否能够到达
                if (meshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial || meshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
                {
                    //目标丢失
                    LoseTarget();
                    return;
                }

                //是否在可追踪距离内
                if (Vector3.Distance(transform.position, Target.transform.position) > followDistance)
                {
                    //目标距离大于可追踪距离后目标丢失
                    LoseTarget();
                    return;
                } 

                if (!Target.transform.GetComponent<Damageable>().IsAlive)//判断目标是否为存活状态
                {
                    LoseTarget();//丢失目标
                    return; 
                }

                //判断是不是在攻击范围
                if (Vector3.Distance(transform.position, Target.transform.position) <= attackDistance)
                {
                    //目标距离小于可攻击距离后进行攻击
                    //Debug.Log("进行攻击");
                    if (isCanAttack)
                    {
                        Attack();
                        isCanAttack = false;
                    }
                } 
            }
            catch(Exception e)
            {  
                //追踪出错，目标丢失
                LoseTarget();
            }
        }
    }

    public virtual void LoseTarget()//目标丢失的方法
    {
        Target = null;
        if (transform.GetComponent<Damageable>().IsAlive)
        {
            //怪物回到初始位置
            meshAgent.SetDestination(startPosition);
            moveSpeed = walkSpeed;
        }
    }
    public virtual void ListenerSpeed()//监听速度
    {
        if (Target != null)
        {
            moveSpeed = runSpeed;
        }
        meshAgent.speed = moveSpeed;
        animator.SetFloat("Speed", meshAgent.velocity.magnitude);
    }
    public virtual void Attack()//攻击方法
    {}

    public virtual void OnDeath(Damageable damageable, DamageMessage data)
    {}


    #endregion

}
