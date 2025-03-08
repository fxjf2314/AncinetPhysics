using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaScript : MonoBehaviour
{
    public AreaDetail areaDetail;

    public List<Card> cards;

    private void Awake()
    {
        areaDetail.Effectiveness = new Dictionary<string, float>()
        {
            {"War", 1f },
            {"EarthQuake",1f },
            {"Flood",1f },
            {"DustStorm",1f },
            {"Fog",1f }
        };
    }
    public string GetName()
    {
        return areaDetail.areaName;
    }
    public string GetPopulation()
    {
        return areaDetail.population.ToString();
    }
    public string GetCoin()
    {
        return areaDetail.coin.ToString();


    }
    public string GetFood()
    {
        return areaDetail.food.ToString();
    }

    public Sprite GetIcon()
    {
        return areaDetail.areaIcon;
    }

    public void PopulationControl(int count)
    {
        areaDetail.population += count;
    }

    public void CoinControl(int count)
    {
        areaDetail.coin += count;
    }

    public void FoodControl(int count)
    {
        areaDetail.food += count;
    }
}
