using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public int foodBaseNumber;
    
    


    public int totalRound;

    private float totalCoin;

    private float totalFood;

    private int totalPopulation;
    
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

    [SerializeField]
    private AreaScript[] areas;

    private Image nextRound;

    private Image tip;

    private void Start()
    {
        nextRound = GetComponentInChildren<Image>();
        totalRound = 1;
        roundText.text = $"{totalRound}/10";
    }

    public void NextRound()
    {
        if(totalRound < 10)
        {
            totalPopulation = 0;
            PopulationNatural();
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
            //area.FoodControl((int)(area.areaDetail.population * ));

        }
    }
    //产出自然增长
    public void CoinNatural()
    {
        foreach (AreaScript area in areas)
        {
            

        }
    }
}
