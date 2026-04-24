using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectionHighlight : MonoBehaviour
{
    private Renderer[] renderers;
    private Color[] originalColors;

    [SerializeField] private Color highlightColor = Color.yellow;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
    }

    private void OnEnable()
    {
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (grab != null)
        {
            grab.selectEntered.AddListener(OnSelected);
            grab.selectExited.AddListener(OnDeselected);
        }
    }

    private void OnDisable()
    {
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (grab != null)
        {
            grab.selectEntered.RemoveListener(OnSelected);
            grab.selectExited.RemoveListener(OnDeselected);
        }
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        SetHighlight(true);
    }

    private void OnDeselected(SelectExitEventArgs args)
    {
        SetHighlight(false);
    }

    private void SetHighlight(bool highlighted)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (highlighted)
            {
                renderers[i].material.color = highlightColor;
            }
            else
            {
                renderers[i].material.color = originalColors[i];
            }
        }
    }
}