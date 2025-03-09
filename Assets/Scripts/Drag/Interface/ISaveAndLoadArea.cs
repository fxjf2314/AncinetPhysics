using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveAndLoadArea
{
    public void SaveArea(ref AreasJson area);
    public void LoadArea(AreasJson area);
}
