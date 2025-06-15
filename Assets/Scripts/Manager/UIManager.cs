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
    public int finalRound;
    public float coinmessage;
    public float foodmessage;
    public float populationmessage;
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

    public int lastPerson;

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
        totalPopulation = 0;
        foreach (AreaScript area in areas)
        {
            totalPopulation += area.areaDetail.population;
            
            populationmessage = totalPopulation;
            
        }
        populationText.text = totalPopulation.ToString();
        if (HandCard.MyInstance.isLevelMode == false)
        {
            finalRound = 10;
        }
        else
        {
            finalRound = ConfirmedCardsManager.MyInstance.StageRound;
        }
        //setting.onClick.AddListener(()=>OpenSettingPanel());
        //totalRound = 1;
        
        roundText.text = $"{totalRound}/{finalRound}";
        foreach (var area in areas)
        {
            StartCoroutine(CheckPeople(area.areaDetail.oPopulation));
        }
    }

    #region 下一回合按钮代码块
    public void NextRound()
    {
        if (totalRound < finalRound)
        {
            lastCoin = totalCoin;
            lastFood = totalFood;
            lastPopulation = totalPopulation;
            totalPopulation = 0;
            
            totalRound++;
            roundText.text = $"{totalRound}/{finalRound}";
            
            foreach (AreaScript area in areas)
            {
                totalPopulation += area.areaDetail.population;
                totalCoin += area.areaDetail.coin;
                totalFood += area.areaDetail.food;
                populationmessage = totalPopulation;
                coinmessage = totalCoin;
                foodmessage = totalFood;
            }
            coinText.text = totalCoin.ToString();
            foodText.text = totalFood.ToString();
            populationText.text = totalPopulation.ToString();
            PopulationNatural();
            FoodNatural();
            CoinNatural();

            AreaTips.MyInstance.FadeOut();

            //CheckPeople();
            ButtonsManager.MyInstance.isPlaceCard = false;
            ButtonsManager.MyInstance.SearchEvent();
            ButtonsManager.MyInstance.waitIcon.transform.gameObject.SetActive(false);
            
        }
        else if (totalRound == finalRound)
        {
            totalPopulation = 0;
            totalRound++;
            foreach (AreaScript area in areas)
            {
                totalPopulation += area.areaDetail.population;
                totalCoin += area.areaDetail.coin;
                totalFood += area.areaDetail.food;
                populationmessage = totalPopulation;
                coinmessage = totalCoin;
                foodmessage = totalFood;
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
            
            AchievementControl.Instance.GameOverTestAchievement();
        }

    }

    IEnumerator CheckPeople(int oPeople)
    {
        if (DisasterManager.thisDisaster)
            yield return new WaitUntil(() => EventVisualization.Instance.isEffecting);
        while (EventVisualization.Instance.isEffecting)
        {
            yield return null;
        }
        foreach (AreaScript area in areas)
        {
            if(oPeople < 3 && area.areaDetail.population >= 3)
            {
                area.transform.GetComponentInChildren<HouseManager>()?.StartInitHouse();
            }
            if (oPeople < 7 && area.areaDetail.population >= 7)
            {
                area.gameObject.GetComponentInChildren<HouseManager>()?.MiddleInitHouse();
            }
            if (oPeople < 11 && area.areaDetail.population >= 11)
            {
                area.gameObject.GetComponentInChildren<HouseManager>()?.FinalInitHouse();
            }
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
            StartCoroutine(CheckPeople(area.areaDetail.oPopulation));
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
        roundText.text = $"{totalRound}/ {finalRound}";
        coinText.text = totalCoin.ToString();
        foodText.text = totalFood.ToString();
        populationText.text = totalPopulation.ToString();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            settingPanel.SetActive(!settingPanel.activeSelf);
        }
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
    /*private void OpenSettingPanel()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
    }*/
}
