using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventVisualization : MonoBehaviour
{
    public GameObject prefab;
    public Dictionary<string, Sprite> signs;
    private int[] oPopulation;
    private float[] oFood;
    private float[] oCoin;
    private AreaScript[] areas;
    [SerializeField]
    private Sprite populationSign;
    [SerializeField]
    private Sprite coinSign;
    [SerializeField]
    private Sprite foodSign;
    public GameObject wait;
    public bool isEffecting;
    public static EventVisualization Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void InitVisual()
    {
        signs = new Dictionary<string, Sprite>()
        {
            {"population",populationSign},
            {"food",foodSign },
            {"coin",coinSign }
        };
        areas =new AreaScript[gameObject.transform.childCount];
        oPopulation= new int[gameObject.transform.childCount];
        oFood = new float[gameObject.transform.childCount];
        oCoin = new float[gameObject.transform.childCount];
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            areas[i]=gameObject.transform.GetChild(i).GetComponent<AreaScript>();
            oPopulation[i] = areas[i].areaDetail.population;
            oFood[i] = areas[i].areaDetail.food;
            oCoin[i] = areas[i].areaDetail.coin;
        }
    }
    public void NextRound()
    {
        StartCoroutine(RoundStart());
    }
    IEnumerator RoundStart()
    {
        yield return null;
        while (wait.activeSelf)
        {
            yield return null;
        }
        yield return null;
        while (isEffecting)
        {
            yield return null;
        }
        for (int i = 0; i < areas.Length; i++)
        {
            StartCoroutine(VisualizeEvent(i));
        }
        yield break;
    }
    IEnumerator VisualizeEvent(int i)
    {
        if (areas[i].areaDetail.population != oPopulation[i])
        {
            int Change = areas[i].areaDetail.population - oPopulation[i];
            CreateText(prefab, areas[i].transform, "population", Change, signs);
            yield return new WaitForSeconds(0.2f);
        }
        if (areas[i].areaDetail.food != oFood[i])
        {
            float Change = areas[i].areaDetail.food - oFood[i];
            CreateText(prefab, areas[i].transform, "food", Change, signs);
            yield return new WaitForSeconds(0.2f);
        }
        if (areas[i].areaDetail.population != oPopulation[i])
        {
            float Change = areas[i].areaDetail.coin - oCoin[i];
            CreateText(prefab, areas[i].transform, "coin", Change, signs);
            yield return new WaitForSeconds(0.2f);
        }
        oPopulation[i] = areas[i].areaDetail.population;
        oFood[i] = areas[i].areaDetail.food;
        oCoin[i] = areas[i].areaDetail.coin;
    }
    private void CreateText(GameObject prefab, Transform area, string type, float change, Dictionary<string, Sprite> signs)
    {
        GameObject textObject = Instantiate(prefab,area.position, prefab.transform.rotation);
        TextMeshPro textMeshPro = textObject.GetComponent<TextMeshPro>();
        if (change < 0)
        {
            textMeshPro.color = new Color(0.6f,0f,0f);
            textMeshPro.text =Convert.ToInt16(change).ToString();
        }
        else
        {
            textMeshPro.color = new Color(0f, 0.5f, 0f);
            textMeshPro.text = "+" + Convert.ToInt16(change);
        }
        Transform sign = textMeshPro.transform.GetChild(0);
        sign.GetComponent<SpriteRenderer>().sprite=signs[type];
        //sign.localPosition = new Vector3(4, 3, 0);
        textMeshPro.SetAllDirty();
        textMeshPro.ForceMeshUpdate();
        textMeshPro.ForceMeshUpdate();
    }
}
