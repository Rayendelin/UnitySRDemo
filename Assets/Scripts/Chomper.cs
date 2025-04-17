using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chomper : EnemyBase
{
    public WeaponAttackController weapon;
    public GameObject Player;
    public override void Attack()
    {
        base.Attack();
        animator.SetTrigger("Attack");
        ChangeDirection();
    }

    public void ChangeDirection()//修改攻击方向
    {
        if (Target != null)
        {
            Vector3 direction = Target.transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void AttackBegin()
    {
        weapon.BeginAttack();
    }

    public void AttackEnd()
    {
        weapon.EndAttack();
    }

    public override void OnDeath(Damageable damageable, DamageMessage data)
    {
        base.OnDeath(damageable, data);
        //丢失目标
        LoseTarget();

        //停止追踪
        meshAgent.isStopped = true;
        meshAgent.enabled = false;

        //播放死亡动画
        animator.SetTrigger("Death");

        //添加一个力飞出去
        Vector3 force = transform.position - Player.transform.position;
        force.y = 0;
        mRigidbody.isKinematic = false;

        mRigidbody.AddForce(force.normalized * 6 + Vector3.up * 1, ForceMode.Impulse);

        //3秒后销毁
        Invoke("Dissove", 1);
    }

    public void Dissove()
    {
        transform.Find("Dissove").gameObject.SetActive(true);
        Destroy(gameObject, 2);
    }
}
