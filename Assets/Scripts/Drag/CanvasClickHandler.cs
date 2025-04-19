using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;


public class CanvasClickHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler,ISaveAndLoadGame
{
    public GameObject prefabToSpawn; // 预制体（2）
    public float riseHeight = 10.0f; // 上升高度
    public float riseDuration = 0.3f; // 上升持续时间
    public LayerMask planeLayer; // Plane 的 Layer
    public float buttonXOffset = 250f; // 水平偏移量（负数向左）
    public float buttonYOffset = 0f;   // 垂直偏移量
    public string planeTag = "Area"; // Plane 的 Tag 名称，可在 Inspector 中修改
    public Material highlightMaterial; // 高亮材质
    public GameObject cancelButtonPrefab; // 取消按钮预制体
    public GameObject applicationButtonPrefab; // 确认按钮预制体
    public GameObject panel;//禁止拖拽卡牌挡板
    public TextMeshProUGUI cardtext;//卡牌名称 

    public Transform[] model=new Transform[20];//卡牌模型

    [Header("Card Info Display")]
    public string cardDetailSceneName = "CardDetailScene";
    public GameObject cardInfoPanelPrefab; // 卡牌信息面板预制体（使用 TMP）
    public float hoverDelay = 1.0f; // 悬浮延迟时间
    public string cardArea = "Area"; // 卡牌区域
    public string cardEffect = "Effect"; // 卡牌效果简述
    public AudioSource house;//建筑声音
    public AudioSource gun;//枪炮声音
    public AudioSource book;//书籍声音

    private GameObject cardInfoPanelInstance; // 卡牌信息面板实例
    private bool isHovering = false; // 是否正在悬浮
    private Coroutine hoverCoroutine; // 悬浮协程

    //-----
    public Vector3 cameraTargetPosition = new Vector3(-30, 10, -10); // 摄像机目标位置
    public Quaternion cameraTargetRotation = Quaternion.Euler(1, 0, 0); // 摄像机目标旋转角度
    public float cameraMoveDuration = 0.5f; // 摄像机移动持续时间

    private GameObject spawnedPrefab; // 生成的预制体（2）
    private CardRangeController rangeController; // 范围控制器

    private bool isDragging = false;
    private bool isPlaced = false;
    private bool isFinalized = false; // 是否已完成放置
    private bool isDestroyed = false;
    private bool isRising = false;
    private bool ischoose=false;//是否可以选择区域

    public bool ifban = false;//是否禁用

    public bool isdragone = false;//是否是抬起的那张卡牌

    public bool ifapplication = false;//是否确认放置

    private Plane targetPlane;
    private Collider[] currentAreaCollider=new Collider[5];
    private Material[] originalMaterial=new Material[5]; // 原始材质
    private GameObject cancelButtonInstance; // 取消按钮实例
    private GameObject cancelButton;
    private GameObject applicationButtonInstance; // 确认按钮实例
    private GameObject applicationButton;
    private HashSet<Collider> affectedAreas = new HashSet<Collider>();
    private Vector3 originalPosition; // 初始位置
    private RectTransform rectTransform;
    private Vector2 originalAnchoredPosition;
    private Vector2 riseStartPosition;
    private Camera mainCamera;
    private Vector3 cameraOriginalPosition; // 摄像机初始位置
    private Quaternion cameraOriginalRotation; // 摄像机初始旋转角度
    private RangeConfirmationButton mabutton;
    private RangeConfirmationButton abutton;
    //private GameObject circle;
    //private GameObject circle0;

    private void Start()
    {
        ischoose = false;
        if (cardInfoPanelPrefab != null && cardInfoPanelInstance == null)
        {
            // 实例化面板
            cardInfoPanelInstance = Instantiate(cardInfoPanelPrefab, FindObjectOfType<Canvas>().transform);
            cardInfoPanelInstance.SetActive(false);

            // 调整面板尺寸
            RectTransform panelRect = cardInfoPanelInstance.GetComponent<RectTransform>();
            panelRect.sizeDelta = new Vector2(100, 75);
        }
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        originalAnchoredPosition = rectTransform.anchoredPosition;
        /*if (GameObject.Find("Circle") != null)
        {
            circle = GameObject.Find("Circle");
        }*/

        foreach(GameObject draghigh in drageffect.Instance.dragtipob)
        {
            if (draghigh != null)
            {
                draghigh.SetActive(false);
            }
        }

        originalPosition = transform.position;
        targetPlane = new Plane(Vector3.up, Vector3.zero); // 假设平面是水平的

        // 记录摄像机的初始位置和旋转角度
        cameraOriginalPosition = Camera.main.transform.position;
        cameraOriginalRotation = Camera.main.transform.rotation;

        //Debug.Log("CanvasClickHandler: Script initialized.");
    }

    // 点击预制体（1）时生成预制体（2）
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isRising && eventData.button == PointerEventData.InputButton.Right)
        {
            riseStartPosition = rectTransform.anchoredPosition;
            StartCoroutine(RisePrefab());
        }
        if (prefabToSpawn != null && spawnedPrefab == null && eventData.button == PointerEventData.InputButton.Right)
        {
            Vector3 spawnPosition = GetMouseWorldPosition();
            spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            rangeController = spawnedPrefab.GetComponent<CardRangeController>();
            isDragging = true;
            isdragone = true;
            Debug.Log("CanvasClickHandler: Prefab (2) spawned at position: " + spawnPosition);

            // 移动摄像机到目标位置和旋转角度
            StartCoroutine(MoveCamera(cameraTargetPosition, cameraTargetRotation));
        }
    }

    // 卡牌动画
    private IEnumerator RisePrefab()
    {
        isRising = true;
        DragCamera.Instance.isClampCamera = false;
        // 在当前位置基础上垂直上升（使用anchoredPosition的Y轴）
        Vector2 targetPosition = riseStartPosition + new Vector2(0, riseHeight);
        float elapsed = 0f;

        while (elapsed < riseDuration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(
                riseStartPosition,
                targetPosition,
                elapsed / riseDuration
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }

    private IEnumerator ResetPosition()
    {
        Vector2 currentPosition = rectTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < riseDuration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(
                currentPosition,
                riseStartPosition, // 回到点击时的起始位置
                elapsed / riseDuration
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = riseStartPosition;
        isRising = false;
        DragCamera.Instance.isClampCamera = true;
    }

    // 拖拽预制体（2）
    public void OnDrag(PointerEventData eventData)
    {
        // 检查是否为右键拖动
        if (isDragging && spawnedPrefab != null && eventData.button == PointerEventData.InputButton.Right)
        {
            AreaTips.MyInstance.FadeOut();
            Vector3 newPosition = GetMouseWorldPosition();
            newPosition.y = spawnedPrefab.transform.position.y; // 保持 Y 轴坐标不变
            spawnedPrefab.transform.position = newPosition;
        }
    }

    // 松开鼠标时放置预制体（2）
    public void OnPointerUp(PointerEventData eventData)
    {
        //if (isDragging && spawnedPrefab != null&& drageffect.Instance.dragone.GetComponent<Cantpushing>()!=null)
        if (isDragging && spawnedPrefab != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, planeLayer))
            {
                //if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Area")&&!drageffect.Instance.dragone.GetComponent<Cantpushing>().ifinmountain)
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Area"))
                {
                    HandCard.MyInstance.targetArea = hit.transform.GetComponent<AreaScript>();
                    AreaTips.MyInstance.FadeIn(HandCard.MyInstance.targetArea.GetComponent<Collider>());
                    if(hit.collider.gameObject.GetComponent<AreaScript>() != null)
                    {
                        if (hit.collider.gameObject.GetComponent<AreaScript>().cards.Count == 3)
                        {
                            Resetmodel();
                            Destroy(spawnedPrefab);
                            spawnedPrefab = null;
                            isDragging = false;
                            ResetPlacement();
                        }
                        else if(hit.collider.gameObject.GetComponent<AreaScript>().cards.Count<3)
                        {
                            // 如果放置在 Area 平面上，完成放置
                            if (eventData.button == PointerEventData.InputButton.Right)
                            {
                                isDragging = false;
                            }
                            isPlaced = true;
                            isFinalized = false;

                            //卡牌作用范围
                            HandCard.MyInstance.applicationArea[0] = hit.transform.GetComponent<AreaScript>();

                            #region 按图索骥
                            switch (cardtext.text)
                            {
                                case "《崇祯历书》":

                                    break;
                                case "地动仪":
                                    for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
                                    {
                                        HandCard.MyInstance.applicationArea[i] = HandCard.MyInstance.allarea[i];
                                    }
                                    break;
                                case "都江堰":
                                    break;
                                case "烽火":
                                    ischoose = true;
                                    Setarea();
                                    break;
                                case "航海术":
                                    break;
                                case "虎蹲炮":
                                    break;
                                case "浑天仪":
                                    break;
                                case "火铳":
                                    ischoose = true;
                                    Setarea();
                                    break;
                                case "火药":
                                    for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
                                    {
                                        HandCard.MyInstance.applicationArea[i] = HandCard.MyInstance.allarea[i];
                                    }
                                    break;
                                case "秦朝军事力学":

                                    break;
                                case "《墨经》《考工记》":
                                    ischoose = true;
                                    Setarea();
                                    break;
                                case "《木经》":
                                    ischoose = true;
                                    Setarea();
                                    break;
                                case "《农桑辑要》":
                                    ischoose = true;
                                    Setarea();
                                    break;
                                case "简单机械组":
                                    break;
                                case "司南":
                                    break;
                                case "唐三彩":
                                    ischoose = true;
                                    Setarea();
                                    break;
                                case "活字印刷术":
                                    break;
                                case "云梯":
                                    break;
                                case "造纸术":
                                    break;
                                case "子母炮":
                                    break;
                            }
                            #endregion

                            highmat();//高亮

                            // 显示取消按钮
                            //if (cancelButtonPrefab != null)
                           // {
                                ShowCancelButton();
                            //}

                            Debug.Log("CanvasClickHandler: Stopped dragging and placed prefab (2) on Area plane.");
                        }
                    }
                }
                else
                {
                    // 如果不在 Area 平面上，销毁预制体（2）
                    Resetmodel();
                    Destroy(spawnedPrefab);
                    spawnedPrefab = null;
                    isDragging = false;
                    ResetPlacement();
                    Debug.Log("CanvasClickHandler: Prefab (2) placed outside Area plane, destroyed.");
                }
            }
            else
            {
                // 如果没有碰到任何物体，销毁预制体（2）
                Resetmodel();
                Destroy(spawnedPrefab);
                spawnedPrefab = null;
                isDragging = false;
                ResetPlacement();
                Debug.Log("CanvasClickHandler: Prefab (2) placed in empty space, destroyed.");
            }
        }
    }

    //卡牌悬浮
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 开始悬停
        isHovering = true;
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine); // 停止之前的协程
        }
        hoverCoroutine = StartCoroutine(ShowCardInfoAfterDelay()); // 启动新的协程
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 结束悬停
        isHovering = false;
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine); // 停止协程
        }
        if (cardInfoPanelInstance != null)
        {
            cardInfoPanelInstance.SetActive(false); // 隐藏面板
        }
    }

    private IEnumerator ShowCardInfoAfterDelay()
    {
        yield return new WaitForSeconds(hoverDelay);

        if (isHovering && cardInfoPanelInstance != null)
        {
            Debug.Log("Displaying card info panel.");
            cardInfoPanelInstance.SetActive(true);

            // 设置卡牌信息面板的位置
            RectTransform rt = cardInfoPanelInstance.GetComponent<RectTransform>();
            rt.anchoredPosition = GetMouseCanvasPosition();

            // 设置卡牌信息
            CardInfoPanel cardInfoPanel = cardInfoPanelInstance.GetComponent<CardInfoPanel>();
            if (cardInfoPanel != null)
            {
                cardInfoPanel.SetCardInfo(cardArea, cardEffect);
            }
        }
    }

    private Vector2 GetMouseCanvasPosition()
    {
        // 将鼠标屏幕坐标转换为 Canvas 坐标
        Vector2 mousePosition = Input.mousePosition;
        Canvas canvas = FindObjectOfType<Canvas>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, mousePosition, canvas.worldCamera, out Vector2 canvasPosition);
        return canvasPosition + new Vector2(50, 50); // 添加偏移量，避免面板紧贴鼠标
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerEnter == gameObject && eventData.button == PointerEventData.InputButton.Left)
        {
            // 进入卡牌介绍界面
            ShowCardDetail();
        }
    }

    private void ShowCardDetail()
    {
        Debug.Log("CanvasClickHandler: Loading card detail scene.");

        // 加载卡牌详情场景
        if (!string.IsNullOrEmpty(cardDetailSceneName))
        {
            SceneManager.LoadScene(cardDetailSceneName);
        }
        else
        {
            Debug.LogError("CardDetailSceneName is not assigned!");
        }
    }

    //--
    public void ResetPlacement()
    {
        if (isRising)
        {
            StartCoroutine(ResetPosition());
        }

        // 销毁已生成的预制体（2）
        if (spawnedPrefab != null)
        {
            Destroy(spawnedPrefab);

            spawnedPrefab = null;
        }

        /*if (circle0 != null)
        {
            Destroy(circle0);
            circle0 = null;
        }*/

        // 重置状态
        isDragging = false;
        isPlaced = false;
        isFinalized = false;
        Debug.Log("CanvasClickHandler: Placement reset.");

        // 重置摄像机位置和旋转角度
        Invoke("resetcamera", 0.1f);

    }

    private void resetcamera()
    {

        if (!GameObject.Find("Sphere(Clone)"))
        {
            StartCoroutine(MoveCamera(cameraOriginalPosition, cameraOriginalRotation));
        }
        CancelInvoke("resetcamera");
    }

    private void returnmat()//恢复材质
    {
        foreach (GameObject one in drageffect.Instance.dragtipob)
        {
            one.SetActive(false);
        }
        // 恢复材质
        for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
        {
            if (HandCard.MyInstance.applicationArea[i] != null)
            {
                if (HandCard.MyInstance.applicationArea[i] == HandCard.MyInstance.allarea[0])
                {
                    foreach (GameObject one in drageffect.Instance.northrender)
                    {
                        one.SetActive(true);
                    }
                }
                else if (HandCard.MyInstance.applicationArea[i] == HandCard.MyInstance.allarea[1])
                {
                    foreach (GameObject one in drageffect.Instance.westrender)
                    {
                        one.SetActive(true);
                    }
                }
                else if (HandCard.MyInstance.applicationArea[i] == HandCard.MyInstance.allarea[2])
                {
                    foreach (GameObject one in drageffect.Instance.centerrender)
                    {
                        one.SetActive(true);
                    }
                }
                else if (HandCard.MyInstance.applicationArea[i] == HandCard.MyInstance.allarea[3])
                {
                    foreach (GameObject one in drageffect.Instance.westsouthrender)
                    {
                        one.SetActive(true);
                    }
                }
                else if (HandCard.MyInstance.applicationArea[i] == HandCard.MyInstance.allarea[4])
                {
                    foreach (GameObject one in drageffect.Instance.southrender)
                    {
                        one.SetActive(true);
                    }
                }
            }
        }
    }

    

    private void highmat()//高亮材质
    {
        for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
        {
            if (HandCard.MyInstance.applicationArea[i] != null)
            {
                if (HandCard.MyInstance.applicationArea[i] == HandCard.MyInstance.allarea[0])
                {
                    foreach(GameObject one in drageffect.Instance.northrender)
                    {
                        one.SetActive(false);
                    }
                }
                else if (HandCard.MyInstance.applicationArea[i] == HandCard.MyInstance.allarea[1])
                {
                    foreach (GameObject one in drageffect.Instance.westrender)
                    {
                        one.SetActive(false);
                    }
                }
                else if (HandCard.MyInstance.applicationArea[i] == HandCard.MyInstance.allarea[2])
                {
                    foreach (GameObject one in drageffect.Instance.centerrender)
                    {
                        one.SetActive(false);
                    }
                }
                else if (HandCard.MyInstance.applicationArea[i] == HandCard.MyInstance.allarea[3])
                {
                    foreach (GameObject one in drageffect.Instance.westsouthrender)
                    {
                        one.SetActive(false);
                    }
                }
                else if (HandCard.MyInstance.applicationArea[i] == HandCard.MyInstance.allarea[4])
                {
                    foreach (GameObject one in drageffect.Instance.southrender)
                    {
                        one.SetActive(false);
                    }
                }
            }
        }
    }

    private void Ban()
    {
        ifban = true;
    }

    private void Resetban()
    {
        ifban = false;
    }

    private void Update()
    {
        /*foreach(Transform models in model)
        {
            models.gameObject.SetActive(true);
        }*/
        #region 换上新物体作为提示
        if (drageffect.Instance.state == drageffect.State.choose)
        {
            foreach(AreaScript highone in HandCard.MyInstance.applicationArea)
            {
                if (highone == HandCard.MyInstance.allarea[0])
                {
                    drageffect.Instance.dragtipob[0].SetActive(true);
                }
                else if (highone == HandCard.MyInstance.allarea[1])
                {
                    drageffect.Instance.dragtipob[1].SetActive(true);
                }
                else if (highone == HandCard.MyInstance.allarea[2])
                {
                    drageffect.Instance.dragtipob[2].SetActive(true);
                }
                else if (highone == HandCard.MyInstance.allarea[3])
                {
                    drageffect.Instance.dragtipob[3].SetActive(true);
                }
                else if (highone == HandCard.MyInstance.allarea[4])
                {
                    drageffect.Instance.dragtipob[4].SetActive(true);
                }
            }
        }
        else 
        {
            foreach (GameObject one in drageffect.Instance.dragtipob)
            {
               one.SetActive(false);
            }
        }
        #endregion
        if (isPlaced)
        {
            // 新逻辑：通过拖拽直接处理材质
            HandleDragSelection();
        }

        if (ifban)
        {
            gameObject.GetComponent<Image>().color = Color.gray;//变灰
            panel.gameObject.SetActive(true);//禁用
        }
        else
        {
            panel.gameObject.SetActive(false);
            gameObject.GetComponent<Image>().color = Color.white;
        }

        //放一张卡牌后全体禁用
        if (drageffect.Instance.allban&&!ifban)
        {
            Ban();
        }

        if (mabutton != null)
        {
            if (mabutton.ifbutton)
            {
                returnmat();
                Destroy(cancelButton);
                Destroy(applicationButton);
                mabutton.ifbutton = false;
            }
        }

        //选中一张卡牌的时候其他卡牌变灰且禁用
        if(drageffect.Instance.state!=drageffect.State.normal && !ifban)
        {
            if (!isDragging)
            {
                Ban();
            }
        }
        else if(drageffect.Instance.state == drageffect.State.normal && ifban&&!drageffect.Instance.allban)
        {
            Resetban();
        }

        //没有卡牌时无法选中
        if (cardtext.text == "" && !ifban)
        {
            Ban();
        }

        if (abutton != null)
        {
            if (abutton.ifbutton)
            {
                returnmat();
                Destroy(cancelButton);
                Destroy(applicationButton);
                abutton.ifbutton = false;
            }
        }

        if (drageffect.Instance.state == drageffect.State.choose&&ischoose)
        {
            #region 按W切换作用区域
            if (Input.GetKeyDown(KeyCode.W)) 
            {
                if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[1])//西
                {
                    if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[2])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[3];

                        highmat();
                    }
                    else if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[3])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[2];
                        highmat();
                    }
                }
                else if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[2])//中
                {
                    if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[0])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[4];
                        highmat();
                    }
                    else if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[1])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[0];
                        highmat();
                    }
                    else if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[4])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[1];
                        highmat();
                    }
                }
                else if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[3])//西南
                {
                    if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[1])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[4];
                        highmat();
                    }
                    else if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[4])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[1];
                        highmat();
                    }
                }
                else if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[4])//南
                {
                    if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[2])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[3];
                        highmat();
                    }
                    else if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[3])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[2];
                        highmat();
                    }
                }
            }
            #endregion
        }

        if (drageffect.Instance.state == drageffect.State.drag&& GameObject.Find("Sphere(Clone)")!=null&&isDragging)
        {
            #region 按图索骥
            switch (cardtext.text)
            {
                case "《崇祯历书》":
                    model[0].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[0].gameObject;
                    model[0].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y+10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "地动仪":
                    model[1].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[1].gameObject;
                    model[1].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x+15, GameObject.Find("Sphere(Clone)").transform.position.y + 5, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "都江堰":
                    model[2].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[2].gameObject;
                    model[2].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 8, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "烽火":
                    model[3].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[3].gameObject;
                    model[3].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x+1, GameObject.Find("Sphere(Clone)").transform.position.y + 5, GameObject.Find("Sphere(Clone)").transform.position.z-2);
                    break;
                case "航海术":
                    model[4].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[4].gameObject;
                    model[4].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "虎蹲炮":
                    model[5].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[5].gameObject;
                    model[5].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x+5, GameObject.Find("Sphere(Clone)").transform.position.y + 14, GameObject.Find("Sphere(Clone)").transform.position.z+8);
                    break;
                case "浑天仪":
                    model[6].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[6].gameObject;
                    model[6].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x-34, GameObject.Find("Sphere(Clone)").transform.position.y + 15, GameObject.Find("Sphere(Clone)").transform.position.z+32);
                    break;
                case "火铳":
                    model[7].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[7].gameObject;
                    model[7].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "火药":
                    model[8].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[8].gameObject;
                    model[8].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x-7, GameObject.Find("Sphere(Clone)").transform.position.y + 30, GameObject.Find("Sphere(Clone)").transform.position.z+37);
                    break;
                case "秦朝军事力学":
                    model[9].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[9].gameObject;
                    model[9].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "《墨经》《考工记》":
                    model[10].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[10].gameObject;
                    model[10].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "《木经》":
                    model[11].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[11].gameObject;
                    model[11].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "《农桑辑要》":
                    model[12].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[12].gameObject;
                    model[12].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "简单机械组":
                    model[13].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[13].gameObject;
                    model[13].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "司南":
                    model[14].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[14].gameObject;
                    model[14].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x+10, GameObject.Find("Sphere(Clone)").transform.position.y + 5, GameObject.Find("Sphere(Clone)").transform.position.z-14);
                    break;
                case "唐三彩":
                    model[15].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[15].gameObject;
                    model[15].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 20, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "活字印刷术":
                    model[16].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[16].gameObject;
                    model[16].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x-5, GameObject.Find("Sphere(Clone)").transform.position.y + 5, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "云梯":
                    model[17].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[17].gameObject;
                    model[17].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "造纸术":
                    model[18].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[18].gameObject;
                    model[18].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 5, GameObject.Find("Sphere(Clone)").transform.position.z+10);
                    break;
                case "子母炮":
                    model[19].gameObject.SetActive(true);
                    drageffect.Instance.dragone = model[19].gameObject;
                    model[19].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 15, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
            }

            drageffect.Instance.hammer.transform.position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x+25, GameObject.Find("Sphere(Clone)").transform.position.y + 8, GameObject.Find("Sphere(Clone)").transform.position.z+5);
            drageffect.Instance.allsmoke.gameObject.transform.position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x + 5, GameObject.Find("Sphere(Clone)").transform.position.y, GameObject.Find("Sphere(Clone)").transform.position.z+110);
            #endregion
        }
    }

    //相邻目标初始放置时默认区域
    private void Setarea()
    {
        if (HandCard.MyInstance.targetArea != null)
        {
            if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[0])//北
            {
                HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[2];
                ischoose = false;
            }
            else if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[1])//西
            {
                HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[2];
            }
            else if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[2])//中
            {
                HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[1];
            }
            else if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[3])//西南
            {
                HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[4];
            }
            else if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[4])//南
            {
                HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[3];
            }
        }
    }

    // 材质相关逻辑
    private void HandleDragSelection()
    {
        // 此处逻辑已由DragMaterialController处理
    }

    // 取消按钮相关逻辑
    private Vector2 GetButtonCanvasPosition()
    {
        Canvas canvas = GameObject.Find("Canvas1").GetComponent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // 将鼠标屏幕坐标转换为Canvas本地坐标
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            canvas.worldCamera,
            out localPoint
        );

        // 仅应用X轴偏移，保持Y轴原始位置
        return new Vector2(
            localPoint.x + buttonXOffset, // 只修改X轴
            localPoint.y                  // Y轴保持原样
        );
    }

    private void ShowCancelButton()
    {
        if (cancelButton == null)
        {
            //cancelButton = Instantiate(cancelButtonPrefab, FindObjectOfType<Canvas>().transform); 
                cancelButton = Instantiate(cancelButtonPrefab, GameObject.Find("Canvas1").GetComponent<Canvas>().transform);
            RectTransform rt = cancelButton.GetComponent<RectTransform>();
            //applicationButton = Instantiate(applicationButtonPrefab, FindObjectOfType<Canvas>().transform);
            applicationButton = Instantiate(applicationButtonPrefab, GameObject.Find("Canvas1").GetComponent<Canvas>().transform);
            RectTransform rt1 = applicationButton.GetComponent<RectTransform>();

            // 设置锚点为左下角（保证Y轴基准一致）
            rt.anchorMin = new Vector2(0, 0.5f);
            rt.anchorMax = new Vector2(0, 0.5f);
            rt.pivot = new Vector2(-5.8f, 1.1f); // 轴心对齐左下角

            rt.anchoredPosition = GetButtonCanvasPosition();

            rt1.anchorMin = new Vector2(0, 0.5f);
            rt1.anchorMax = new Vector2(0, 0.5f);
            rt1.pivot = new Vector2(-5.28f, 0.3f); // 轴心对齐左下角

            rt1.anchoredPosition = GetButtonCanvasPosition();

            cancelButton.GetComponent<Button>().onClick.AddListener(CancelPlacement);
            applicationButton.GetComponent<Button>().onClick.AddListener(ApplicationPlacement);
            mabutton = cancelButton.GetComponent<RangeConfirmationButton>();
            abutton = applicationButton.GetComponent<RangeConfirmationButton>();
            cancelButtonInstance = cancelButton;
            applicationButtonInstance = applicationButton;
        }
        cancelButton.SetActive(true);
        applicationButton.SetActive(true);
    }

    private void HideCancelButton()
    {
        if (cancelButton != null || applicationButton != null)
        {
            cancelButton.SetActive(false);
            applicationButton.SetActive(false);
        }
    }
    // 完成放置并销毁预制体（1）
    public void FinalizePlacement()
    {
        if (isDestroyed) return;

        // 立即标记防止重复调用
        isDestroyed = true;

        // 启动异步完成流程
        StartCoroutine(FinalizationProcess());
    }

    private IEnumerator FinalizationProcess()
    {
        // 第一阶段：摄像机复位
        yield return StartCoroutine(MoveCamera(cameraOriginalPosition, cameraOriginalRotation));

        // 第二阶段：执行清理操作
        ExecuteCleanup();

        // 第三阶段：延迟销毁确保协程完成
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    private void ExecuteCleanup()
    {
        returnmat();

        // 清理UI
        if (cancelButtonInstance)
        {
            Destroy(cancelButtonInstance);
            Destroy(applicationButtonInstance);
            cancelButtonInstance = null;
            applicationButtonInstance = null;
        }

        // 重置状态
        spawnedPrefab = null;
        isPlaced = false;
        isFinalized = true;

        // 清理区域记录
        foreach (var area in affectedAreas)
        {
            //ResetMaterial(area);

            returnmat();
        }
        affectedAreas.Clear();
    }

    private void ResetMaterial(Collider collider)
    {
        Renderer[] renderer = new Renderer[5];
        for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
        {
            if (collider != null)
            {
                renderer[i] = collider.GetComponent<Renderer>();
                if (renderer[i] != null)
                {
                    renderer[i].material = originalMaterial[i];
                }
            }
        }
    }

    // 获取鼠标在世界坐标中的位置
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (targetPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    // 取消放置
    public void CancelPlacement()
    {
        ischoose = false;
        Resetmodel();
        returnmat();
        HandCard.MyInstance.targetArea = null;
        for(int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++) 
        {
            if (HandCard.MyInstance.applicationArea[i] != null)
            {
                HandCard.MyInstance.applicationArea[i] = null;
            }
        }
        AreaTips.MyInstance.FadeOut();
        ResetPlacement();
        Debug.Log("CanvasClickHandler: Placement canceled.");
    }

    //确认放置
    public void ApplicationPlacement()
    {
        drageffect.Instance.dragone.SetActive(false);
            //放置声音
            #region 放置声音
            if (isdragone)
            {
            Invoke("HousePlay", 0.55f);
                isdragone = false;
            }
            #endregion
            ischoose = false;
            drageffect.Instance.Allban();
            Invoke("SmokeStart", -0.1f);
            drageffect.Instance.hammer.SetActive(true);
            drageffect.Instance.hammerani.SetBool("ifhit",true);
            if (ifapplication == false)
            {
                Invoke("resettargetcard", 0.5f);
                ifapplication = true;
                ResetPlacement();
            }
    }

    void SmokeStart()
    {
        foreach (var smoke in drageffect.Instance.smoke)
        {
            smoke.Play();
        }
    }

    void HousePlay()
    {
        CancelInvoke("HousePlay");
        house.Play();
    }

    //复原目标区域
    void resettargetcard()
    {
        HandCard.MyInstance.targetArea = null;
        for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
        {
            if (HandCard.MyInstance.applicationArea[i] != null)
            {
                HandCard.MyInstance.applicationArea[i] = null;
            }
        }
    }

    // 平滑移动摄像机
    private IEnumerator MoveCamera(Vector3 targetPosition, Quaternion targetRotation)
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main camera is null.");
            yield break;
        }

        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;
        float elapsed = 0f;

        while (elapsed < cameraMoveDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / cameraMoveDuration);
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / cameraMoveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;
        //DragCamera.Instance.isClampCameraY = true;
    }

    public void Resetmodel()
    {
        #region 按图索骥--复原模型
        if (isdragone)
        {
            switch (cardtext.text)
            {
                case "《崇祯历书》":
                    model[0].position = new Vector3(124, -500, -82);
                    model[0].gameObject.SetActive(false);
                    drageffect.Instance.dragone=null;
                    break;
                case "地动仪":
                    model[1].position = new Vector3(124, -500, -82);
                    model[1].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "都江堰":
                    model[2].position = new Vector3(124, -500, -82);
                    model[2].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "烽火":
                    model[3].position = new Vector3(124, -500, -82);
                    model[3].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "航海术":
                    model[4].position = new Vector3(124, -500, -82);
                    model[4].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "虎蹲炮":
                    model[5].position = new Vector3(124, -500, -82);
                    model[5].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "浑天仪":
                    model[6].position = new Vector3(124, -500, -82);
                    model[6].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "火铳":
                    model[7].position = new Vector3(124, -500, -82);
                    model[7].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "火药":
                    model[8].position = new Vector3(124, -500, -82);
                    model[8].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "秦朝军事力学":
                    model[9].position = new Vector3(124, -500, -82);
                    model[9].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "《墨经》《考工记》":
                    model[10].position = new Vector3(124, -500, -82);
                    model[10].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "《木经》":
                    model[11].position = new Vector3(124, -500, -82);
                    model[11].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "《农桑辑要》":
                    model[12].position = new Vector3(124, -500, -82);
                    model[12].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "简单机械组":
                    model[13].position = new Vector3(124, -500, -82);
                    model[13].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "司南":
                    model[14].position = new Vector3(124, -500, -82);
                    model[14].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "唐三彩":
                    model[15].position = new Vector3(124, -500, -82);
                    model[15].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "活字印刷术":
                    model[16].position = new Vector3(124, -500, -82);
                    model[16].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "云梯":
                    model[17].position = new Vector3(124, -500, -82);
                    model[17].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "造纸术":
                    model[18].position = new Vector3(124, -500, -82);
                    model[18].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
                case "子母炮":
                    model[19].position = new Vector3(124, -500, -82);
                    model[19].gameObject.SetActive(false);
                    drageffect.Instance.dragone = null;
                    break;
            }
            isdragone = false;
        }
        #endregion
    }

    public void Save(ref GameData gameData)
    {
        gameData.ifban = ifban;
        gameData.allban = drageffect.Instance.allban;
    }

    public void Load(GameData gameData)
    {
        ifban = gameData.ifban;
        drageffect.Instance.allban = gameData.allban;
    }
}