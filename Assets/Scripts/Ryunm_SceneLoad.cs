using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ryunm_SceneLoad : MonoBehaviour
{
    public Image image;
    public float speed = 1;
    public Slider ProgressBar;
    public Text ProgressBarText;
    public GameObject LoadText;
    // Start is called before the first frame update
    void Start()
    {
        ProgressBar.gameObject.SetActive(false);
        ProgressBarText.gameObject.SetActive(false);
        LoadText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick_Btn_LoadSceneAsync(string sceneName)
    {
        StartCoroutine(SceneLoadAsync(sceneName));

    }

    IEnumerator SceneLoadAsync(string sceneName)
    {
        yield return null;
        Color tempColor = image.color;
        tempColor.a = 0;
        image.color = tempColor;
        while (image.color.a < 1)
        {
            image.color += new Color(0, 0, 0, 1);
            //yield return null;
        }
        
        ProgressBar.gameObject.SetActive(true);
        ProgressBarText.gameObject.SetActive(true);
        LoadText.gameObject.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            ProgressBar.value = asyncOperation.progress;
            ProgressBarText.text = asyncOperation.progress * 100 + "%";
            yield return null;
            if (asyncOperation.progress >= 0.9f)
            {
                ProgressBarText.text = asyncOperation.progress * 100 + "%";
                yield return null;
                asyncOperation.allowSceneActivation = true;
            }
        }

    }
}
