using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AreaScript : MonoBehaviour,ISaveAndLoadArea
{
    public AreaDetail areaDetail;

    public List<Card> cards;

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

    public void SaveArea(ref AreasJson area)
    {
        area.cards = cards;
        area.areaDetail = areaDetail;
    }

    public void LoadArea(AreasJson area)
    {
        cards = area.cards;
        areaDetail = area.areaDetail;
    }
}
