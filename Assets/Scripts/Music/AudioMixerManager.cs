using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    public static AudioMixerManager Instance => instance;
    private static AudioMixerManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] 
    AudioMixer audioMixer;

    [SerializeField]
    public Sprite openVoice,closeVoice;

    //改变音调
    const float pitchMin = 0.9f;
    const float pitchMax = 1.1f;

    //用于判断静音与否
    private bool IsMuteMaster = false;
    private bool IsMuteMusic = false;
    private bool IsMuteSound = false;
    //用于储存静音前的音量
    private float LastMaster;
    private float LastMusic;
    private float LastSound;

    //音量设置
    //Slider on click：调节音量，若静音则取消静音
    public void MasterSldOnClick(Image image, Slider slider)
    {
        audioMixer.SetFloat("vMaster", slider.value);
        if (slider.value == slider.minValue)
        {
            image.sprite = closeVoice;
        }
        else
        {
            image.sprite = openVoice;
        }
        if (IsMuteMaster == false) return;
        else
        {
            image.sprite = openVoice;
            IsMuteMaster = false;
        }
    }
    public void MusicSldOnClick(GameObject image, Slider slider)
    {
        audioMixer.SetFloat("vMusic", slider.value);
        if (IsMuteMusic == false) return;
        else
        {
            image.SetActive(false);
            IsMuteMusic = false;
        }
    }
    public void SoundSldOnClick(GameObject image, Slider slider)
    {
        audioMixer.SetFloat("vSound", slider.value);
        if (IsMuteSound == false) return;
        else
        {
            image.SetActive(false);
            IsMuteSound = false;
        };
    }

    //Button on click：若静音则取消静音并回调音量；若未静音则静音并储存音量
    public void MasterBtnOnClick(Image image, Slider Master)
    {
        if (IsMuteMaster)
        {
            image.sprite = openVoice;
            IsMuteMaster = false;
            Master.value = LastMaster;
        }

        else
        {
            image.sprite = closeVoice;
            LastMaster = Master.value;
            Master.value = Master.minValue;
            IsMuteMaster = true;
        }
    }
    public void SoundBtnOnClick(GameObject image, Slider Sound)
    {
        if (IsMuteSound)
        {
            image.SetActive(false);
            IsMuteSound = false;
            Sound.value = Instance.LastSound;
        }

        else
        {
            image.SetActive(true);
            LastSound = Sound.value;
            Sound.value = Sound.minValue;
            IsMuteSound = true;
        }
    }
    public void MusicBtnOnClick(GameObject image, Slider Music)
    {
        if (IsMuteMusic)
        {
            image.SetActive(false);
            IsMuteMusic = false;
            Music.value = LastMusic;
        }

        else
        {
            image.SetActive(true);
            LastMusic = Music.value;
            Music.value = Music.minValue;
            IsMuteMusic = true;
        }
    }


    [SerializeField] AudioSource SoundPlayer;
    //播放音效
    public void PlaySound(AudioClip audioClip)
    {
        SoundPlayer.pitch = 1;
        SoundPlayer.PlayOneShot(audioClip);
    }

    // 改变音调，主要用于重复播放的音效
    public void PlayRandomSound(AudioClip audioClip)
    {
        SoundPlayer.pitch = Random.Range(pitchMin, pitchMax);
        SoundPlayer.PlayOneShot(audioClip);
    }

    public void PlayRandomSound(AudioClip[] audioClip)
    {
        PlayRandomSound(audioClip[Random.Range(0, audioClip.Length)]);
    }

    public float GetAudioVolume(string trackName)
    {
        float volume = 0;
        audioMixer.GetFloat(trackName,out volume);
        return volume;
    }

}
