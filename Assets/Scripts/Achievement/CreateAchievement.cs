using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Achievement")]
[System.Serializable]
public class CreateAchievement : ScriptableObject
{
    public int id;//成就id
    public string achievementname;//成就名
    public Sprite blacksprite;//黑白原画
    public Sprite finishsprite;//彩色原画
    [TextArea]
    public string icon;//成就介绍
    [TextArea]
    public string addicon;//附加成就介绍
    public bool ifhide;//是否为隐藏成就
    public bool finish;//是否达成
}
