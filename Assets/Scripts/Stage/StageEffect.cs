using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEffect : MonoBehaviour
{
    public SerializableDictionary<Stage, StageAffect> stageAffects;
    private static StageEffect instance;
    public static StageEffect Instance => instance;
    private void Awake()
    {
        instance = this;
        if (ConfirmedCardsManager.MyInstance)
        {
            Debug.Log(ConfirmedCardsManager.MyInstance.confirmStageType);
            stageAffects[ConfirmedCardsManager.MyInstance.confirmStageType].Effect();
        }
    }
}
