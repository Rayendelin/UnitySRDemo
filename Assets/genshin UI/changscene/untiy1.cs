using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class untiy1 : MonoBehaviour
{
    public Button button1;
    void Start()
    {
        button1.onClick.AddListener(SwitchScene);
    }
    void SwitchScene()
    {
        //方法一，通过索引值切换场景
        //SceneManager.LoadScene(1);
        //方法二，通过场景名字切换场景
        SceneManager.LoadScene("Suntail Village");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
