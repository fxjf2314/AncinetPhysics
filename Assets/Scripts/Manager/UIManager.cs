using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour,ISaveAndLoadGame
{
    private static UIManager instance;
    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    #region 变量
    public TextMeshProUGUI disasterName;
    
    public int foodBaseNumber;

    public int foodIncrease;

    public int coinIncraese;

    public int totalRound = 1;

    public float totalCoin;

    public float lastCoin;

    public float totalFood;

    public float lastFood;

    public int totalPopulation;

    public int lastPopulation;

    [SerializeField]
    private GameObject nextRoundButtton;
    
    [SerializeField]
    private GameObject areaTip;

    [SerializeField]
    private GameObject settingPanel;

    [SerializeField]
    private TextMeshProUGUI roundText;

    [SerializeField]
    private TextMeshProUGUI coinText;

    [SerializeField]
    private TextMeshProUGUI foodText;

    [SerializeField]
    private TextMeshProUGUI populationText;

    [SerializeField]
    private UnityEngine.UI.Button setting;

    #endregion

    //区域集合
    [SerializeField]
    private AreaScript[] areas;

   


    private void Start()
    {
        
        roundText.text = $"{totalRound}/10";
        //Debug.Log("Start函数");
    }

    #region 下一回合按钮代码块
    public void NextRound()
    {
        if (totalRound < 10)
        {
            lastCoin = totalCoin;
            lastFood = totalFood;
            lastPopulation = totalPopulation;
            totalPopulation = 0;
            
            totalRound++;
            roundText.text = $"{totalRound}/10";
            
            foreach (AreaScript area in areas)
            {
                totalPopulation += area.areaDetail.population;
                totalCoin += area.areaDetail.coin;
                totalFood += area.areaDetail.food;
            }
            coinText.text = totalCoin.ToString();
            foodText.text = totalFood.ToString();
            populationText.text = totalPopulation.ToString();
            PopulationNatural();
            FoodNatural();
            CoinNatural();
            AreaTips.MyInstance.FadeOut();

            ButtonsManager.MyInstance.isPlaceCard = false;
            ButtonsManager.MyInstance.SearchEvent();
            ButtonsManager.MyInstance.waitIcon.transform.gameObject.SetActive(false);
            
        }

    }
    


    //人口自然增长
    public void PopulationNatural()
    {
        foreach (AreaScript area in areas)
        {
            area.areaDetail.oPopulation = area.areaDetail.population;
            area.PopulationControl((int)(area.areaDetail.food / (area.areaDetail.population * foodBaseNumber)));
            area.PopulationControl(1);

        }
    }

    //粮食自然增长
    public void FoodNatural()
    {
        foreach (AreaScript area in areas)
        {
            area.areaDetail.oFood = area.areaDetail.food;
            area.FoodControl((int)(area.areaDetail.population * foodIncrease));

        }
    }
    //产出自然增长
    public void CoinNatural()
    {
        foreach (AreaScript area in areas)
        {
            area.areaDetail.oCoin = area.areaDetail.coin;
            area.CoinControl((int)(area.areaDetail.population * coinIncraese));
        }
    }

    #endregion

    public void CheckEvent()
    {
        ButtonsManager.MyInstance.isHappenEvent = false;
        ButtonsManager.MyInstance.stepButtons[1].transform.gameObject.SetActive(true);
    }

    public void UpdataUI()
    {
        roundText.text = $"{totalRound}/10";
        coinText.text = totalCoin.ToString();
        foodText.text = totalFood.ToString();
        populationText.text = totalPopulation.ToString();
    }

    public void Save(ref GameData gameData)
    {
        gameData.people = totalPopulation;
        gameData.harvest = (int)totalFood;
        gameData.output = (int)totalCoin;
        gameData.round = totalRound;
        if (areas == null) Debug.Log("AreaScripts[]");
        for (int i = 0; i < areas.Length; i++)
        {
            if (areas[i] == null) Debug.Log("AreaScripts");
            if (gameData.areas[i] == null) Debug.Log("这不对吧？");
            areas[i].SaveArea(ref gameData.areas[i]);
            
        }
    }

    public void Load(GameData gameData)
    {
        totalPopulation = gameData.people;
        totalFood = gameData.harvest;
        totalCoin = gameData.output;
        totalRound = gameData.round;
        UpdataUI();
        //Debug.Log("执行读档功能");
        for (int i = 0; i < areas.Length; i++)
        {
            areas[i].LoadArea(gameData.areas[i]);
        }
    }
    
}
