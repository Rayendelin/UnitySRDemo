using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWaitAnimationTrigger : MonoBehaviour
{
    public Animator animator; // 指定Animator组件
    public string waitAnimationStateName; // 等待动画状态的名称
    public float waitAnimationLength; // 等待动画的总长度
 
    void Start()
    {
        animator = this.GetComponent<Animator>();
        // 在开始时随机触发等待动画
        Invoke("TriggerRandomWaitAnimation", Random.Range(0f, 0f)); // 在0到0秒之间随机时间触发
    }
 
    void TriggerRandomWaitAnimation()
    {
        // 计算随机的起始帧
        float randomStartTime = Random.Range(0f, waitAnimationLength);
 
        // 播放动画并设置为随机的起始帧
        animator.Play(waitAnimationStateName, 0, randomStartTime / waitAnimationLength);
    }
}
