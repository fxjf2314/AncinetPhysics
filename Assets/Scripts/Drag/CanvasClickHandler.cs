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
    public float yOffset = 1.0f; // Y 轴偏移量，可在 Inspector 中修改
    public float buttonXOffset = 250f; // 水平偏移量（负数向左）
    public float buttonYOffset = 0f;   // 垂直偏移量
    public string planeTag = "Area"; // Plane 的 Tag 名称，可在 Inspector 中修改
    public Material highlightMaterial; // 高亮材质
    public GameObject cancelButtonPrefab; // 取消按钮预制体

    private GameObject spawnedPrefab; // 生成的预制体（2）

    private bool isDragging = false;
    private bool isPlaced = false;
    private bool isFinalized = false; // 是否已完成放置
    private bool isDestroyed = false;
    private bool isRising = false;

    private Plane targetPlane;
    private int currentRangeIndicatorCount = 0;
    private List<GameObject> rangeIndicators = new List<GameObject>(); // 存储所有生成的 RangeIndicator
    private HashSet<Collider> clickedPlanes = new HashSet<Collider>(); // 记录已被点击的 Plane
    private Renderer currentRenderer; // 当前高亮的 Renderer
    private Material originalMaterial; // 原始材质
    private GameObject cancelButtonInstance; // 取消按钮实例
    private GameObject cancelButton;
    private HashSet<Collider> affectedAreas = new HashSet<Collider>();
    private Vector3 originalPosition; // 初始位置
    private RectTransform rectTransform;
    private Vector2 originalAnchoredPosition;
    private Vector2 riseStartPosition;
    private Vector3 originalWorldPosition;

    // 是否已放置且未完成
    public bool IsPlacedAndNotFinalized => isPlaced && !isFinalized;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalAnchoredPosition = rectTransform.anchoredPosition;
        
        originalPosition = transform.position;
        targetPlane = new Plane(Vector3.up, Vector3.zero); // 假设平面是水平的
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
            spawnPosition.y += yOffset; // 在鼠标 Y 轴位置的基础上增加偏移量
            spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            isDragging = true;
            Debug.Log("CanvasClickHandler: Prefab (2) spawned at position: " + spawnPosition);
        }
    }
    //卡牌动画
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
            //Debug.Log("CanvasClickHandler: Dragging prefab (2) to position: " + newPosition);
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

        // 销毁所有已生成的范围指示器
        foreach (var indicator in rangeIndicators)
        {
            if (indicator != null)
            {
                Destroy(indicator);
            }
        }
        rangeIndicators.Clear();

        // 重置状态
        isDragging = false;
        isPlaced = false;
        isFinalized = false;
        currentRangeIndicatorCount = 0;
        clickedPlanes.Clear();

        // 恢复 Area 的材质
        if (currentRenderer != null)
        {
            currentRenderer.material = originalMaterial;
            currentRenderer = null;
        }

        Debug.Log("CanvasClickHandler: Placement reset.");
    }

    private void Update()
    {
        if (isPlaced)
        {
            // 新逻辑：通过拖拽直接处理材质
            HandleDragSelection();
        }
        HandleAreaHighlight(); // 处理 Area 的高亮逻辑
    }



    //材质相关逻辑
    private void HandleDragSelection()
    {
        // 此处逻辑已由DragMaterialController处理
    }

    //取消按钮相关逻辑
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
    
    // 处理 Area 的高亮逻辑
    private void HandleAreaHighlight()
    {

        if (isDragging || isPlaced) return;


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Area"))
            {
                var dragController = FindObjectOfType<DragMaterialController>();
                if (dragController != null && dragController.affectedAreas.Contains(hit.collider))
                    return;
                // 高亮 Area
                if (currentRenderer != null && currentRenderer != hit.collider.GetComponent<Renderer>())
                {
                    currentRenderer.material = originalMaterial;
                }

                currentRenderer = hit.collider.GetComponent<Renderer>();
                originalMaterial = currentRenderer.material;
                currentRenderer.material = highlightMaterial;
            }
            else
            {
                // 恢复 Area 的材质
                if (currentRenderer != null)
                {
                    currentRenderer.material = originalMaterial;
                    currentRenderer = null;
                }
            }
        }
        else
        {
            // 恢复 Area 的材质
            if (currentRenderer != null)
            {
                currentRenderer.material = originalMaterial;
                currentRenderer = null;
            }
        }
    }

    // 完成放置并销毁预制体（1）
    public void FinalizePlacement()
    {
        if (isDestroyed) return;

        // 销毁关联的DragMaterialController
        var dragController = FindObjectOfType<DragMaterialController>();
        if (dragController != null)
        {
            dragController.ConfirmSelection();
            Destroy(dragController);        // 如果直接挂载在spawnedPrefab上
        }

        Debug.Log("CanvasClickHandler: Placement finalized.");

        // 销毁预制体（1）
        if (gameObject != null)
        {
            isDestroyed = true;
            Destroy(gameObject);
        }

        // 销毁取消按钮
        if (cancelButtonInstance != null)
        {
            HideCancelButton();
            Destroy(cancelButtonInstance);
        }

        // 重置状态
        spawnedPrefab = null;
        isPlaced = false;
        isFinalized = true;
        currentRangeIndicatorCount = 0;

        // 清空已点击的 Plane 记录
        clickedPlanes.Clear();

        // 恢复所有 Area 的材质
        foreach (Collider area in affectedAreas)
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
}