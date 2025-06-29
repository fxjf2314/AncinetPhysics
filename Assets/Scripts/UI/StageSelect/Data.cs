using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct Data {

    public int index;

    public Sprite image;

    public string name;

    public Color color;

    public List<Card> cards;

    public int StageRound;

    public Stage stageType;
}

