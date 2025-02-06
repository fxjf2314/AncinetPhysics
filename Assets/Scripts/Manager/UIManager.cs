using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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

    private void Start()
    {
        totalRound = 0;
    }

    public void NextRound()
    {
        if(totalRound < 10)
        {
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
        }
        
    }
}
