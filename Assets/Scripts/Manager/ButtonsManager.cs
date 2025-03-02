using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
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

    //ÿһ���İ�ť����
    public UnityEngine.UI.Button[] stepButtons;

    private void Start()
    {
        //WaitRound();
        for (int i = 0; i < stepButtons.Length; i++)
        {
            int index = i; // ���浱ǰ����
            stepButtons[i].onClick.AddListener(() => OnButtonClick(index));
        }
        AddEvent(SearchEventNext);
        AddEvent(RemindCardNext);
        AddEvent(WaitRound);
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

    #region �鿴�����¼������
    public void SearchEvent()
    {
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

    #region ���ѷ��ÿ��ƴ����
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

    #region �ȴ������

   
    public Image waitIcon;

    public float rotationInterval;

    private float rotationDuration = 1.5f;

    private Quaternion targetRotation;
    
    public void WaitRound()
    {
        stepButtons[2].transform.gameObject.SetActive(false);
        waitIcon.transform.gameObject.SetActive(true);
        StartCoroutine(IconRotation());
    }

    private IEnumerator IconRotation()
    {
        float elapsedTimeFirst = 0.0f; 
        while (elapsedTimeFirst < 3.0f)
        {
            targetRotation = waitIcon.transform.rotation * Quaternion.Euler(0,0, 180);

            // ƽ����ת��Ŀ��Ƕ�
            float elapsedTime = 0.0f;
            while (elapsedTime < rotationDuration)
            {
                waitIcon.transform.rotation = Quaternion.Slerp(waitIcon.transform.rotation, targetRotation, elapsedTime / rotationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTimeFirst += rotationInterval;
            // �ȴ����ʱ��
            yield return new WaitForSeconds(rotationInterval);
        }
         UIManager.MyInstance.NextRound();
    }

    
    #endregion
}
