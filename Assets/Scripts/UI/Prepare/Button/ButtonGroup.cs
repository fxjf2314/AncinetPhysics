using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    #region ������ť
    [Header("������ť")]
    [SerializeField]
    Button chunQiuZhanGuoBtn;
    [SerializeField]
    GameObject chunQiuZhanGuo;

    [SerializeField]
    Button qinBtn;
    [SerializeField]
    GameObject qin;

    [SerializeField]
    Button hanBtn;
    [SerializeField]
    GameObject han;

    [SerializeField]
    Button tangBtn;
    [SerializeField]
    GameObject tang;

    [SerializeField]
    Button songBtn;
    [SerializeField]
    GameObject song;

    [SerializeField]
    Button yuanBtn;
    [SerializeField]
    GameObject yuan;

    [SerializeField]
    Button mingBtn;
    [SerializeField]
    GameObject ming;

    [SerializeField]
    Button qingBtn;
    [SerializeField]
    GameObject qing;

    #endregion


    [Header("��Ϸ��ť")]
    [SerializeField]
    Button clearHandCardGroupBtn;


    [SerializeField]
    Button goToGameBtn;

    [Header("ϵͳ��ť")]
    [SerializeField]
    Button backToTitleBtn;

    GameObject currentDy;

    private void Start()
    {
        currentDy = chunQiuZhanGuo;
        AddButtonListener();
    }

    void AddButtonListener()
    {
        chunQiuZhanGuoBtn.onClick.AddListener(() =>
        {
            ChangeDynasty(chunQiuZhanGuo);
        });

        qinBtn.onClick.AddListener(() =>
        {
            ChangeDynasty(qin);
        });

        hanBtn.onClick.AddListener(() =>
        {
            ChangeDynasty(han);
        });

        tangBtn.onClick.AddListener(() =>
        {
            ChangeDynasty(tang);
        });

        songBtn.onClick.AddListener(() =>
        {
            ChangeDynasty(song);
        });

        yuanBtn.onClick.AddListener(() =>
        {
            ChangeDynasty(yuan);
        });

        mingBtn.onClick.AddListener(() =>
        {
            ChangeDynasty(ming);
        });

        qingBtn.onClick.AddListener(() =>
        {
            ChangeDynasty(qing);
        });

        clearHandCardGroupBtn.onClick.AddListener(() =>
        {
            TipPanelManager.Instance.OpenPanel(BtnFunction.clearHandCard, "�Ƿ��������");
        });

        goToGameBtn.onClick.AddListener(() =>
        {
            TipPanelManager.Instance.OpenPanel(BtnFunction.goToGame, "���������ú���" + HandCardGroup.Instance.handCards.Count + "�ſ���,�Ƿ�Ҫ������Ϸ");
        });

        backToTitleBtn.onClick.AddListener(() =>
        {
            TipPanelManager.Instance.OpenPanel(BtnFunction.goToTitle, "�Ƿ�������Ʋ����ر���");
        });

    }

    void ChangeDynasty(GameObject nextDy)
    {
        currentDy.SetActive(false);
        currentDy = nextDy;
        currentDy.SetActive(true);
    }
}
