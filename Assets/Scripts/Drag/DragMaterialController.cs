using UnityEngine;
using System.Collections.Generic;

public class DragMaterialController : MonoBehaviour
{
    [Header("材质设置")]
    public Material highlightMaterial; // 临时材质
    public LayerMask areaLayer;         // Area层级
    public int maxAffectedAreas = 3;    // 最大影响数量

    private Dictionary<Collider, Material> originalMaterials = new Dictionary<Collider, Material>(); // 记录每个Area的原始材质
    public HashSet<Collider> affectedAreas = new HashSet<Collider>();
    private HashSet<Collider> pendingAreas = new HashSet<Collider>();
    private bool isDragging = false;
    private CanvasClickHandler canvasClickHandler;
    private Collider lastHoveredCollider;
    private Collider lastHoveredArea;
    private void Awake()
    {
        // 获取 CanvasClickHandler 实例
        canvasClickHandler = FindObjectOfType<CanvasClickHandler>();
    }

    private void Update()
    {
        if (isDragging)
        {
            HandleDragSelection();
        }
    }

    private void OnMouseDown()
    {
        if (IsPlacedCorrectly())
        {
            isDragging = true;
            Debug.Log("开始区域选择");
            Cursor.visible = false; // 隐藏光标增强体验
        }
    }

    private void OnMouseUp()
    {
        if (isDragging)
        {
            affectedAreas.UnionWith(pendingAreas);
            pendingAreas.Clear();

            FinalizeSelection();
            ResetDragState();
        }
    }

    private void HandleDragSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 清除上次悬停区域的临时效果
       /* if (lastHoveredArea != null && !affectedAreas.Contains(lastHoveredArea))
        {
            ResetMaterial(lastHoveredArea);
            pendingAreas.Remove(lastHoveredArea);
        }*/
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, areaLayer))
        {
            Collider currentCollider = hit.collider;
            lastHoveredArea = currentCollider;

            // 防止重复选择检查
            if (affectedAreas.Contains(currentCollider)) return;

            if (affectedAreas.Count < maxAffectedAreas)
            {
                // 临时应用材质到悬停区域
                if (!pendingAreas.Contains(currentCollider))
                {
                    ApplyMaterial(currentCollider);
                    pendingAreas.Add(currentCollider);
                }
            }
            else
            {
                ShowMaxLimitEffect(currentCollider.transform.position);
            }
        }
        else
        {
            lastHoveredArea = null;
        }
    }
    private void ApplyMaterial(Collider collider)
    {
        Renderer renderer = collider.GetComponent<Renderer>();
        if (renderer != null)
        {
            // 保存原始材质
            if (!originalMaterials.ContainsKey(collider))
            {
                originalMaterials[collider] = renderer.material;
            }

            // 应用高亮材质
            renderer.material = highlightMaterial;
            PlaySelectionEffect(collider.transform.position);
        }
    }

    private void ShowMaxLimitEffect(Vector3 position)
    {
        // 显示最大限制提示（示例使用Debug，可替换为粒子效果）
        Debug.LogWarning("已达到最大选择数量！");
        // Instantiate(maxLimitEffectPrefab, position, Quaternion.identity);
    }

    private void PlaySelectionEffect(Vector3 position)
    {
        // 添加选择特效（示例代码）
        // Instantiate(selectionEffectPrefab, position, Quaternion.identity);
    }

    private void FinalizeSelection()
    {
        int totalSelected = affectedAreas.Count + pendingAreas.Count;

        // 精确匹配最大数量时才确认
        if (totalSelected == maxAffectedAreas)
        {
            Debug.Log("区域选择完成");
            affectedAreas.UnionWith(pendingAreas);
            ConfirmSelection(); // 确认选择但不恢复材质

            if (canvasClickHandler != null)
            {
                canvasClickHandler.FinalizePlacement();
            }
        }
        else
        {
            Debug.Log($"选择未完成（当前选择数：{totalSelected}），已重置");
            RestoreAllMaterials();
        }

        pendingAreas.Clear();
        lastHoveredCollider = null;
    }
    public void ConfirmSelection()
    {
        // 保持当前材质不恢复
        originalMaterials.Clear();
        foreach (Collider area in affectedAreas)
        {
            originalMaterials[area] = area.GetComponent<Renderer>().material;
        }
    }
    public void ForceRestoreMaterials()
    {
        RestoreAllMaterials();
    }

    private void RestoreAllMaterials()
    {
        foreach (Collider area in affectedAreas)
        {
            ResetMaterial(area);
        }
        affectedAreas.Clear();
        originalMaterials.Clear();
    }

    private void ResetMaterial(Collider collider)
    {
        if (collider != null && originalMaterials.ContainsKey(collider))
        {
            Renderer renderer = collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = originalMaterials[collider];
            }
        }
    }
    private void OnDestroy()
    {
        // 确保销毁时恢复所有材质
        RestoreAllMaterials();
    }
    private void ResetDragState()
    {
        isDragging = false;
        Cursor.visible = true;
    }

    private bool IsPlacedCorrectly()
    {
        // 添加你的放置验证逻辑
        return true;
    }
}