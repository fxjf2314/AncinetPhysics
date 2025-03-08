using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailedPanel : MonoBehaviour
{
    public static DetailedPanel Instance => instance;
    static DetailedPanel instance;

    Image icon;
    TextMeshProUGUI title;
    TextMeshProUGUI detailDes;
    

    // Start is called before the first frame update
    void Start()
    {
        icon = TransformFind.TransformFindChild(transform, "Icon").GetComponent<Image>();
        title = TransformFind.TransformFindChild(transform, "Title").GetComponent<TextMeshProUGUI>();
        detailDes = TransformFind.TransformFindChild(transform, "DetailDes").GetComponent<TextMeshProUGUI>();
    }

    public void LoadCardData(Card cardData)
    {
        icon.sprite = cardData.GetSprite();
        title.text = cardData.GetDescription().title;
        //Debug.Log(detailDes.gameObject.name);
        detailDes.text = cardData.GetDescription().detailDes;
    }
}
