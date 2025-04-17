using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpView : ViewBase
{
    public GameObject HpItemPrefab;
    public Damageable damageable;
    private Toggle[] hps;

    private void Start()
    {
        hps = new Toggle[damageable.maxHP];

        for (int i = 0; i < damageable.maxHP; i++)
        {
            GameObject hpItem = GameObject.Instantiate(HpItemPrefab, transform.Find("Hps"));
            hps[i] = hpItem.GetComponent<Toggle>();
        }
    }

    public void UpdateHPView()
    {
        for (int i = 0; i < hps.Length; i++)
        {
            if (hps[i].isOn && i >= damageable.CurrentHp)
            {
                hps[i].transform.Find("Blood/Dissolve").gameObject.SetActive(false);
                hps[i].transform.Find("Blood/Dissolve").gameObject.SetActive(true);
            }
            
            hps[i].isOn = i < damageable.CurrentHp;
        }
    }
}
