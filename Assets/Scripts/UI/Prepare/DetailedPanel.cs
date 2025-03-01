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
    TextMeshProUGUI area;
    TextMeshProUGUI effect;

    // Start is called before the first frame update
    void Start()
    {
        icon = TransformFind.TransformFindChild(transform, "Icon").GetComponent<Image>();
        title = TransformFind.TransformFindChild(transform, "Title").GetComponent<TextMeshProUGUI>();
        area = TransformFind.TransformFindChild(transform, "Area").GetComponent<TextMeshProUGUI>();
        effect = TransformFind.TransformFindChild(transform, "Effect").GetComponent<TextMeshProUGUI>();
    }

    public void LoadCardData(Card cardData)
    {
        icon.sprite = cardData.GetSprite();
        title.text = cardData.GetDescription().title;
        area.text = cardData.GetDescription().area;
        effect.text = cardData.GetDescription().effect;
    }
}
