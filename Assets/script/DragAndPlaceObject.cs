using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
public class DragAndPlaceIn3D : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Settings")]
    public GameObject draggableObjectPrefab;
    public LayerMask placementLayer;
    public GameObject cancelButtonPrefab;

    [Header("Range Selection")]
    public GameObject rangeIndicatorPrefab;
    public float gridSize = 1f;

    // 状态变量
    private GameObject currentObject;
    private GameObject cancelButton;
    private bool isDragging = false;
    private bool isPlaced = false;
    private bool isSelectingRange = false;
    private GameObject currentRangeIndicator;
    private GameObject currentPlane; // 修改为 GameObject 类型

    private Color debugColor = new Color(1, 0.5f, 0);
    // 公共属性供按钮访问
    public bool IsPlacedAndNotFinalized => isPlaced && !isSelectingRange;

    void Update()
    {
        DebugState();
        if (isSelectingRange)
        {
            UpdateRangeSelection();
        }
    }
    private bool GetMousePositionOnPlane(out Vector3 position, bool allowMultiPlane = false)
    {
        position = Vector3.zero;

        // 获取主摄像机引用
        Camera mainCam = Camera.main;
        if (!mainCam)
        {
            Debug.LogError("主摄像机未找到！");
            return false;
        }

        // 生成射线
        Vector3 mousePos = Input.mousePosition;
        Ray ray = mainCam.ScreenPointToRay(mousePos);

        // 可视化射线（场景视图中可见）
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.magenta, 2f);
        Debug.Log($"射线生成于：[摄像机位置]{mainCam.transform.position} " +
                 $"方向：[{ray.direction}] " +
                 $"屏幕坐标：[{mousePos}]");

        // 射线投射检测
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            Debug.Log($"射线命中：[物体]{hit.collider.name} " +
                     $"位置：[{hit.point}] " +
                     $"层级：[{LayerMask.LayerToName(hit.collider.gameObject.layer)}]");

            if (allowMultiPlane)
            {
                position = SnapToGrid(hit.point);
                return true;
            }

            if (hit.collider.gameObject == currentPlane)
            {
                position = SnapToGrid(hit.point);
                Debug.Log($"有效平面位置：[{position}]");
                return true;
            }

            Debug.LogWarning($"当前平面不匹配：目标平面[{currentPlane?.name}] 实际命中[{hit.collider.name}]");
            return false;
        }

        Debug.LogWarning($"未命中任何平面，当前层级掩码值：{placementLayer.value}");
        return false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("检测到鼠标按下事件");

        if (!isPlaced && !isDragging)
        {
            StartDragging();
        }
        else if (IsClickValid())
        {
            StartRangeSelection();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && !isPlaced)
        {
            UpdateObjectPosition();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("[PointerUp] 松开事件");

        if (isDragging && !isPlaced)
        {
            PlaceObject();
        }
        else if (isSelectingRange)
        {
            FinalizePlacement();
        }
    }

    private void StartDragging()
    {
        try
        {
            currentObject = Instantiate(draggableObjectPrefab);
            Debug.Log("实例化新物体: " + currentObject.name);

            // 自动添加必要组件
            if (!currentObject.GetComponent<Collider>())
            {
                currentObject.AddComponent<BoxCollider>();
                Debug.Log("已自动添加碰撞体");
            }

            isDragging = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("实例化失败: " + e.Message);
        }
    }
    private void UpdateObjectPosition()
    {
        if (GetMousePositionOnPlane(out Vector3 pos))
        {
            Debug.Log($"更新物体位置到：[{pos}]");
            currentObject.transform.position = pos;
        }
        else
        {
            Debug.LogWarning("无法获取有效平面位置，请检查：\n" +
                            "1. 平面碰撞体是否存在\n" +
                            "2. 是否处于正确层级\n" +
                            "3. 主摄像机是否正确设置");
        }
    }


    private void PlaceObject()
    {
        isDragging = false;
        isPlaced = true;
        Debug.Log($"<color=yellow>物体放置完成：{currentObject.transform.position}</color>");

        // 显示平面法线方向
        StartCoroutine(ShowPlaneNormal());
        ShowCancelButton();
    }
    IEnumerator ShowPlaneNormal()
    {
        Vector3 normal = currentPlane.transform.up;
        for (int i = 0; i < 10; i++)
        {
            Debug.DrawRay(currentPlane.transform.position, normal * 2, Color.red, 1f);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void StartRangeSelection()
    {
        isSelectingRange = true;
        // 在物体位置生成初始指示器
        if (currentObject != null)
        {
            currentRangeIndicator = CreateIndicator(currentObject.transform.position);
        }
    }

    private void UpdateRangeSelection()
    {
        if (GetMousePositionOnPlane(out Vector3 pos, true))
        {
            pos = SnapToGrid(pos);

            if (currentRangeIndicator == null)
            {
                currentRangeIndicator = CreateIndicator(pos);
            }
            else if (Vector3.Distance(pos, currentRangeIndicator.transform.position) > gridSize * 0.5f)
            {
                Destroy(currentRangeIndicator);
                currentRangeIndicator = CreateIndicator(pos);
            }
        }
    }

    private void FinalizePlacement()
    {
        Destroy(currentRangeIndicator);
        isSelectingRange = false;
        isPlaced = false;
        DestroyImmediate(currentObject, true);
        DestroyCancelButton();
    }

    private GameObject CreateIndicator(Vector3 pos)
    {
        return Instantiate(rangeIndicatorPrefab, pos, Quaternion.identity);
    }


    private GameObject GetCurrentPlane()
    {
        Ray ray = Camera.main.ScreenPointToRay(currentObject.transform.position);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            return hit.collider.gameObject; // 返回 GameObject
        }
        return null;
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x / gridSize) * gridSize,
            position.y,
            Mathf.Round(position.z / gridSize) * gridSize
        );
    }

    private bool IsClickValid()
    {
        return IsPlacedAndNotFinalized &&
               GetMousePositionOnPlane(out _) &&
               currentPlane == GetCurrentPlane(); // 直接比较 GameObject
    }

    private void ShowCancelButton()
    {
        if (cancelButton == null)
        {
            cancelButton = Instantiate(cancelButtonPrefab, FindObjectOfType<Canvas>().transform);
            cancelButton.GetComponent<RangeConfirmationButton>().placementSystem = this;
        }
        cancelButton.SetActive(true);
    }

    public void DestroyCancelButton()
    {
        if (cancelButton != null)
        {
            Destroy(cancelButton);
        }
    }

    public void CancelPlacement()
    {
        Destroy(currentObject);
        DestroyCancelButton();
        ResetState();
    }

    private void ResetState()
    {
        isDragging = false;
        isPlaced = false;
        isSelectingRange = false;
        currentObject = null;
    }
    private void DebugState()
    {
        string state = $"状态：拖拽中[{isDragging}] 已放置[{isPlaced}] 选择范围[{isSelectingRange}]";
        Debug.DrawLine(Vector3.zero, new Vector3(0, 1, 0), isDragging ? Color.green : Color.red);
    }
    
}