using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDescribable //所有可描述的UI可以挂这个接口
{
    Description GetDescription();

    Sprite GetSprite();
}
