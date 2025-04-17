using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackMaskView : MonoBehaviour
{
    public static BlackMaskView Instance;
    private Image image;

    private void Awake()
    {
        if (Instance != null)
        {
            throw new System.Exception("场景中有多个BlackMask");
        }
        Instance = this;
    }

    private void Start()
    {
        image = transform.GetComponent<Image>();
    }

    public IEnumerator FadeIn()//透明从1到0
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        while (image.color.a > 0)
        {
            yield return null;
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - Time.deltaTime);
        }
    }

    public IEnumerator FadeOut()//透明从0到1
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        while (image.color.a < 1)
        {
            yield return null;
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
