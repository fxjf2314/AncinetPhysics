using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanelManager : MonoBehaviour
{
    [SerializeField]
    Button saveBtn, loadBtn, goToTitleBtn;

    [SerializeField]
    GameObject savePanel, loadPanel;

    private void Start()
    {
        AddListener();
    }

    void AddListener()
    {
        saveBtn.onClick.AddListener(() =>
        {
            savePanel.SetActive(true);
        });

        loadBtn.onClick.AddListener(() =>
        {
            loadPanel.SetActive(true);
        });

        goToTitleBtn.onClick.AddListener(() =>
        {
            TipPanelManager.Instance.OpenPanel(BtnFunction.goToTitle, "�Ƿ񷵻ر������\n!δ��������ݽ������!");
        });
    }
}
