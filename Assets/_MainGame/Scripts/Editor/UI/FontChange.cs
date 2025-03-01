using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class FontChange : EditorWindow
{
    [MenuItem("FontTools/替换场景中所有TMP字体")]
    public static void ChangeTMPFont_Scene()
    {
        // 弹出TMP字体选择窗口
        string fontPath = EditorUtility.OpenFilePanelWithFilters("选择TMP字体文件",
            Application.dataPath,
            new[] { "TMP Font", "asset" });

        if (string.IsNullOrEmpty(fontPath))
        {
            Debug.LogWarning("字体选择已取消");
            return;
        }

        // 转换为相对路径
        if (!fontPath.StartsWith(Application.dataPath))
        {
            Debug.LogError("请选择项目Assets目录内的字体文件！");
            return;
        }
        string relativePath = "Assets" + fontPath.Substring(Application.dataPath.Length);

        // 加载TMP字体
        TMP_FontAsset targetFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(relativePath);
        if (targetFont == null)
        {
            Debug.LogError($"TMP字体加载失败，请确认路径有效：{relativePath}");
            return;
        }

        // 获取场景中所有TMP组件
        TMP_Text[] allTexts = GameObject.FindObjectsOfType<TMP_Text>(true);
        int replacedCount = 0;

        foreach (TMP_Text text in allTexts)
        {
            // 排除Prefab实例中的对象
            if (PrefabUtility.IsPartOfPrefabAsset(text.gameObject)) continue;

            Undo.RecordObject(text, "Change TMP Font");
            text.font = targetFont;
            replacedCount++;

            // 标记对象已修改
            EditorUtility.SetDirty(text);
        }

        // 标记场景需要保存
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        Debug.Log($"<color=green>成功替换 {replacedCount} 个TMP文本组件的字体</color>");
    }

    [MenuItem("FontTools/替换预设物中所有TMP字体")]
    public static void ChangeTMPFont_Prefab()
    {
        string fontPath = EditorUtility.OpenFilePanelWithFilters("选择TMP字体文件",
            Application.dataPath,
            new[] { "TMP Font", "asset" });

        if (string.IsNullOrEmpty(fontPath))
        {
            Debug.LogWarning("字体选择已取消");
            return;
        }

        string relativePath = "Assets" + fontPath.Substring(Application.dataPath.Length);
        TMP_FontAsset targetFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(relativePath);

        if (targetFont == null)
        {
            Debug.LogError($"TMP字体加载失败：{relativePath}");
            return;
        }

        // 获取所有预制体
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
        Debug.Log($"<color=green>修改了 {modifiedPrefabs} 个预制体，共更新 {totalModified} 个TMP组件</color>");
    }
}