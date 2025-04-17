using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using System;


public class DialogueBox : MonoBehaviour
{
    public Text nameText;
    public Text contentText;
    public float typingSpeed = 0.1f;
    private string currentContent;
    private Coroutine typingCoroutine;
    public PlayableDirector director;
    protected bool m_AlreadyTriggered;
    public GameObject[]  activation;
    public GameObject[] destroy;
    public GameObject player;

#region 对话

    public void Say1()
    {
        ShowDialogue("奥德尔", "对话模式测试...........");
    }

    public void Say2()
    {
        ShowDialogue("诺艾尔", "对话模式测试.................");
    }

    public void Say3()
    {
        ShowDialogue("奥德尔", "对话模式测试............................................................");
    }

    public void Say4()
    {
        ShowDialogue("奥德尔", "对话模式测试................................");
    }
    public void Say5()
    {
        ShowDialogue("诺艾尔", "谢谢提醒...");
    }


    public void ShowDialogue(string name, string content)
    {
        nameText.text = name;
        currentContent = content;
        contentText.text = "";
 
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
 
        typingCoroutine = StartCoroutine(TypeContent());
    }
 
    IEnumerator TypeContent()
    {
        foreach (char c in currentContent)
        {
            contentText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

#endregion

    private void OnTriggerEnter(Collider other)
    {
        if (m_AlreadyTriggered) {return; }
        director.Play();
        m_AlreadyTriggered = true;
        PlayerControl03 PlayerControl03 = other.GetComponent<PlayerControl03>();
        Animator m_Animator = other.GetComponent<Animator>();
        m_Animator.SetBool("isWalk", false);
        PlayerControl03.enabled = false;
    }

    public void DialogueBoxEnd()
    {
        Debug.Log("结束对话");
        foreach (var objActivation in activation)
        {
            objActivation.SetActive(true);
        }
        foreach (var objDestroy in destroy)
        {
            objDestroy.SetActive(false);
        }
        PlayerControl03 PlayerControl03 = player.GetComponent<PlayerControl03>();
        PlayerControl03.enabled = true;
    }


}
