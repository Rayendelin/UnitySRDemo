using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CheckPoint
{
    public Transform point;
    public float radius;
}

public class WeaponAttackController : MonoBehaviour
{
    #region 字段
    public CheckPoint[] checkPoint;
    public Color color;
    private RaycastHit[] radius = new RaycastHit[10];
    public LayerMask layerMask;
    public bool Attack = false;
    public int damage;
    public GameObject mySelf;
    private List<GameObject> attackList = new List<GameObject>();

    #endregion


    #region 生命周期
    void Update()
    {
        CheckGameObject();
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < checkPoint.Length; i++)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(checkPoint[i].point.position, checkPoint[i].radius);
        }
    }

    #endregion


    #region 方法
    public void BeginAttack()
    {
        Attack = true;
        //Debug.Log("开始攻击");
    }

    public void EndAttack()
    {
        Attack = false;
        attackList.Clear();
        //Debug.Log("结束攻击");
    }

    public void CheckGameObject()//监测敌人进行攻击
    {
        if (!Attack) {return; }

        for (int i = 0; i < checkPoint.Length; i++)
        {
            int count = Physics.SphereCastNonAlloc(checkPoint[i].point.position, checkPoint[i].radius, Vector3.forward, radius, 0, layerMask.value);
            for (int j = 0; j < count; j++)
            {
                //Debug.Log("监测到敌人，进行攻击：" + radius[j].transform.name);
                CheckDamage(radius[j].transform.gameObject);
            }
        }
    }

    public void CheckDamage(GameObject obj)//对敌人造成伤害
    {
        //判断敌人是否有受伤的功能
        Damageable damageable = obj.GetComponent<Damageable>();
        if (damageable == null) {return; }

        //监测到自己
        if (obj == mySelf) { return; }

        if (attackList.Contains(obj)) {return; }

        //进行攻击
        DamageMessage data = new DamageMessage();
        data.damage = damage;
        damageable.OnDamage(data);
        attackList.Add(obj);
    }

    #endregion
    
}
