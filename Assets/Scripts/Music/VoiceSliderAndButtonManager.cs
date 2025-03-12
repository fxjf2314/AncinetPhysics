using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceSliderAndButtonManager : MonoBehaviour
{
    [Header("-----音量设置条-----")]
    [SerializeField] Slider Master;
    [SerializeField] Slider Music;
    [SerializeField] Slider Sound;

    //SldOnClick：传递参数以触发AudioManager的SldOnClick
    public void MasterSldOnClick(Image image)
    {
        AudioMixerManager.Instance.MasterSldOnClick(image, Master);
    }
    public void MusicSldOnClick(GameObject image)
    {
        AudioMixerManager.Instance.MusicSldOnClick(image, Music);
    }
    public void SoundSldOnClick(GameObject image)
    {
        AudioMixerManager.Instance.SoundSldOnClick(image, Sound);
    }

    //BtnOnClick：传递参数以触发AudioManager的BtnOnClick
    public void MasterBtnOnClick(Image image)
    {
        AudioMixerManager.Instance.MasterBtnOnClick(image, Master);
    }
    public void SoundBtnOnClick(GameObject image)
    {
        AudioMixerManager.Instance.SoundBtnOnClick(image, Sound);
    }
    public void MusicBtnOnClick(GameObject image)
    {
        AudioMixerManager.Instance.MusicBtnOnClick(image, Music);
    }
}
