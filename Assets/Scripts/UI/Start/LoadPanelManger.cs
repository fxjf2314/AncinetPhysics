using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanelManger : MonoBehaviour
{
    [SerializeField]
    protected Button save1, save2, save3;
    [SerializeField]
    protected GameObject noSave1, noSave2, noSave3;
    protected Dictionary<Button, GameObject> saves = new Dictionary<Button, GameObject>();

    protected void Awake()
    {
        Debug.Log(1345534);
        saves.Add(save1, noSave1);
        saves.Add(save2, noSave2);
        saves.Add(save3, noSave3);
        ButtonAddListener();
        UpdataPanel();
    }

    private void OnEnable()
    {
        UpdataPanel();
    }

    protected virtual void ButtonAddListener()
    {
        save1.onClick.AddListener(() =>
        {
            Debug.Log("��ת���浵1");
            DataPersistence.Instance.LoadGame(SaveTool.File_Name_01);
        });

        save2.onClick.AddListener(() =>
        {
            Debug.Log("��ת���浵2");
            DataPersistence.Instance.LoadGame(SaveTool.File_Name_02);
        });

        save3.onClick.AddListener(() =>
        {
            Debug.Log("��ת���浵3");
            DataPersistence.Instance.LoadGame(SaveTool.File_Name_03);
        });
    }

    protected void UpdateText(Transform saveGrid, GameData gameData)
    {
        TextMeshProUGUI round = saveGrid.Find("Round").GetComponent<TextMeshProUGUI>();
        round.text = "�غ�����" + gameData.round;
        TextMeshProUGUI people = saveGrid.Find("People").GetComponent<TextMeshProUGUI>();
        round.text = "���˿ڣ�" + gameData.people;
        TextMeshProUGUI harvest = saveGrid.Find("Harvest").GetComponent<TextMeshProUGUI>();
        round.text = "���ճɣ�" + gameData.harvest;
        TextMeshProUGUI output = saveGrid.Find("Output").GetComponent<TextMeshProUGUI>();
        round.text = "�ܲ�����" + gameData.output;
    }

    protected void UpdateSaveShow(Button save, GameData gameData)
    {
        if(gameData == null)
        {
            saves[save].SetActive(true);
        }
        else
        {
            saves[save].SetActive(false);
            UpdateText(save1.transform, gameData);
        }
    }

    protected void UpdataPanel()
    {
        UpdateSaveShow(save1, SaveTool.Load<GameData>(SaveTool.File_Name_01));
        UpdateSaveShow(save2, SaveTool.Load<GameData>(SaveTool.File_Name_02));
        UpdateSaveShow(save3, SaveTool.Load<GameData>(SaveTool.File_Name_03));
    }
}
