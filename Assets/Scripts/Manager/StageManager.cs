using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private static StageManager instance;
    public static StageManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageManager>();
            }
            return instance;
        }
    }

    public SerializableDictionary<Stage, StageDetail> stages;

    

    
}
