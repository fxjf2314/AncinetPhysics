#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
public class SerializableDictionaryDrawer : PropertyDrawer
{
    private const float DeleteButtonWidth = 25f;
    private const float LineSpacing = 2f;
    private const float MinTextAreaHeight = 35f;

    // ����֧�ֻ��е��ı���ʽ
    private static GUIStyle _wrappedTextAreaStyle;
    private static GUIStyle WrappedTextAreaStyle
    {
        get
        {
            if (_wrappedTextAreaStyle == null)
            {
                _wrappedTextAreaStyle = new GUIStyle(EditorStyles.textArea)
                {
                    wordWrap = true,
                    padding = new RectOffset(4, 4, 4, 4)
                };
            }
            return _wrappedTextAreaStyle;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var keys = property.FindPropertyRelative("keys");
        var values = property.FindPropertyRelative("values");

        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        // �����ֵ���Ŀ
        for (int i = 0; i < keys.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // ɾ����ť����ࣩ
            if (GUILayout.Button("��", GUILayout.Width(DeleteButtonWidth)))
            {
                keys.DeleteArrayElementAtIndex(i);
                values.DeleteArrayElementAtIndex(i);
                break;
            }

            // ����������
            EditorGUILayout.BeginVertical();
            {
                var keyProperty = keys.GetArrayElementAtIndex(i);
                var valueProperty = values.GetArrayElementAtIndex(i);

                // ���Ƽ�
                EditorGUILayout.PropertyField(keyProperty, GUIContent.none);

                // ��̬�����ı��߶�
                float textHeight = MinTextAreaHeight;
                if (valueProperty.propertyType == SerializedPropertyType.String)
                {
                    textHeight = WrappedTextAreaStyle.CalcHeight(
                        new GUIContent(valueProperty.stringValue),
                        EditorGUIUtility.currentViewWidth - DeleteButtonWidth - 30
                    ) + 8;
                    textHeight = Mathf.Max(textHeight, MinTextAreaHeight);
                }

                // ����ֵ�����Զ����У�
                if (valueProperty.propertyType == SerializedPropertyType.String)
                {
                    valueProperty.stringValue = EditorGUILayout.TextArea(
                        valueProperty.stringValue,
                        WrappedTextAreaStyle,
                        GUILayout.Height(textHeight)
                    );
                }
                else
                {
                    EditorGUILayout.PropertyField(valueProperty, GUIContent.none);
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(LineSpacing);
        }

        // ��Ӱ�ť
        if (GUILayout.Button("�� Add Entry", GUILayout.Height(25)))
        {
            keys.arraySize++;
            values.arraySize++;
        }

        EditorGUI.indentLevel--;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var keys = property.FindPropertyRelative("keys");
        var values = property.FindPropertyRelative("values");

        float totalHeight = EditorGUIUtility.singleLineHeight * 2; // ����+��ť

        for (int i = 0; i < keys.arraySize; i++)
        {
            var valueProperty = values.GetArrayElementAtIndex(i);
            float itemHeight = EditorGUIUtility.singleLineHeight; // ���ĸ߶�

            // ����ֵ�ĸ߶�
            if (valueProperty.propertyType == SerializedPropertyType.String)
            {
                var content = new GUIContent(valueProperty.stringValue);
                float textHeight = WrappedTextAreaStyle.CalcHeight(
                    content,
                    EditorGUIUtility.currentViewWidth - DeleteButtonWidth - 30
                ) + 8;
                itemHeight += Mathf.Max(textHeight, MinTextAreaHeight);
            }
            else
            {
                itemHeight += EditorGUIUtility.singleLineHeight;
            }

            totalHeight += itemHeight + LineSpacing;
        }

        return totalHeight;
    }
}
#endif