using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtons : MonoBehaviour,IPointerClickHandler
{

    #region 按钮组
    [SerializeField]
    Button startGameBtn;
    [SerializeField]
    Button freeGameBtm;
    [SerializeField]
    Button stageGameBtn;
    [SerializeField]
    Button continueGameBtn;
    [SerializeField]
    Button loadGameBtn;
    [SerializeField]
    Button exitGameBtn;
    [SerializeField]
    Button TutorialBtn;
    #endregion

    #region 变量
    [SerializeField]
    private CanvasGroup freeGameBtnCanvas;
    [SerializeField]
    private CanvasGroup stageGameBtnCanvas;
    [SerializeField]
    private CanvasGroup startGameBtnCanvas;

    [SerializeField]
    private RectTransform startGameBtnTransform;
    [SerializeField]
    private RectTransform freeGameBtnTransform;
    [SerializeField]
    private RectTransform stageGameBtnTransform;
    private Vector2 freeGameBtnStartPos;
    private Vector2 stageGameBtnStartPos;
    private Vector2 freeGameBtnFinalPos = new Vector2(-18,-394);
    private Vector2 stageGameBtnFinalPos = new Vector2(-18,49);
    private Vector2 startGameBtnPos = new Vector2(-18,-165);

    private float duration = 0.2f;

    private bool isChooseMode;
    private bool isHaveSave;
    #endregion

    [SerializeField]
    GameObject filePanel;

    // Start is called before the first frame update
    void Start()
    {
        freeGameBtnStartPos = freeGameBtnTransform.localPosition;
        stageGameBtnStartPos = stageGameBtnTransform.localPosition;
        
        startGameBtnCanvas.alpha = 1.0f;
        freeGameBtnCanvas.alpha = 0;
        stageGameBtnCanvas.alpha = 0;

        //filePanel.SetActive(false);
        AddListeners();
    }

    
    

    void AddListeners()
    {
        
        stageGameBtn.onClick.AddListener(() =>
        {
            Transition.Instance.LoadSceneWithTransition("StageSelect");
        });
        freeGameBtm.onClick.AddListener(() =>
        {
            Transition.Instance.LoadSceneWithTransition("PrepareScene");
        });

        if(GameSettingSave.Instance.isExistSave)
        {
            isHaveSave = true;
            continueGameBtn.onClick.AddListener(() =>
            {
                Transition.Instance.LoadSceneWithTransition("wwwww", GameSettingSave.Instance.setting.currentSave);
            });
        }
        else
        {
            isHaveSave = false;
            continueGameBtn.interactable = false;
            Image tmp = continueGameBtn.transform.Find("text").GetComponent<Image>();
            Color color = tmp.color;
            color.a = 0.5f;
            tmp.color = color;
        }

        startGameBtn.onClick.AddListener(() =>
        {
            if (isHaveSave == true)
            {
                MakeButtonInteractable(continueGameBtn);
            }
            //MakeButtonInteractable(continueGameBtn);
            MakeButtonInteractable(exitGameBtn);
            MakeButtonInteractable(loadGameBtn);
            MakeButtonInteractable(TutorialBtn);

            isChooseMode = true;
            FadeOut(startGameBtnCanvas, startGameBtnTransform, startGameBtnPos);
            startGameBtnCanvas.blocksRaycasts = false;
            FadeIn(stageGameBtnCanvas, stageGameBtnTransform, stageGameBtnFinalPos);
            FadeIn(freeGameBtnCanvas, freeGameBtnTransform, freeGameBtnFinalPos);
            stageGameBtnCanvas.blocksRaycasts = true;
            freeGameBtnCanvas.blocksRaycasts = true;

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

    private void FadeIn(CanvasGroup canvasGroup,RectTransform tsf,Vector2 finalPos)
    {
        StartCoroutine(FadeCanvasGroupRoutine(canvasGroup, tsf, canvasGroup.alpha, 1, duration, finalPos));
        
        /*if (currentCoroutine1 == null)
        {
            currentCoroutine1 = ;
        }
        else if (currentCoroutine2 == null)
        {
            currentCoroutine2 = StartCoroutine(FadeCanvasGroupRoutine(canvasGroup, tsf, canvasGroup.alpha, 1, duration, finalPos));
        }*/


    }

    private void FadeOut(CanvasGroup canvasGroup, RectTransform tsf, Vector2 startPos)
    {
        StartCoroutine(FadeCanvasGroupRoutine(canvasGroup, tsf, canvasGroup.alpha, 0, duration, startPos));
        /*if (currentCoroutine1 == null)
        {
            currentCoroutine1 = 
        }
        else if (currentCoroutine2 == null)
        {
            currentCoroutine2 = StartCoroutine(FadeCanvasGroupRoutine(canvasGroup, tsf, canvasGroup.alpha, 0, duration, startPos));
        }*/
    }

    private IEnumerator FadeCanvasGroupRoutine(CanvasGroup cg, RectTransform tsf, float start, float end, float duration, Vector2 targetPosition)
    {

        float counter = 0f;
        Vector2 currentPosition = tsf.anchoredPosition;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            // 使用Mathf.Lerp在指定时间内平滑过渡alpha值
            cg.alpha = Mathf.Lerp(start, end, counter / duration);
            tsf.anchoredPosition = Vector2.Lerp(currentPosition, targetPosition, counter / duration);
            // 如果alpha值已经接近目标值，提前结束循环
            if (Mathf.Approximately(cg.alpha, end))
            {
                break;
            }

            yield return null; // 等待下一帧
        }
        // 确保alpha值精确设置为目标值
        cg.alpha = end;
        // 清除协程引用，释放资源
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isChooseMode)
            {
                if(isHaveSave == true)
                {
                    MakeButtonInteractable(continueGameBtn);
                }
                
                MakeButtonInteractable(exitGameBtn);
                MakeButtonInteractable(loadGameBtn);
                MakeButtonInteractable(TutorialBtn);
                isChooseMode = false;
                FadeOut(stageGameBtnCanvas, stageGameBtnTransform, stageGameBtnStartPos);
                FadeOut(freeGameBtnCanvas, freeGameBtnTransform, freeGameBtnStartPos);
                stageGameBtnCanvas.blocksRaycasts = false;
                freeGameBtnCanvas.blocksRaycasts = false;
                FadeIn(startGameBtnCanvas, startGameBtnTransform, startGameBtnPos);
                startGameBtnCanvas.blocksRaycasts = true;

            }
        }
    }

    void MakeButtonInteractable(Button button)
    {
        button.interactable = !button.interactable;
        Image image = button.transform.Find("text").GetComponent<Image>();
        Color color = image.color;
        color.a = color.a == 0.5f ? 1f : 0.5f;
        image.color = color;   
    }
}
