using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Transition : MonoBehaviour
{
    public static Transition Instance => instance;
    static Transition instance;

    public Image blackScreen; // ���� Image �ؼ�
    [SerializeField]
    public float transitionTime = 1f; // ת������ʱ��
    private Coroutine transitionCoroutine; // ����Э��
    [SerializeField]
    bool isFadeOutWhenOpen = true;
    string nextSceneName, saveName;

    private void Start()
    {
        instance = this;
        blackScreen = GetComponent<Image>();
        if(isFadeOutWhenOpen)
        {
            FadeOut();
            blackScreen.raycastTarget = false;
        }
        
    }

    // �����������͸������͸����
    public void FadeIn()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(FadeToBlack(0f, 1f));
    }

    // �����������Ӳ�͸����͸����
    public void FadeOut()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(FadeToBlack(1f, 0f));
    }

    // ���ƺ��������Э��
    private IEnumerator FadeToBlack(float startAlpha, float targetAlpha)
    {
        blackScreen.raycastTarget = true;
        float elapsedTime = 0f;
        Color blackScreenColor = blackScreen.color;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / transitionTime);
            blackScreenColor.a = alpha;
            blackScreen.color = blackScreenColor;
            yield return null;
        }

        // �л�����ʱ��������ȫ��͸���������³���
        if (blackScreenColor.a == 1)
        {
            if (saveName != null)
            {
                //Debug.Log(saveName);
                SceneManager.sceneLoaded += OnSceneLoaded;


            }
            // �滻Ϊ��ĳ�������
            SceneManager.LoadScene(nextSceneName);
            DataPersistence.Instance.NewGame();
        }
        blackScreen.raycastTarget = false;
    }

    // �л�����ʱ�Ľӿ�
    public void LoadSceneWithTransition(string sceneName, string save)
    {
        nextSceneName = sceneName;
        saveName = save;
        // �ȵ������
        FadeIn();
    }

    public void LoadSceneWithTransition(string sceneName)
    {
        nextSceneName = sceneName;
        // �ȵ������
        FadeIn();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DataPersistence.Instance.LoadGame(saveName);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}