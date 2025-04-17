using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class AngularVelocity : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public float text;
    public GameObject player;
    public TextMeshProUGUI NavMeshAgentPoint;
    public TextMeshProUGUI Text;
    public Vector3 position; // 存储物体的位置信息
    Animator m_Animator;
    public ParticleSystem particleEffect; //特效

    void Start() {
        m_Animator = player.GetComponent<Animator>();
        m_Animator.SetBool("isWalk", true);
        
    }

    void Update()
    {
        navMeshAgent = player.GetComponent<NavMeshAgent>(); //获取挂载自动巡航对象上的代理器组件
        text = navMeshAgent.angularSpeed; // 获取自动巡航角速度的数值
        Text.text = text.ToString();
        position = player.transform.position; // 更新物体的位置信息

        if (position.z - 124 <= 1 && position.z - 124 >= -1)
        {
            if(position.x - 137 <= 1 && position.x - 137 >= -1)
            {
                navMeshAgent.angularSpeed = 720;
                NavMeshAgentPoint.text = 0.ToString("g");
            }
        }
        if (position.z - 131 <= 1 && position.z - 131 >= -1)
        {
            if (position.x - 138 <= 1 && position.x - 138 >= -1)
            {
                navMeshAgent.angularSpeed = 50;
                NavMeshAgentPoint.text = 1.ToString("g");
                particleEffect.Play();
                m_Animator.SetTrigger("Attack");
            }
        }
        if (position.z - 149 <= 1 && position.z - 149 >= -1)
        {
            if (position.x - 140 <= 1 && position.x - 140 >= -1)
            {
                navMeshAgent.angularSpeed = 10;
                NavMeshAgentPoint.text = 2.ToString("g");
            }
        }
        if (position.z - 158 <= 1 && position.z - 158 >= -1)
        {
            if (position.x - 143 <= 1 && position.x - 143 >= -1)
            {
                navMeshAgent.angularSpeed = 30;
                NavMeshAgentPoint.text = 3.ToString("g");
            }
        }
        if (position.z - 166 <= 1 && position.z - 166 >= -1)
        {
            if (position.x - 139 <= 1 && position.x - 139 >= -1)
            {
                navMeshAgent.angularSpeed = 50;
                NavMeshAgentPoint.text = 4.ToString("g");
            }
        }
        if (position.z - 175 <= 1 && position.z - 175 >= -1)
        {
            if (position.x - 139 <= 1 && position.x - 139 >= -1)
            {
                navMeshAgent.angularSpeed = 100;
                NavMeshAgentPoint.text = 5.ToString("g");
            }
        }
        if (position.z - 175 <= 1 && position.z - 175 >= -1)
        {
            if (position.x - 149 <= 1 && position.x - 149 >= -1)
            {
                navMeshAgent.angularSpeed = 50;
                NavMeshAgentPoint.text = 6.ToString("g");
            }
        }
        if (position.z - 161 <= 1 && position.z - 161 >= -1)
        {
            if (position.x - 149 <= 1 && position.x - 149 >= -1)
            {
                navMeshAgent.angularSpeed = 600;
                NavMeshAgentPoint.text = 7.ToString("g");
            }
        }
        if (position.z - 158 <= 1 && position.z - 158 >= -1)
        {
            if (position.x - 159 <= 1 && position.x - 159 >= -1)
            {
                navMeshAgent.angularSpeed = 40;
                NavMeshAgentPoint.text = 8.ToString("g");
            }
        }
        if (position.z - 112 <= 1 && position.z - 112 >= -1)
        {
            if (position.x - 157 <= 1 && position.x - 157 >= -1)
            {
                navMeshAgent.angularSpeed = 500;
                NavMeshAgentPoint.text = 9.ToString("g");
            }
        }
        if (position.z - 112 <= 1 && position.z - 112 >= -1)
        {
            if (position.x - 138 <= 1 && position.x - 138 >= -1)
            {
                navMeshAgent.angularSpeed = 30;
                NavMeshAgentPoint.text = 10.ToString("g");
            }
        }
        if (position.z - 119 <= 1 && position.z - 119 >= -1)
        {
            if (position.x - 137 <= 1 && position.x - 137 >= -1)
            {
                navMeshAgent.angularSpeed = 300;
                NavMeshAgentPoint.text = 11.ToString("g");
            }
        }
    }
}
