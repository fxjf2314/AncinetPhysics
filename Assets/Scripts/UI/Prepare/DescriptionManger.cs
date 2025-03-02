using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DescriptionManger : MonoBehaviour
{
    public static DescriptionManger Instance => instance;
    static DescriptionManger instance;

    [SerializeField]
    GameObject simpleDes;
    [SerializeField]
    GameObject detailDes;

    [SerializeField]
    Vector2 simpleDesOffset;//介绍悬浮窗口与鼠标的偏移值

    Coroutine timer;//用于计时的协程
    Image progressBar;
    [SerializeField]
    float time;//鼠标需要停留的时间
    public bool isCanClick => mIsCanClick;//是否能点击显示详细介绍窗口
    bool mIsCanClick;

    private void Start()
    {
        instance = this;
        //simpleDes.SetActive(false);
        //detailDes.SetActive(false);
        progressBar = simpleDes.transform.Find("ProgressBar").GetComponent<Image>();
    }

    public void OpenSimpleDes(Card cardData)
    {
        if(simpleDes != null)
        {
            simpleDes.SetActive(true);
            simpleDes.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = cardData.GetDescription().title;
            simpleDes.transform.Find("Area").GetComponent<TextMeshProUGUI>().text = cardData.GetDescription().area;
            simpleDes.transform.Find("Effect").GetComponent<TextMeshProUGUI>().text = cardData.GetDescription().effect;
            LayoutRebuilder.ForceRebuildLayoutImmediate(simpleDes.GetComponent<RectTransform>());
            timer = StartCoroutine(CanClickTime());
        }
        
    }

    public void OpenDetailDes(Card cardData)
    {
        if (SceneManager.GetActiveScene().name == "wwwww") return;
        detailDes.SetActive(true);
        TransformFind.TransformFindChild(detailDes.transform,"Icon").GetComponent<Image>().sprite = cardData.GetSprite();
        TransformFind.TransformFindChild(detailDes.transform, "Title").GetComponent<TextMeshProUGUI>().text = cardData.GetDescription().title;
        TransformFind.TransformFindChild(detailDes.transform, "Area").GetComponent<TextMeshProUGUI>().text = cardData.GetDescription().area;
        TransformFind.TransformFindChild(detailDes.transform, "Effect").GetComponent<TextMeshProUGUI>().text = cardData.GetDescription().effect;
        LayoutRebuilder.ForceRebuildLayoutImmediate(detailDes.GetComponent<RectTransform>());
    }

    public void CloseSimpleDes()
    {
        simpleDes.SetActive(false);
        if (timer != null)
        {
            StopCoroutine(timer);
        }
        mIsCanClick = false;
        progressBar.fillAmount = 0;
    }

    public void CloseDetailedDes()
    {
        detailDes.SetActive(false);
    }

    private void simpleDesPosUpdata()
    {
        if (simpleDes.activeSelf)
        {
            Vector3 mousePos = Input.mousePosition;
            simpleDes.transform.position = new Vector3(mousePos.x , mousePos.y , 0);
        }
    }

    IEnumerator CanClickTime()
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            progressBar.fillAmount = Mathf.Lerp(0, 1, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        }
        mIsCanClick = true;
    }

    private void Update()
    {
        simpleDesPosUpdata();
    }

}
