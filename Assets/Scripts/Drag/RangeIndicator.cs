using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    public Material highlightMaterial; // ¸ßÁÁ²ÄÖÊ
    public LayerMask planeLayer; // Plane µÄ Layer

    public Material originalMaterial;
    private Renderer currentRenderer;

    private void Update()
    {
        HandleRangeHighlight();
    }

    private void HandleRangeHighlight()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, planeLayer))
        {
            if (hit.collider.CompareTag("Area"))
            {
                if (currentRenderer != null && currentRenderer != hit.collider.GetComponent<Renderer>())
                {
                    currentRenderer.material = originalMaterial;
                }

                currentRenderer = hit.collider.GetComponent<Renderer>();
                originalMaterial = currentRenderer.material;
                currentRenderer.material = highlightMaterial;
            }
        }
        else
        {
            if (currentRenderer != null)
            {
                currentRenderer.material = originalMaterial;
                currentRenderer = null;
            }
        }
    }
}