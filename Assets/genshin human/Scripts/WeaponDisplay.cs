using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    public string animationStateName; // 动画状态的名称

    public GameObject weaponModel;
 
    void Start()
    {
        animator = GetComponent<Animator>();
        // 默认隐藏模型
        weaponModel.SetActive(false);
    }
 
    void Update()
    {
        // 检查动画状态是否正在播放
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName))
        {
            // 如果动画状态正在播放，显示模型
            weaponModel.SetActive(true);
        }
        else
        {
            // 如果动画状态没有在播放，隐藏模型
            weaponModel.SetActive(false);
        }
    }
}
