using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    Renderer[] renderers;

    public float dissolveTime  = 3f;
    private float dissolveTimer = 0;

    MaterialPropertyBlock propertyBlock;

    void Start()
    {
        renderers = transform.GetComponentsInChildren<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
        dissolveTimer += Time.deltaTime;
        if (dissolveTimer >= dissolveTime) {return; }
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat("", dissolveTimer / dissolveTime);
            renderers[i].SetPropertyBlock(propertyBlock);
        }
    }
}
