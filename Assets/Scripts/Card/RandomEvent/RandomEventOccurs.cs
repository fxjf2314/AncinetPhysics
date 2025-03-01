using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomEventOccurs : MonoBehaviour
{
    public GameObject RandomEventTipsButton;
    public TextMeshProUGUI RandomEventTips;
    public RandomEvent[] randomEvents;

    private void Awake()
    {
        foreach (RandomEvent randomEvent in randomEvents)
        {
            for (int i = 0; i < randomEvent.areasProbability.Length; i++)
            {
                randomEvent.areasProbability[i] = new AreaProbability(transform.GetChild(i).gameObject, randomEvent.areasProbability[i].probability);
                randomEvent.randomEventTips = RandomEventTips;
            }
        }
    }
    private void Start()
    {
        RoundStart();
    }
    public void RoundStart()
    {
        RandomEventTipsButton.SetActive(false);
        foreach (RandomEvent randomEvent in randomEvents)
        {
            if(!RandomEventTipsButton.activeSelf)
            RandomEventTipsButton.SetActive(true);
            randomEvent.Use();
        }
    }
}
