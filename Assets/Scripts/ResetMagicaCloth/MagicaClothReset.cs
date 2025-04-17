using UnityEngine;
using MagicaCloth2; // 引用插件的命名空间

[System.Serializable]
public class MagicaClothReset : MonoBehaviour
{
    [Header("MagicaCloth2 Components")]
    public MagicaCloth[] clothComponents; // 使用实际的类名 MagicaCloth

    [Header("Reset Settings")]
    public bool resetOnStart = false;

    void Start()
    {
        if (resetOnStart)
            ResetAllCloths();
    }

    public void ResetAllCloths()
    {
        if (clothComponents == null || clothComponents.Length == 0)
        {
            Debug.LogWarning("没有布料!");
            return;
        }

        foreach (var cloth in clothComponents)
        {
            if (cloth != null)
            {
                cloth.ResetCloth(); // 调用官方提供的重置方法
                Debug.Log($"Reset cloth: {cloth.name}", cloth);
            }
        }
    }
}