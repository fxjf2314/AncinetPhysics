using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    [SerializeField]
    Button voiceBtn;
    [SerializeField] Slider voiceSlider;
    void Start()
    {
        if (VoiceSliderAndButtonManager.Instance != null)
        {
            VoiceSliderAndButtonManager.Instance.InitOtherSliderAndBtn(voiceSlider, voiceBtn);
        }
    }

    
}
