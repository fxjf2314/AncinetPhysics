using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Transition : MonoBehaviour
{
    public static Transition Instance => instance;
    static Transition instance;

    public Image blackScreen; // 黑屏 Image 控件
    [SerializeField]
    public float transitionTime = 1f; // 转场持续时间
    private Coroutine transitionCoroutine; // 控制协程
    string nextSceneName, saveName;

    private void Start()
    {
        instance = this;
        blackScreen = GetComponent<Image>();
        FadeOut();
    }

    // 淡入黑屏（从透明到不透明）
    public void FadeIn()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(FadeToBlack(0f, 1f));
    }

    // 淡出黑屏（从不透明到透明）
    public void FadeOut()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(FadeToBlack(1f, 0f));
    }

    // 控制黑屏渐变的协程
    private IEnumerator FadeToBlack(float startAlpha, float targetAlpha)
    {
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

        // 切换场景时，黑屏完全不透明，加载新场景
        if (blackScreenColor.a == 1)
        {
            if (saveName != null)
            {
                Debug.Log(saveName);
                SceneManager.sceneLoaded += OnSceneLoaded;


            }
            // 替换为你的场景名称
            SceneManager.LoadScene(nextSceneName);
            //DataPersistence.Instance.NewGame();
        }
        blackScreen.raycastTarget = false;
    }

    // 切换场景时的接口
    public void LoadSceneWithTransition(string sceneName, string save)
    {
        nextSceneName = sceneName;
        saveName = save;
        // 先淡入黑屏
        FadeIn();
    }

    public void LoadSceneWithTransition(string sceneName)
    {
        nextSceneName = sceneName;
        // 先淡入黑屏
        FadeIn();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DataPersistence.Instance.LoadGame(saveName);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}