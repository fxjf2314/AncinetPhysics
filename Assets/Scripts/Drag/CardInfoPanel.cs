using UnityEngine;
using TMPro; // ���� TextMeshPro �����ռ�

public class CardInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI areaText; // ʹ�� TextMeshProUGUI
    public TextMeshProUGUI effectText; // ʹ�� TextMeshProUGUI

    public void SetCardInfo(string area, string effect)
    {
        areaText.text = "Area: " + area;
        effectText.text = "Effect: " + effect;
    }
}
