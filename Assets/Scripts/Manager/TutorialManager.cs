using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialManager : MonoBehaviour
{
    VideoPlayer videoPlayer;
    [SerializeField]
    SerializableDictionary<int, string> textDic;
    [SerializeField]
    SerializableDictionary<int, VideoClip> videoDic;

    TextMeshProUGUI text;

    Button pageUpBtn;
    Button pageDownBtn;

    int currentIndex = 0;
    const int lowIndex = 0;
    const int highIndex = 2;

    // Start is called before the first frame update
    void Start()
    {
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        videoPlayer = transform.Find("Video Player").GetComponent<VideoPlayer>();
        videoPlayer.Play();
        pageUpBtn = transform.Find("PageUp").GetComponent<Button>();
        pageDownBtn = transform.Find("PageDown").GetComponent<Button>();
        pageUpBtn.interactable = false;
        UpdateTextAndVideo();
    }

    public void PageUp()
    {
        if (--currentIndex <= lowIndex)
        {
            currentIndex = lowIndex;
            pageUpBtn.interactable = false;
        }
        pageDownBtn.interactable = true;
        UpdateTextAndVideo();
    }

    public void PageDown()
    {
        if (++currentIndex >= highIndex)
        {
            currentIndex = highIndex;
            pageDownBtn.interactable = false;
        }
        pageUpBtn.interactable= true;
        UpdateTextAndVideo();
    }
    
    void UpdateTextAndVideo()
    {
        text.text = textDic[currentIndex];
        videoPlayer.clip = videoDic[currentIndex];
    }

}
