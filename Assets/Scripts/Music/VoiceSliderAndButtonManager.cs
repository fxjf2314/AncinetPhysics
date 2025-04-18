using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VoiceSliderAndButtonManager : MonoBehaviour
{
    public static VoiceSliderAndButtonManager Instance => instance;
    static VoiceSliderAndButtonManager instance;

    [Header("-----音量设置条-----")]
    [SerializeField] Slider Master;
    [SerializeField] Slider Music;
    [SerializeField] Slider Sound;
    [SerializeField] Button Btn;

    private void Start()
    {
        instance = this;
        if(AudioMixerManager.Instance != null && Master!=null)
        {
            Debug.Log(AudioMixerManager.Instance.GetAudioVolume("vMaster"));
            Master.maxValue = AudioMixerManager.Instance.GetAudioVolume("vMaster");
            Master.minValue = -80;
        }
        
    }

    public void InitOtherSlider(Slider slider, Image image)
    {
        slider.maxValue = Master.maxValue;
        slider.minValue = Master.minValue;
        slider.onValueChanged = Master.onValueChanged;
    }

    public void InitOtherBtn(Button button)
    {
        button.onClick = Btn.onClick;
    }

    public void InitOtherSliderAndBtn(Slider slider, Button button)
    {
        Debug.Log(button.name);
        slider.maxValue = Master.maxValue;
        slider.minValue = Master.minValue;
        slider.value = Master.value;
        button.GetComponent<Image>().sprite = Btn.GetComponent<Image>().sprite;
        slider.onValueChanged.AddListener((float num) =>
        {
            AudioMixerManager.Instance.MasterSldOnClick(button.GetComponent<Image>(), slider);
            Master.value = slider.value;
            Btn.GetComponent<Image>().sprite = button.GetComponent<Image>().sprite;
        });
        button.onClick.AddListener(() =>
        {
            AudioMixerManager.Instance.MasterBtnOnClick(button.GetComponent<Image>(), slider);
            Master.value = slider.value;
            Btn.GetComponent<Image>().sprite = button.GetComponent<Image>().sprite;
        });
        //slider.onValueChanged = Master.onValueChanged;
        //slider.onValueChanged.AddListener((float num) =>
        //{
        //    button.GetComponent<Image>().sprite = Btn.GetComponent<Image>().sprite;
        //    Master.value = slider.value;
        //});
        //button.onClick = Btn.onClick;
        //button.onClick.AddListener(() =>
        //{
        //    button.GetComponent<Image>().sprite = Btn.GetComponent<Image>().sprite;
        //    slider.value = Master.value;
        //});
    }

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
