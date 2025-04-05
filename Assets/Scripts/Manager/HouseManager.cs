using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] houses;

    public void StartInitHouse()
    {
        for(int i = 0; i < 3; i++)
        {
            houses[GameSeed.MyInstance.houseSeed[i]].SetActive(true);
        }
    }

    public void MiddleInitHouse()
    {
        for (int i = 3; i < 6; i++)
        {
            houses[GameSeed.MyInstance.houseSeed[i]].SetActive(true);
        }
    }

    public void FinalInitHouse()
    {
        for(int i = 6; i < 8; i++)
        {
            houses[GameSeed.MyInstance.houseSeed[i]].SetActive(true);
        }
    }
    
}
