using UnityEngine;
using System.Collections.Generic;

public class DragMaterialController : MonoBehaviour
{
    [Header("��������")]
    public Material highlightMaterial; // ��ʱ����
    public LayerMask areaLayer;         // Area�㼶
    public int maxAffectedAreas = 3;    // ���Ӱ������

    private Dictionary<Collider, Material> originalMaterials = new Dictionary<Collider, Material>(); // ��¼ÿ��Area��ԭʼ����
    public HashSet<Collider> affectedAreas = new HashSet<Collider>();
    private HashSet<Collider> pendingAreas = new HashSet<Collider>();
    private bool isDragging = false;
    private CanvasClickHandler canvasClickHandler;
    private Collider lastHoveredCollider;
    private Collider lastHoveredArea;
    private void Awake()
    {
        // ��ȡ CanvasClickHandler ʵ��
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
            Debug.Log("��ʼ����ѡ��");
            Cursor.visible = false; // ���ع����ǿ����
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

        // ����ϴ���ͣ�������ʱЧ��
       /* if (lastHoveredArea != null && !affectedAreas.Contains(lastHoveredArea))
        {
            ResetMaterial(lastHoveredArea);
            pendingAreas.Remove(lastHoveredArea);
        }*/
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, areaLayer))
        {
            Collider currentCollider = hit.collider;
            lastHoveredArea = currentCollider;

            // ��ֹ�ظ�ѡ����
            if (affectedAreas.Contains(currentCollider)) return;

            if (affectedAreas.Count < maxAffectedAreas)
            {
                // ��ʱӦ�ò��ʵ���ͣ����
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
            // ����ԭʼ����
            if (!originalMaterials.ContainsKey(collider))
            {
                originalMaterials[collider] = renderer.material;
            }

            // Ӧ�ø�������
            renderer.material = highlightMaterial;
            PlaySelectionEffect(collider.transform.position);
        }
    }

    private void ShowMaxLimitEffect(Vector3 position)
    {
        // ��ʾ���������ʾ��ʾ��ʹ��Debug�����滻Ϊ����Ч����
        Debug.LogWarning("�Ѵﵽ���ѡ��������");
        // Instantiate(maxLimitEffectPrefab, position, Quaternion.identity);
    }

    private void PlaySelectionEffect(Vector3 position)
    {
        // ���ѡ����Ч��ʾ�����룩
        // Instantiate(selectionEffectPrefab, position, Quaternion.identity);
    }

    private void FinalizeSelection()
    {
        int totalSelected = affectedAreas.Count + pendingAreas.Count;

        // ��ȷƥ���������ʱ��ȷ��
        if (totalSelected == maxAffectedAreas)
        {
            Debug.Log("����ѡ�����");
            affectedAreas.UnionWith(pendingAreas);
            ConfirmSelection(); // ȷ��ѡ�񵫲��ָ�����

            if (canvasClickHandler != null)
            {
                canvasClickHandler.FinalizePlacement();
            }
        }
        else
        {
            Debug.Log($"ѡ��δ��ɣ���ǰѡ������{totalSelected}����������");
            RestoreAllMaterials();
        }

        pendingAreas.Clear();
        lastHoveredCollider = null;
    }
    public void ConfirmSelection()
    {
        // ���ֵ�ǰ���ʲ��ָ�
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
        // ȷ������ʱ�ָ����в���
        RestoreAllMaterials();
    }
    private void ResetDragState()
    {
        isDragging = false;
        Cursor.visible = true;
    }

    private bool IsPlacedCorrectly()
    {
        // �����ķ�����֤�߼�
        return true;
    }
}