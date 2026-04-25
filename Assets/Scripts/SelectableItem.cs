using UnityEngine;

public class SelectableItem : MonoBehaviour
{
    private Renderer[] renderers;
    private Color[] originalColors;
    private Rigidbody rb;

    public bool isSelected = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        renderers = GetComponentsInChildren<Renderer>();

        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
    }

    public void Select()
    {
        isSelected = true;
        Highlight(Color.yellow);
    }

    public void Deselect()
    {
        isSelected = false;
        RemoveHighlight();
    }

    public void ForceUnselected()
    {
        isSelected = false;
        RemoveHighlight();
    }

    private void Highlight(Color color)
    {
        foreach (Renderer r in renderers)
        {
            r.material.color = color;
        }
    }

    private void RemoveHighlight()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }
}