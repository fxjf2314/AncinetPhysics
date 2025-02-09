using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mozi", menuName = "Card/Mozi", order = 4)]
public class Mozi : Card
{
    public override void Use(AreaScript area)
    {
        Debug.Log("111");
        base.Use(area  );

        if (area.cards.Count < 3)
        {
            area.CoinControl(200);
        }
    }
}
