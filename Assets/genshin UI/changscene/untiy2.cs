using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class untiy2 : MonoBehaviour
{
    public Button button1;
    void Start()
    {
        button1.onClick.AddListener(SwitchScene);
    }
    void SwitchScene()
    {
        //����һ��ͨ������ֵ�л�����
        //SceneManager.LoadScene(1);
        //��������ͨ�����������л�����
        SceneManager.LoadScene("Suntail Village Navigation");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
