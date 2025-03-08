using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomEventOccurs : MonoBehaviour
{
    public GameObject RandomEventTipsButton;
    public TextMeshProUGUI RandomEventTips;
    public RandomEvent[] randomEvents;
    public GameObject wait;
    private bool waitOver;

    private void Awake()
    {
        foreach (RandomEvent randomEvent in randomEvents)
        {
            for (int i = 0; i < randomEvent.areasProbability.Length; i++)
            {
                randomEvent.areasProbability[i] = new AreaProbability(transform.GetChild(i).gameObject, randomEvent.areasProbability[i].probability);
                randomEvent.randomEventTips = RandomEventTips;
                randomEvent.randomEventTipsButton = RandomEventTipsButton;
            }
        }
    }
    private void Start()
    {
        StartCoroutine(RoundStart());
    }
    public void NextRound()
    {
        StartCoroutine(RoundStart());
    }
    IEnumerator RoundStart()
    {
        RandomEventTips.gameObject.SetActive(false);
        RandomEventTips.text = "";
        RandomEventTipsButton.SetActive(false);
        ButtonsManager.MyInstance.isHappenEvent = false;
        yield return null;
        while (wait.activeSelf)
        {
            yield return null;
        }
        foreach (RandomEvent randomEvent in randomEvents)
        {
            randomEvent.Use();
        }
        yield break;
    }
}
