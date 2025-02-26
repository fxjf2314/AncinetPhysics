using UnityEngine;

public class RangeIndicator11 : MonoBehaviour
{
    public DragAndPlaceIn3D placementSystem; // ��������ק����ϵͳ
    public Material highlightMaterial; // ��������
    public LayerMask planeLayer; // Plane �� Layer

    private Material originalMaterial;
    private Renderer currentRenderer;

    private void Start()
    {
        // ��ȡ�������е� DragAndPlaceIn3D ���
        placementSystem = GetComponentInParent<DragAndPlaceIn3D>();
    }

    private void Update()
    {
        // �����ק����ϵͳ����δ���״̬������Χѡ��
        //if (placementSystem != null && placementSystem.IsPlacedAndNotFinalized)
        {
            HandleRangeSelection();
        }
    }

    private void HandleRangeSelection()
    {
        // �����λ�÷�������
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, planeLayer))
        {
            // ������߻��� Plane
            if (hit.collider.CompareTag("Plane"))
            {
                // �ָ�֮ǰ Plane �Ĳ���
                if (currentRenderer != null)
                {
                    currentRenderer.material = originalMaterial;
                }

                // ��ȡ��ǰ Plane �� Renderer ��Ӧ�ø�������
                currentRenderer = hit.collider.GetComponent<Renderer>();
                originalMaterial = currentRenderer.material;
                currentRenderer.material = highlightMaterial;

                // ������������£����ӷ�Χָʾ������
                if (Input.GetMouseButtonDown(0))
                {
                    //placementSystem.AddRangeIndicator();
                }
            }
        }
        else
        {
            // �������δ���� Plane���ָ�֮ǰ Plane �Ĳ���
            if (currentRenderer != null)
            {
                currentRenderer.material = originalMaterial;
                currentRenderer = null;
            }
        }
    }
}