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

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }
}
