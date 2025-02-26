using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空间

public class CardInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI areaText; // 使用 TextMeshProUGUI
    public TextMeshProUGUI effectText; // 使用 TextMeshProUGUI

    public void SetCardInfo(string area, string effect)
    {
        areaText.text = "Area: " + area;
        effectText.text = "Effect: " + effect;
    }
}
