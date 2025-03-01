using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class FontChange : EditorWindow
{
    [MenuItem("FontTools/�滻����������TMP����")]
    public static void ChangeTMPFont_Scene()
    {
        // ����TMP����ѡ�񴰿�
        string fontPath = EditorUtility.OpenFilePanelWithFilters("ѡ��TMP�����ļ�",
            Application.dataPath,
            new[] { "TMP Font", "asset" });

        if (string.IsNullOrEmpty(fontPath))
        {
            Debug.LogWarning("����ѡ����ȡ��");
            return;
        }

        // ת��Ϊ���·��
        if (!fontPath.StartsWith(Application.dataPath))
        {
            Debug.LogError("��ѡ����ĿAssetsĿ¼�ڵ������ļ���");
            return;
        }
        string relativePath = "Assets" + fontPath.Substring(Application.dataPath.Length);

        // ����TMP����
        TMP_FontAsset targetFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(relativePath);
        if (targetFont == null)
        {
            Debug.LogError($"TMP�������ʧ�ܣ���ȷ��·����Ч��{relativePath}");
            return;
        }

        // ��ȡ����������TMP���
        TMP_Text[] allTexts = GameObject.FindObjectsOfType<TMP_Text>(true);
        int replacedCount = 0;

        foreach (TMP_Text text in allTexts)
        {
            // �ų�Prefabʵ���еĶ���
            if (PrefabUtility.IsPartOfPrefabAsset(text.gameObject)) continue;

            Undo.RecordObject(text, "Change TMP Font");
            text.font = targetFont;
            replacedCount++;

            // ��Ƕ������޸�
            EditorUtility.SetDirty(text);
        }

        // ��ǳ�����Ҫ����
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        Debug.Log($"<color=green>�ɹ��滻 {replacedCount} ��TMP�ı����������</color>");
    }

    [MenuItem("FontTools/�滻Ԥ����������TMP����")]
    public static void ChangeTMPFont_Prefab()
    {
        string fontPath = EditorUtility.OpenFilePanelWithFilters("ѡ��TMP�����ļ�",
            Application.dataPath,
            new[] { "TMP Font", "asset" });

        if (string.IsNullOrEmpty(fontPath))
        {
            Debug.LogWarning("����ѡ����ȡ��");
            return;
        }

        string relativePath = "Assets" + fontPath.Substring(Application.dataPath.Length);
        TMP_FontAsset targetFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(relativePath);

        if (targetFont == null)
        {
            Debug.LogError($"TMP�������ʧ�ܣ�{relativePath}");
            return;
        }

        // ��ȡ����Ԥ����
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        int modifiedPrefabs = 0;
        int totalModified = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            bool modified = false;
            TMP_Text[] texts = prefab.GetComponentsInChildren<TMP_Text>(true);

            foreach (TMP_Text text in texts)
            {
                if (text.font != targetFont)
                {
                    text.font = targetFont;
                    modified = true;
                    totalModified++;
                }
            }

            if (modified)
            {
                PrefabUtility.SavePrefabAsset(prefab);
                modifiedPrefabs++;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"<color=green>�޸��� {modifiedPrefabs} ��Ԥ���壬������ {totalModified} ��TMP���</color>");
    }
}