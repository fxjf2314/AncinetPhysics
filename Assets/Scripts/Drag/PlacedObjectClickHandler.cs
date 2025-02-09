using UnityEngine;

public class RangeIndicator11 : MonoBehaviour
{
    public DragAndPlaceIn3D placementSystem; // 关联的拖拽放置系统
    public Material highlightMaterial; // 高亮材质
    public LayerMask planeLayer; // Plane 的 Layer

    private Material originalMaterial;
    private Renderer currentRenderer;

    private void Start()
    {
        // 获取父对象中的 DragAndPlaceIn3D 组件
        placementSystem = GetComponentInParent<DragAndPlaceIn3D>();
    }

    private void Update()
    {
        // 如果拖拽放置系统处于未完成状态，处理范围选择
        //if (placementSystem != null && placementSystem.IsPlacedAndNotFinalized)
        {
            HandleRangeSelection();
        }
    }

    private void HandleRangeSelection()
    {
        // 从鼠标位置发射射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, planeLayer))
        {
            // 如果射线击中 Plane
            if (hit.collider.CompareTag("Plane"))
            {
                // 恢复之前 Plane 的材质
                if (currentRenderer != null)
                {
                    currentRenderer.material = originalMaterial;
                }

                // 获取当前 Plane 的 Renderer 并应用高亮材质
                currentRenderer = hit.collider.GetComponent<Renderer>();
                originalMaterial = currentRenderer.material;
                currentRenderer.material = highlightMaterial;

                // 如果鼠标左键按下，增加范围指示器计数
                if (Input.GetMouseButtonDown(0))
                {
                    //placementSystem.AddRangeIndicator();
                }
            }
        }
        else
        {
            // 如果射线未击中 Plane，恢复之前 Plane 的材质
            if (currentRenderer != null)
            {
                currentRenderer.material = originalMaterial;
                currentRenderer = null;
            }
        }
    }
}