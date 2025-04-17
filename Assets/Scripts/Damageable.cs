using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageMessage
{
    public int damage;//伤害
}

[Serializable]

public class DamageEvent : UnityEvent<Damageable, DamageMessage> {}

public partial class Damageable : MonoBehaviour
{

    #region 字段
    public int maxHP;//最大血量
    public int hp;//当前血量

    public float invincibleTime = 0;//无敌时间
    private bool isInvincible = false;//是否处于无敌状态
    private float invincibleTimer = 0;//无敌状态计时

    public DamageEvent onHurt;
    public DamageEvent onDeath;
    public DamageEvent onReset;
    public DamageEvent onInvincibleTimeOut;
    public int CurrentHp{
        get{
            return hp;
        }
    }

    public bool IsAlive{
        get{
            return CurrentHp > 0;//判断是否存活
        }
    }

    #endregion


    #region 生命周期
    private void Start()
    {
        hp = maxHP;
    }
    private void Update()
    {
        if (isInvincible)
        {
            invincibleTimer += Time.deltaTime;
            if (invincibleTimer >= invincibleTime)//无敌时间计时到达时间
            {
                isInvincible = false;
                invincibleTimer = 0;
                onInvincibleTimeOut?.Invoke(this, null);
            }
        }
    }
    #endregion


    #region 方法
    public void OnDamage(DamageMessage data)
    {

        if (hp <= 0) {return;}

        if (isInvincible) {return;}//无敌状态

        hp -= data.damage;
        isInvincible = true;

        if (hp <= 0)
        {
            //死亡了
            onDeath?.Invoke(this, data);
        }
        else
        {
            //受伤了
            onHurt?.Invoke(this, data);
        }
    }

    public void ResetDamage()//重置数据复活
    {
        hp = maxHP;
        isInvincible = false;
        invincibleTimer = 0;
        onReset?.Invoke(this, null);
    }
    #endregion
}