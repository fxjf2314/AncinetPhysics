using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DisasterOccurs : MonoBehaviour
{
    private GameObject area;
    public EarthQuake earthQuake;
    public Flood flood;
    public DustStorm dustStorm;
    public Fog fog;
    public GameObject disasterEffect;
    private float duration;
    public Sprite[] images;
    public Transform canvas;
    public TextMeshProUGUI tmpText;
    public string[] EarthQuakeForcast;
    public string[] FloodForcast;
    public string[] DustStormForcast;
    public string[] FogForcast;
    private Dictionary<string, string[]> DisasterForcast;
    public GameObject button;
    public Image black;
    public float fadeDuration;
    public float targetAlpha;
    private float fadeTimer = 0f;
    private bool canFading=false;
    private bool isFadingIn;
    public GameObject wait;
    public GameObject tip;

    private void Start()
    {
        DisasterForcast = new Dictionary<string, string[]>
        {
            { "EarthQuake", EarthQuakeForcast },
            { "Flood", FloodForcast },
            { "DustStorm", DustStormForcast },
            { "Fog", FogForcast }
        };
        StartCoroutine(RoundStart());
    }
    private void Update()
    {
        fadeTimer += Time.deltaTime;
        float alpha;
        if (canFading)
        {
            if (isFadingIn)
            {
                alpha = Mathf.Lerp(0f, targetAlpha, fadeTimer / fadeDuration);
            }
            else
            {
                alpha = Mathf.Lerp(targetAlpha, 0f, fadeTimer / fadeDuration);
            }

            Color color = black.color;
            color.a = alpha;
            black.color = color;
        }
    }
    public void NextRound()
    {
        if (UIManager.MyInstance.totalRound<=9)
        {
            StartCoroutine(RoundStart());
        }
        
    }
    public IEnumerator RoundStart()
    {
        button.SetActive(false);
        tmpText.gameObject.SetActive(false);
        yield return null;
        while (wait.activeSelf)
        {
            yield return null;
        }
        fadeTimer = 0f;
        canFading = false;
        DisasterManager.canOccur = true;
        DisasterManager.nextDisaster = null;
        if (DisasterManager.thisDisaster != null)
        {
            tip.GetComponent<TextMeshProUGUI>().text = DisasterManager.thisDisaster.title;
            tip.SetActive(false);
            tip.SetActive(true);
            images = DisasterManager.thisDisaster.images;
            AchievementControl.Instance.disastertime++;
            StartCoroutine(PlayImageSequence());
            if(area == null)
            {
                area = transform.GetChild(3).gameObject;
            }
            DisasterManager.thisDisaster.Use(area);
                
            if (area.GetComponent<AreaScript>().cards.Contains(Cards.Instance.cards[7]) && Achievements.Instance.achievements[11].finish == false&&AchievementControl.Instance.test)
            {
                    AchievementControl.Instance.if11happen=true;
            }//11

            if (Achievements.Instance.achievements[16].finish == false && AchievementControl.Instance.test)
            {
                switch (DisasterManager.thisDisaster.title)
                {
                    case "…≥≥æ±©":
                        if(!(area.GetComponent<AreaScript>().cards.Contains(Cards.Instance.cards[7])|| area.GetComponent<AreaScript>().cards.Contains(Cards.Instance.cards[15])))
                        {
                            AchievementControl.Instance.if16happen = false;
                        }
                        break;
                    case "µÿ’":
                        bool ifdidongyi=false;
                        foreach(AreaScript area in HandCard.MyInstance.allarea)
                        {
                            if (area.cards.Contains(Cards.Instance.cards[2]))
                            {
                                ifdidongyi = true;
                                break;
                            }
                        }
                        if (!(area.GetComponent<AreaScript>().cards.Contains(Cards.Instance.cards[7]) || ifdidongyi))
                        {
                            AchievementControl.Instance.if16happen = false;
                        }
                        break;
                    case "∫È¿‘":
                        if (!(area.GetComponent<AreaScript>().cards.Contains(Cards.Instance.cards[7]) || area.GetComponent<AreaScript>().cards.Contains(Cards.Instance.cards[3])))
                        {
                            AchievementControl.Instance.if16happen = false;
                        }
                        break;
                    case "¥ÛŒÌ":
                        if (!(area.GetComponent<AreaScript>().cards.Contains(Cards.Instance.cards[7]) || area.GetComponent<AreaScript>().cards.Contains(Cards.Instance.cards[15])))
                        {
                            AchievementControl.Instance.if16happen = false;
                        }
                        break;
                }               
            }//16

        }
        DisasterManager.thisDisaster = null;
        earthQuake.Judge();
        flood.Judge();
        dustStorm.Judge();
        fog.Judge();
        while (EventVisualization.Instance.isEffecting)
        {
            yield return null;
        }
        if (DisasterManager.nextDisaster != null)
        {
            RandomArea();
            Inform();
            DisasterManager.thisDisaster = DisasterManager.nextDisaster;
        }
    }
    private  void RandomArea()
    {
        int childCount = transform.childCount;
        if (childCount > 0)
        {
            int randomIndex = Random.Range(0, childCount);
            area = transform.GetChild(randomIndex).gameObject;
        }
    }
    IEnumerator PlayImageSequence()
    {
        canFading = true;
        isFadingIn = true;
        EventVisualization.Instance.isEffecting = true;
        for (int i = 0; i < images.Length; i++)
        {
            GameObject theImage = Instantiate(disasterEffect, canvas);
            theImage.GetComponent<Image>().sprite = images[i];
            duration = theImage.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
            Destroy(theImage, duration);
            yield return new WaitForSeconds(duration / 3);
        }
        fadeTimer = 0f;
        isFadingIn = false;
        EventVisualization.Instance.isEffecting = false;
    }
    public void Inform()
    {
        button.SetActive(true);
        button.GetComponent<Image>().enabled = true;
        Disaster nextDisaster = DisasterManager.nextDisaster;
        if (nextDisaster != null)
        {
            string[] disasterForcasts = DisasterForcast[nextDisaster.name];
            int index = Random.Range(0, disasterForcasts.Length);
            string disasterForcast = disasterForcasts[index];
            tmpText.text = area.GetComponent<AreaScript>().areaDetail.areaName + disasterForcast;
        }
    }
}
