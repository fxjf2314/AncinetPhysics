using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformFind
{
    public static Transform TransformFindChild(Transform parent, string name)
    {
        // ���ڵ�ǰ�㼶����
        Transform found = parent.Find(name);
        if (found != null)
        {
            return found;
        }
        else
        {
            // �����ǰ�㼶û�ҵ������������ӽڵ㲢�ݹ����
            foreach (Transform child in parent)
            {
                found = TransformFindChild(child, name);
                if (found != null)
                {
                    return found;
                }
            }
        }
        // ������������в㼶��û�ҵ�������null
        return null;
    }
}
