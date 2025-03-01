using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtons : MonoBehaviour
{
    [SerializeField]
    Button startGameBtn;
    [SerializeField]
    Button continueGameBtn;
    [SerializeField]
    Button loadGameBtn;
    [SerializeField]
    Button exitGameBtn;

    [SerializeField]
    GameObject filePanel;

    // Start is called before the first frame update
    void Start()
    {
        //filePanel.SetActive(false);
        AddListeners();
    }

    void AddListeners()
    {
        startGameBtn.onClick.AddListener(() =>
        {
            Transition.Instance.LoadSceneWithTransition("PrepareScene");
        });

        if(GameSettingSave.Instance.isExistSave)
        {
            continueGameBtn.onClick.AddListener(() =>
            {
                Transition.Instance.LoadSceneWithTransition("wwwww", GameSettingSave.Instance.setting.currentSave);
            });
        }
        else
        {
            continueGameBtn.interactable = false;
            TextMeshProUGUI tmp = continueGameBtn.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            Color color = tmp.color;
            color.a = 0.5f;
            tmp.color = color;
        }

        loadGameBtn.onClick.AddListener(() =>
        {
            filePanel.SetActive(true);
        });

        exitGameBtn.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            // �������Unity�༭���У�����Unity�Ĺرշ���
            UnityEditor.EditorApplication.isPlaying = false;
#else
			// ������ڹ�������Ϸ�У�����Application���˳�����
			Application.Quit();
#endif
        });
    }
}
