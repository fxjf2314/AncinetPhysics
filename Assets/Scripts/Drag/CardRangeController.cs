using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CardRangeController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float rangeRadius = 5.0f; // 卡牌的作用范围半径
    public LayerMask areaLayer; // Area 的 Layer
    public Material highlightMaterial; // 高亮材质

    private bool isDragging = false;
    private HashSet<Collider> affectedAreas = new HashSet<Collider>();
    private Dictionary<Collider, Material> originalMaterials = new Dictionary<Collider, Material>();

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDragging)
        {
            isDragging = true;
            Debug.Log("CardRangeController: Started dragging to adjust range.");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector3 newPosition = GetMouseWorldPosition();
            newPosition.y = transform.position.y; // 保持 Y 轴坐标不变
            transform.position = newPosition;

            UpdateAffectedAreas();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;
            Debug.Log("CardRangeController: Stopped dragging to adjust range.");
        }
    }

    private void UpdateAffectedAreas()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, rangeRadius, areaLayer);
        HashSet<Collider> newAffectedAreas = new HashSet<Collider>(hitColliders);

        // 恢复之前受影响区域的材质
        foreach (Collider area in affectedAreas)
        {
            if (!newAffectedAreas.Contains(area))
            {
                ResetMaterial(area);
            }
        }

        // 应用高亮材质到新受影响区域
        foreach (Collider area in newAffectedAreas)
        {
            if (!affectedAreas.Contains(area))
            {
                ApplyMaterial(area);
            }
        }

        affectedAreas = newAffectedAreas;
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
        }
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

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, areaLayer))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private void OnDestroy()
    {
        // 确保销毁时恢复所有材质
        foreach (Collider area in affectedAreas)
        {
            ResetMaterial(area);
        }
        affectedAreas.Clear();
        originalMaterials.Clear();
    }
}