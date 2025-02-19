using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformFind
{
    public static Transform TransformFindChild(Transform parent, string name)
    {
        // 先在当前层级查找
        Transform found = parent.Find(name);
        if (found != null)
        {
            return found;
        }
        else
        {
            // 如果当前层级没找到，遍历所有子节点并递归查找
            foreach (Transform child in parent)
            {
                found = TransformFindChild(child, name);
                if (found != null)
                {
                    return found;
                }
            }
        }
        // 如果遍历完所有层级都没找到，返回null
        return null;
    }
}
