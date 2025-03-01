using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    #region 朝代按钮
    [Header("朝代按钮")]
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


    [Header("游戏按钮")]
    [SerializeField]
    Button clearHandCardGroupBtn;


    [SerializeField]
    Button goToGameBtn;

    [Header("系统按钮")]
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
            TipPanelManager.Instance.OpenPanel(BtnFunction.clearHandCard, "是否清空手牌");
        });

        goToGameBtn.onClick.AddListener(() =>
        {
            TipPanelManager.Instance.OpenPanel(BtnFunction.goToGame, "你现在配置好了" + HandCardGroup.Instance.handCards.Count + "张卡牌,是否要进入游戏");
        });

        backToTitleBtn.onClick.AddListener(() =>
        {
            TipPanelManager.Instance.OpenPanel(BtnFunction.goToTitle, "是否清空手牌并返回标题");
        });

    }

    void ChangeDynasty(GameObject nextDy)
    {
        currentDy.SetActive(false);
        currentDy = nextDy;
        currentDy.SetActive(true);
    }
}
