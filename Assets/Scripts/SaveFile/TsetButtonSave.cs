using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TsetButtonSave : MonoBehaviour
{
    [SerializeField]
    Button saveButton;
    [SerializeField]
    Button LoadButton;

    // Start is called before the first frame update
    void Start()
    {
        saveButton.onClick.AddListener(() =>
        {
            DataPersistence.Instance.SaveGame(SaveTool.File_Name_01);
        });

        LoadButton.onClick.AddListener(() =>
        {
            DataPersistence.Instance.LoadGame(SaveTool.File_Name_01);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
