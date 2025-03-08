using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceSliderAndButtonManager : MonoBehaviour
{
    [Header("-----����������-----")]
    [SerializeField] Slider Master;
    [SerializeField] Slider Music;
    [SerializeField] Slider Sound;

    //SldOnClick�����ݲ����Դ���AudioManager��SldOnClick
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

    //BtnOnClick�����ݲ����Դ���AudioManager��BtnOnClick
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
