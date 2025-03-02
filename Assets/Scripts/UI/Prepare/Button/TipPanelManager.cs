using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BtnFunction
{
    clearHandCard,
    goToGame,
    goToTitle,
    
}

public class TipPanelManager : MonoBehaviour
{
    public static TipPanelManager Instance => instance;
    static TipPanelManager instance;

    [SerializeField]
    Button confirmBtn;
    [SerializeField]
    TextMeshProUGUI tmp;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        this.gameObject.SetActive(false);
    }

    public void OpenPanel(BtnFunction btnf, string tipText)
    {
        this.gameObject.SetActive(true);
        confirmBtn.onClick.RemoveAllListeners();
        tmp.text = tipText;
        switch (btnf)
        {
            case BtnFunction.clearHandCard:
                {
                    confirmBtn.onClick.AddListener(() =>
                    {
                        List<GameObject> slots = HandCardGroupUI.Instance.slots;
                        int handCardCount = HandCardGroup.Instance.handCards.Count;
                        for (int i = 0; i < handCardCount; i++)
                        {
                            slots[0].GetComponent<HandCardUI>().RemoveCard();
                            //Debug.Log(slots[0].GetComponent<HandCardUI>().isContainCard);
                            //Debug.Log(2);
                        }
                        this.gameObject.SetActive(false);
                    });
                }break;
            case BtnFunction.goToGame:
                {
                    confirmBtn.onClick.AddListener(() =>
                    {
                        Debug.Log("¿ªÊ¼ÓÎÏ·");
                        Transition.Instance.LoadSceneWithTransition("wwwww");
                        this.gameObject.SetActive(false);

                    });
                }break;
            case BtnFunction.goToTitle:
                {
                    confirmBtn.onClick.AddListener(() =>
                    {
                        SceneManager.LoadScene("StartScene");
                        this.gameObject.SetActive(false);
                    });
                }break;
        }
    }
}
