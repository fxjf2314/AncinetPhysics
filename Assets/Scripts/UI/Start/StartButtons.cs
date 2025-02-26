using System.Collections;
using System.Collections.Generic;
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
        filePanel.SetActive(false);
        AddListeners();
    }

    void AddListeners()
    {
        startGameBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("PrepareScene");
        });

        continueGameBtn.onClick.AddListener(() =>
        {

        });

        loadGameBtn.onClick.AddListener(() =>
        {
            filePanel.SetActive(true);
        });

        exitGameBtn.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            // 如果是在Unity编辑器中，调用Unity的关闭方法
            UnityEditor.EditorApplication.isPlaying = false;
#else
			// 如果是在构建的游戏中，调用Application的退出方法
			Application.Quit();
#endif
        });
    }
}
