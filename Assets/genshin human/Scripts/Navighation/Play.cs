using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Play : MonoBehaviour
{
    public Transform[] waypoints; // ���Ѳ����
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
        // ����Ѿ����ﵱǰѲ���㣬��ѡ����һ��Ѳ����
        if (navMeshAgent.remainingDistance < 0.1f && !navMeshAgent.pathPending)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            SetDestination(waypoints[currentWaypointIndex]);
        }
    }

    void SetDestination(Transform target)
    {
        // ���� NavMeshAgent ��Ŀ��Ϊ��ǰѲ����
        if (target != null)
        {
            navMeshAgent.SetDestination(target.position);
        }
    }
}
