using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    #region 朝代按钮
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

    [SerializeField]
    Button clearHandCardGroupBtn;

    [SerializeField]
    Button backToTitleBtn;

    [SerializeField]
    Button goToGameBtn;
    [SerializeField]
    GameObject goToGameTip;

    [SerializeField]
    Button confirmStartGameBtn;

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
            List<GameObject> slots = HandCardGroupUI.Instance.slots;
            int handCardCount = HandCardGroup.Instance.handCards.Count;
            for (int i = 0; i < handCardCount; i++)
            {
                slots[0].GetComponent<HandCardUI>().RemoveCard();
                //Debug.Log(slots[0].GetComponent<HandCardUI>().isContainCard);
                //Debug.Log(2);
            }
        });

        goToGameBtn.onClick.AddListener(() =>
        {
            Debug.Log("111");
            goToGameTip.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "你现在配置好了" + HandCardGroup.Instance.handCards.Count + "张卡牌,是否要进入游戏";
        });

        confirmStartGameBtn.onClick.AddListener(() =>
        {

        });

    }

    void ChangeDynasty(GameObject nextDy)
    {
        currentDy.SetActive(false);
        currentDy = nextDy;
        currentDy.SetActive(true);
    }
}
