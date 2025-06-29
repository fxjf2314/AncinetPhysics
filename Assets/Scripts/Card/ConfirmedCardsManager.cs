using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConfirmedCardsManager : MonoBehaviour
{
    private static ConfirmedCardsManager Instance;
    public static  ConfirmedCardsManager MyInstance => Instance;

    public List<Card> ConfirmedCards = new List<Card>();

    public int StageRound;

    public Stage confirmStageType;

    public int confirmLevelNum;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
