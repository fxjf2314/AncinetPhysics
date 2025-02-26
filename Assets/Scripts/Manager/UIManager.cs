using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
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
    
    
    public int foodBaseNumber;

    public int foodIncrease;

    public int coinIncraese;

    public int totalRound;

    private float totalCoin;

    private float totalFood;

    private int totalPopulation;

    [SerializeField]
    private GameObject nextRoundButtton;
    
    [SerializeField]
    private GameObject areaTip;

    [SerializeField]
    private TextMeshProUGUI roundText;

    [SerializeField]
    private TextMeshProUGUI coinText;

    [SerializeField]
    private TextMeshProUGUI foodText;

    [SerializeField]
    private TextMeshProUGUI populationText;

    #endregion

    //区域集合
    [SerializeField]
    private AreaScript[] areas;

   


    private void Start()
    {
        
        totalRound = 1;
        roundText.text = $"{totalRound}/10";
        
    }

    #region 下一回合按钮代码块
    public void NextRound()
    {
        if (totalRound < 10)
        {
            totalPopulation = 0;
            PopulationNatural();
            FoodNatural();
            CoinNatural();
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
            AreaTips.MyInstance.FadeOut();

            ButtonsManager.MyInstance.isPlaceCard = false;
            ButtonsManager.MyInstance.SearchEvent();
            ButtonsManager.MyInstance.stepButtons[2].transform.gameObject.SetActive(false);
            
        }

    }
    

    //人口自然增长
    public void PopulationNatural()
    {
        foreach (AreaScript area in areas)
        {
            area.PopulationControl((int)(area.areaDetail.food / (area.areaDetail.population * foodBaseNumber)));
            area.PopulationControl(1);

        }
    }

    //粮食自然增长
    public void FoodNatural()
    {
        foreach (AreaScript area in areas)
        {
            area.FoodControl((int)(area.areaDetail.population * foodIncrease));

        }
    }
    //产出自然增长
    public void CoinNatural()
    {
        foreach (AreaScript area in areas)
        {

            area.CoinControl((int)(area.areaDetail.population * coinIncraese));
        }
    }

    #endregion

    public void CheckEvent()
    {
        ButtonsManager.MyInstance.isHappenEvent = false;
        ButtonsManager.MyInstance.stepButtons[1].transform.gameObject.SetActive(true);
    }

}
