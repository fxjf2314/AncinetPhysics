using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfitVisualization : MonoBehaviour
{
    public GameObject prefab;
    public Dictionary<string, TMP_SpriteAsset> signs;
    private int[] oPopulation;
    private float[] oFood;
    private float[] oCoin;
    private AreaScript[] areas;
    [SerializeField]
    private TMP_SpriteAsset populationSign;
    [SerializeField]
    private TMP_SpriteAsset coinSign;
    [SerializeField]
    private TMP_SpriteAsset foodSign;
    public GameObject wait;
    
    public static ProfitVisualization Instance;

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
        signs = new Dictionary<string, TMP_SpriteAsset>()
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
    public void GetAreaData()
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
        for (int i = 0; i < areas.Length; i++)
        {
            if (areas[i].areaDetail.population != oPopulation[i])
            {
                int Change = areas[i].areaDetail.population - oPopulation[i];
                //VisualizeEvent(areas[i].transform, "population", Change);
            }
            if (areas[i].areaDetail.food != oFood[i])
            {
                float Change = areas[i].areaDetail.food - oFood[i];
                //VisualizeEvent(areas[i].transform, "food", Change);
            }
            if (areas[i].areaDetail.population != oPopulation[i])
            {
                float Change = areas[i].areaDetail.coin - oCoin[i];
                //VisualizeEvent(areas[i].transform, "coin", Change);
            }
            oPopulation[i] = areas[i].areaDetail.population;
            oFood[i] = areas[i].areaDetail.food;
            oCoin[i] = areas[i].areaDetail.coin;
        }
        yield break;
    }
    /*private void VisualizeEvent(Transform area, string type, float change)
    {
        CreateText(prefab, area, type, change, signs);
    }
    private void CreateText(GameObject prefab, Transform area, string type, float change, Dictionary<string, TMP_SpriteAsset> signs)
    {
        GameObject textObject = Instantiate(prefab);
        textObject.transform.position = area.position;
        TextMeshPro textMeshPro = textObject.GetComponent<TextMeshPro>();
        if (change < 0)
        {
            textMeshPro.color = Color.red;
            textMeshPro.text = "<sprite=0>" + change;
        }
        else
        {
            textMeshPro.color = Color.green;
            textMeshPro.text = "<sprite=0>+" + change; 
        }
        textMeshPro.spriteAsset = signs[type];
        textMeshPro.SetAllDirty();
        textMeshPro.ForceMeshUpdate();
        Transform sign = textMeshPro.transform.GetChild(0);
        sign.localPosition = new Vector3(4, 3, 0);
    }*/
}
