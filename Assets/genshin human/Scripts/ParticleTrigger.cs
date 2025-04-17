using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ParticleEffectOnButton : MonoBehaviour
{
    public ParticleSystem particleEffect;
    public Button triggerButton;
    public PlayerControl03 PlayerControl03;

    void Start()
    {
        triggerButton.onClick.AddListener(PlayParticleEffect);
        
    }

    void PlayParticleEffect()
    {
        bool isCanAttack = PlayerControl03.isCanControl;
        if (!isCanAttack) {return; }

        particleEffect.Play();
    }
}