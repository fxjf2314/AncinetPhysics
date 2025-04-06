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
    public GameObject[] war;

    private void Awake()
    {
        foreach (RandomEvent randomEvent in randomEvents)
        {
            for (int i = 0; i < randomEvent.areasProbability.Length; i++)
            {
                randomEvent.areasProbability[i] = new AreaProbability(transform.GetChild(i).gameObject, randomEvent.areasProbability[i].Probability);
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
        if (UIManager.MyInstance.totalRound <= 9)
        {
            StartCoroutine(RoundStart());
        }
    }
    IEnumerator RoundStart()
    {
        RandomEventTips.gameObject.SetActive(false);
        RandomEventTips.text = "";
        RandomEventTipsButton.SetActive(false);
        ButtonsManager.MyInstance.isHappenEvent = false;
        SetAnimation(false);
        yield return null;
        while (wait.activeSelf)
        {
            yield return null;
        }
        if (DisasterManager.thisDisaster)
            yield return new WaitUntil(() => EventVisualization.Instance.isEffecting);
        while (EventVisualization.Instance.isEffecting)
        {
            yield return null;
        }
        foreach (RandomEvent randomEvent in randomEvents)
        {
            randomEvent.Use();
        }
        yield break;
    }
    public void SetAnimation(bool state)
    {
        for (int i = 0; i < war.Length; i++)
        {
            war[i].SetActive(state);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(1).gameObject.SetActive(state);
        }
    }
}
