using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveTipPanelManager : MonoBehaviour
{
    public event Action AfterDelete;
    public GameObject saveTipPanel;
    GameObject delete,rename;
    public TMP_InputField input;
    
    // Start is called before the first frame update
    void Start()
    {
        saveTipPanel = transform.Find("SaveTipPanel").gameObject;
        saveTipPanel.SetActive(false);
        delete = TransformFind.TransformFindChild(transform, "Delete").gameObject;
        rename = TransformFind.TransformFindChild(transform, "Rename").gameObject;
        delete.SetActive(false);rename.SetActive(false);
        input = TransformFind.TransformFindChild(rename.transform, "InputField").GetComponent<TMP_InputField>();
        AfterDelete.Invoke();
    }

    public void OpenRenameOrDeletePanel(bool isRename,int index)
    {
        if (isRename)
        {
            OpenRenamePanel(index);
        }
        else
        {
            OpenDeletePanel(index);
        }
    }

    void OpenRenamePanel(int index)
    {
        saveTipPanel.SetActive(true);
        rename.SetActive(true);
        GameData gameData = SaveTool.Load<GameData>("Save0" + index);
        input.text = gameData.saveFileName;
        Button confirmDelete = TransformFind.TransformFindChild(rename.transform, "Yes").GetComponent<Button>();
        confirmDelete.onClick.RemoveAllListeners();
        confirmDelete.onClick.AddListener(() =>
        {
            gameData.saveFileName = input.text;
            SaveTool.Save<GameData>("Save0" + index,gameData);
            rename.SetActive(false);
            saveTipPanel.SetActive(false);
            AfterDelete.Invoke();
        });
    }

    void OpenDeletePanel(int index)
    {
        saveTipPanel.SetActive(true);
        delete.SetActive(true);
        GameData gameData = SaveTool.Load<GameData>("Save0" + index);
        TextMeshProUGUI text = delete.transform.Find("Tip").GetComponent<TextMeshProUGUI>();
        text.text = " «∑Ò»∑»œ…æ≥˝" + gameData.saveFileName + "¥Êµµ";
        Button confirmDelete = TransformFind.TransformFindChild(delete.transform, "Yes").GetComponent<Button>();
        confirmDelete.onClick.RemoveAllListeners();
        confirmDelete.onClick.AddListener(() =>
        {
            DeleteSave(index);
            delete.SetActive(false);
            saveTipPanel.SetActive(false);
        });
        //Button cancelDelete = TransformFind.TransformFindChild(delete.transform, "No").GetComponent<Button>();
        //confirmDelete.onClick.AddListener(() =>
        //{
        //    delete.SetActive(false);
        //});
    }

    public void DeleteSave(int index)
    {
        DataPersistence.Instance.DeleteGame(index);
        AfterDelete.Invoke();
    }
}
