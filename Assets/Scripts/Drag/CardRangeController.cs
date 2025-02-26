using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CardRangeController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float rangeRadius = 5.0f; // ���Ƶ����÷�Χ�뾶
    public LayerMask areaLayer; // Area �� Layer
    public Material highlightMaterial; // ��������

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
            newPosition.y = transform.position.y; // ���� Y �����겻��
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

        // �ָ�֮ǰ��Ӱ������Ĳ���
        foreach (Collider area in affectedAreas)
        {
            if (!newAffectedAreas.Contains(area))
            {
                ResetMaterial(area);
            }
        }

        // Ӧ�ø������ʵ�����Ӱ������
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
            // ����ԭʼ����
            if (!originalMaterials.ContainsKey(collider))
            {
                originalMaterials[collider] = renderer.material;
            }

            // Ӧ�ø�������
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
        // ȷ������ʱ�ָ����в���
        foreach (Collider area in affectedAreas)
        {
            ResetMaterial(area);
        }
        affectedAreas.Clear();
        originalMaterials.Clear();
    }
}