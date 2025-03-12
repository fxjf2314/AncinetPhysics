using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSave : MonoBehaviour,ISaveAndLoadGame
{
    [SerializeField]
    CanvasClickHandler clickHandler;

    Transform[] modelsTransform;
    // Start is called before the first frame update
    void Start()
    {
        modelsTransform = clickHandler.model;
    }

    public void Load(GameData gameData)
    {
        modelsTransform = clickHandler.model;
        for(int i = 0; i < gameData.modelPosition.Length; i++)
        {
            if (modelsTransform[i]!=null)
            {
                modelsTransform[i].position = gameData.modelPosition[i];
            }
        }
    }

    public void Save(ref GameData gameData)
    {
        for (int i = 0; i < gameData.modelPosition.Length; i++)
        {
            if (modelsTransform[i] != null)
            {
                gameData.modelPosition[i] = modelsTransform[i].position;
            }
        }
    }
}
