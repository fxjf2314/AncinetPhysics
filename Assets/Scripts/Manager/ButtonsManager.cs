using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class ButtonsManager : MonoBehaviour
{
    private static ButtonsManager instance;
    public static ButtonsManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ButtonsManager>();
            }
            return instance;
        }
    }

    public delegate void EventDelegate();

    private List<EventDelegate> stepEvents = new List<EventDelegate>();

    public void AddEvent(EventDelegate method)
    {
        stepEvents.Add(method);
    }
    
    public bool isPlaceCard;

    public bool isHappenEvent;

    //每一步的按钮集合
    public UnityEngine.UI.Button[] stepButtons;

    private void Start()
    {
        for (int i = 0; i < stepButtons.Length; i++)
        {
            int index = i; // 保存当前索引
            stepButtons[i].onClick.AddListener(() => OnButtonClick(index));
        }
        AddEvent(SearchEventNext);
        AddEvent(RemindCardNext);
        AddEvent(UIManager.MyInstance.NextRound);
        RemindCard();
    }

    private void OnButtonClick(int index)
    {
        //Debug.Log("111");
        stepEvents[index]?.Invoke();
        if(index != 2)
        {
            stepButtons[index].onClick.RemoveListener(()=>OnButtonClick(index));
            stepButtons[index].transform.gameObject.SetActive(false);
            stepButtons[index + 1].transform.gameObject.SetActive(true);
        }
        
    }

    private void Update()
    {
        if(!isHappenEvent)
        {
            StopCoroutine(CheckHappenEvent());
        }
    }

    #region 查看区域事件代码块
    public void SearchEvent()
    {
        Debug.Log("1111");
        if(isHappenEvent)
        {
            stepButtons[0].transform.gameObject.SetActive(true);

            StartCoroutine(CheckHappenEvent());
        }
        else
        {
            OnButtonClick(0);
        }
        
        
    }

    public void SearchEventNext()
    {
        isHappenEvent = false;
        //stepButtons[0].transform.gameObject.SetActive(false);
        StopCoroutine(CheckHappenEvent());
    }
    

    private IEnumerator CheckHappenEvent()
    {
        while(true)
        {
            if (!isHappenEvent)
            {
                stepButtons[0].transform.gameObject.SetActive(false);
            }
            yield return null;
        }
        
        
    }
    #endregion

    #region 提醒放置卡牌代码块
    public void RemindCard()
    {
        if(!isPlaceCard)
        {
            stepButtons[1].transform.gameObject.SetActive(true);
            StartCoroutine(CheckCardPlaced());
        }
        else
        {
            OnButtonClick(1);
        }
    }
        
    public void RemindCardNext()
    {
        isPlaceCard = true;
        StopCoroutine(CheckCardPlaced());
    }

    private IEnumerator CheckCardPlaced()
    {
        while(true)
        {
            if(isPlaceCard)
            {
                stepButtons[1].transform.gameObject.SetActive(false);
            }
            yield return null;
        }
    }
    #endregion
}
