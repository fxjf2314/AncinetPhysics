using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class CanvasClickHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
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

    public Vector3 cameraTargetPosition = new Vector3(0, 10, -10); // 摄像机目标位置
    public Quaternion cameraTargetRotation = Quaternion.Euler(1, 0, 0); // 摄像机目标旋转角度
    public float cameraMoveDuration = 0.5f; // 摄像机移动持续时间

    private GameObject spawnedPrefab; // 生成的预制体（2）
    private CardRangeController rangeController; // 范围控制器

    private bool isDragging = false;
    private bool isPlaced = false;
    private bool isFinalized = false; // 是否已完成放置
    private bool isDestroyed = false;
    private bool isRising = false;

    private Plane targetPlane;
    private Collider currentAreaCollider;
    private Material originalMaterial; // 原始材质
    private GameObject cancelButtonInstance; // 取消按钮实例
    private GameObject cancelButton;
    private HashSet<Collider> affectedAreas = new HashSet<Collider>();
    private Vector3 originalPosition; // 初始位置
    private RectTransform rectTransform;
    private Vector2 originalAnchoredPosition;
    private Vector2 riseStartPosition;
    private Camera mainCamera;
    private Vector3 cameraOriginalPosition; // 摄像机初始位置
    private Quaternion cameraOriginalRotation; // 摄像机初始旋转角度

    private void Start()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        originalAnchoredPosition = rectTransform.anchoredPosition;

        originalPosition = transform.position;
        targetPlane = new Plane(Vector3.up, Vector3.zero); // 假设平面是水平的

        // 记录摄像机的初始位置和旋转角度
        cameraOriginalPosition = Camera.main.transform.position;
        cameraOriginalRotation = Camera.main.transform.rotation;

        Debug.Log("CanvasClickHandler: Script initialized.");
    }

    // 点击预制体（1）时生成预制体（2）
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isRising)
        {
            riseStartPosition = rectTransform.anchoredPosition;
            StartCoroutine(RisePrefab());
        }
        if (prefabToSpawn != null && spawnedPrefab == null)
        {
            Vector3 spawnPosition = GetMouseWorldPosition();
            spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            rangeController = spawnedPrefab.GetComponent<CardRangeController>();
            isDragging = true;
            Debug.Log("CanvasClickHandler: Prefab (2) spawned at position: " + spawnPosition);

            // 移动摄像机到目标位置和旋转角度
            StartCoroutine(MoveCamera(cameraTargetPosition, cameraTargetRotation));
        }
    }

    // 卡牌动画
    private IEnumerator RisePrefab()
    {
        isRising = true;

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
    }

    // 拖拽预制体（2）
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && spawnedPrefab != null)
        {
            Vector3 newPosition = GetMouseWorldPosition();
            newPosition.y = spawnedPrefab.transform.position.y; // 保持 Y 轴坐标不变
            spawnedPrefab.transform.position = newPosition;
        }
    }

    // 松开鼠标时放置预制体（2）
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging && spawnedPrefab != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, planeLayer))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Area"))
                {
                    // 如果放置在 Area 平面上，完成放置
                    isDragging = false;
                    isPlaced = true;
                    isFinalized = false;

                    currentAreaCollider = hit.collider;
                    Renderer renderer = currentAreaCollider.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        originalMaterial = renderer.material;
                        renderer.material = highlightMaterial; // 设置高亮材质
                    }

                    // 显示取消按钮
                    if (cancelButtonPrefab != null)
                    {
                        ShowCancelButton();
                    }

                    Debug.Log("CanvasClickHandler: Stopped dragging and placed prefab (2) on Area plane.");
                }
                else
                {
                    // 如果不在 Area 平面上，销毁预制体（2）
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
                Destroy(spawnedPrefab);
                spawnedPrefab = null;
                isDragging = false;
                ResetPlacement();
                Debug.Log("CanvasClickHandler: Prefab (2) placed in empty space, destroyed.");
            }
        }
    }

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

        // 重置状态
        isDragging = false;
        isPlaced = false;
        isFinalized = false;
        Debug.Log("CanvasClickHandler: Placement reset.");

        // 重置摄像机位置和旋转角度
        StartCoroutine(MoveCamera(cameraOriginalPosition, cameraOriginalRotation));
    }

    private void Update()
    {
        if (isPlaced)
        {
            // 新逻辑：通过拖拽直接处理材质
            HandleDragSelection();
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
        Canvas canvas = FindObjectOfType<Canvas>();
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
            cancelButton = Instantiate(cancelButtonPrefab, FindObjectOfType<Canvas>().transform);
            RectTransform rt = cancelButton.GetComponent<RectTransform>();

            // 设置锚点为左下角（保证Y轴基准一致）
            rt.anchorMin = new Vector2(0, 0.5f);
            rt.anchorMax = new Vector2(0, 0.5f);
            rt.pivot = new Vector2(0, 0.5f); // 轴心对齐左下角

            rt.anchoredPosition = GetButtonCanvasPosition();

            cancelButton.GetComponent<Button>().onClick.AddListener(CancelPlacement);
            cancelButtonInstance = cancelButton;
        }
        cancelButton.SetActive(true);
    }

    private void HideCancelButton()
    {
        if (cancelButton != null)
        {
            cancelButton.SetActive(false);
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
        // 恢复材质
        if (currentAreaCollider != null)
        {
            var renderer = currentAreaCollider.GetComponent<Renderer>();
            if (renderer) renderer.material = originalMaterial;
        }

        // 清理UI
        if (cancelButtonInstance)
        {
            Destroy(cancelButtonInstance);
            cancelButtonInstance = null;
        }

        // 重置状态
        spawnedPrefab = null;
        isPlaced = false;
        isFinalized = true;

        // 清理区域记录
        foreach (var area in affectedAreas)
        {
            ResetMaterial(area);
        }
        affectedAreas.Clear();
    }
    private void ResetMaterial(Collider collider)
    {
        if (collider != null)
        {
            Renderer renderer = collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = originalMaterial;
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
        ResetPlacement();
        Debug.Log("CanvasClickHandler: Placement canceled.");
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
    }
}