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
            // �������Unity�༭���У�����Unity�Ĺرշ���
            UnityEditor.EditorApplication.isPlaying = false;
#else
			// ������ڹ�������Ϸ�У�����Application���˳�����
			Application.Quit();
#endif
        });
    }
}
