using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiDong", menuName = "Card/DiDong", order = 10)]
public class DiDong : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
            if (area != null)
            {
            if (area.cards.Count < 3)
            {
                for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
                {
                    if (HandCard.MyInstance.applicationArea[i] != null)
                    {
                        area.transform.parent.GetComponent<DisasterOccurs>().earthQuake.population = 0;
                        area.transform.parent.GetComponent<DisasterOccurs>().earthQuake.coinRatio -= 0.2f;
                        area.transform.parent.GetComponent<DisasterOccurs>().earthQuake.foodRatio -= 0.2f;
                    }
                }
            }
        }
        
        //受地震影响时，人口不减少
        //受地震影响时，收成降低更少。
        //受地震影响时，产出降低更少。
    }
}
