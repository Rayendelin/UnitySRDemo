using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IK : MonoBehaviour
{
    private Animator theAnimator;
    private Vector3 leftFootIk, rightFootIk;//射线检测需要的 IK 位置
    private Vector3 leftFootPosition, rightFootPosition;//IK 位置赋值
    private Quaternion leftFootRotation, rightFootRotation;//IK 旋转赋值

    #region 射线检测相关
    [SerializeField] private LayerMask iKLayer;//射线可以检测到的层
    [SerializeField] [Range(0, 0.2f)] private float rayHitOffset;//射线检测位置与 IK 位置的偏移
    [SerializeField] private float rayCastDistance;//射线检测距离
    private RaycastHit hitInfo;
    #endregion


    [SerializeField] private bool enableIK = true;//是否启用 IK
    [SerializeField] private float iKSphereRadius = 0.05f;
    [SerializeField] private float positionSphereRadius = 0.05f;

    public void Awake()
    {
        theAnimator = this.gameObject.GetComponent<Animator>();
        leftFootIk = theAnimator.GetIKPosition(AvatarIKGoal.LeftFoot);//获取 IK 位置
        rightFootIk = theAnimator.GetIKPosition(AvatarIKGoal.RightFoot);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        leftFootIk = theAnimator.GetIKPosition(AvatarIKGoal.LeftFoot);
        rightFootIk = theAnimator.GetIKPosition(AvatarIKGoal.RightFoot);

        if (!enableIK)
        {
            return;
        }

        #region 设置 IK 权重
        theAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, theAnimator.GetFloat("LIK"));//设置IK权重
        theAnimator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, theAnimator.GetFloat("LIK"));
        theAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, theAnimator.GetFloat("RIK"));
        theAnimator.SetIKRotationWeight(AvatarIKGoal.RightFoot, theAnimator.GetFloat("RIK"));
        #endregion

        #region 设置 IK 位置和旋转值
        theAnimator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPosition);//设置脚步位置
        theAnimator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);//设置旋转值
        theAnimator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPosition);
        theAnimator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);
        #endregion
        }

    private void FixedUpdate()
    {
        Debug.DrawLine(leftFootIk + (Vector3.up * 0.5f), leftFootIk + Vector3.down * rayCastDistance, Color.blue, Time.deltaTime);
        Debug.DrawLine(rightFootIk + (Vector3.up * 0.5f), rightFootIk + Vector3.down * rayCastDistance, Color.blue, Time.deltaTime);

        #region 获得旋转值和位置
        if (Physics.Raycast(leftFootIk + (Vector3.up * 0.5f), Vector3.down, out RaycastHit hit, rayCastDistance + 1, iKLayer))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.red, Time.deltaTime);
            leftFootPosition = hit.point + Vector3.up * rayHitOffset;// 如果让脚的位置等于碰撞点，那么可能会出现穿模的情况。
            leftFootRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;
        }

        if (Physics.Raycast(rightFootIk + (Vector3.up * 0.5f), Vector3.down, out RaycastHit hit_01, rayCastDistance + 1, iKLayer))
        {
            Debug.DrawRay(hit_01.point, hit_01.normal, Color.red, Time.deltaTime);
            rightFootPosition = hit_01.point + Vector3.up * rayHitOffset;
            rightFootRotation = Quaternion.FromToRotation(Vector3.up, hit_01.normal) * transform.rotation;
        }
        #endregion

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(leftFootIk, iKSphereRadius);
            Gizmos.DrawSphere(rightFootIk, iKSphereRadius);
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(leftFootPosition, positionSphereRadius);
            Gizmos.DrawSphere(rightFootPosition, positionSphereRadius);
        }
    }
}
