using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Play : MonoBehaviour
{
    public Transform[] waypoints; // 多个巡航点
    private int currentWaypointIndex = 0;

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            SetDestination(waypoints[currentWaypointIndex]);
        }
    }

    void Update()
    {
        // 如果已经到达当前巡航点，则选择下一个巡航点
        if (navMeshAgent.remainingDistance < 0.1f && !navMeshAgent.pathPending)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            SetDestination(waypoints[currentWaypointIndex]);
        }
    }

    void SetDestination(Transform target)
    {
        // 设置 NavMeshAgent 的目标为当前巡航点
        if (target != null)
        {
            navMeshAgent.SetDestination(target.position);
        }
    }
}
