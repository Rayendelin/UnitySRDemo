using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public AnimationCurve showCurve;//存储显示动画曲线
    public AnimationCurve hideCurve;//存储隐藏动画曲线
    public float animationSpeed;//动画速度
    public GameObject panel;//面板对象
    public GameObject windows;

    IEnumerator showPanel(GameObject gameObject)
    {
        float timer = 0;//初始化计时器
        while (timer <= 1)
        {
            gameObject.transform.localScale = Vector3.one * showCurve.Evaluate(timer);
            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }
        //yield return null;
        Time.timeScale = 0;
    }

    IEnumerator HidePanel(GameObject gameObject)
    {
        float timer = 0;//初始化计时器
        while (timer <= 1)
        {
            gameObject.transform.localScale = Vector3.one * hideCurve.Evaluate(timer);
            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }
        yield return null;
        panel.SetActive(false);
        windows.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(showPanel(panel));
        }
    }

    public void Hide()
    {
        Time.timeScale = 1;
        StartCoroutine(HidePanel(panel));
    }
}
