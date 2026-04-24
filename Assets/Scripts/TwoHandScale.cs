using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class TwoHandScale : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    private readonly List<UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor> interactors = new List<UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor>();

    private float startingDistance;
    private Vector3 startingScale;

    [SerializeField] private float minScale = 0.3f;
    [SerializeField] private float maxScale = 3.0f;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (!interactors.Contains(args.interactorObject))
        {
            interactors.Add(args.interactorObject);
        }

        if (interactors.Count == 2)
        {
            startingDistance = GetControllerDistance();
            startingScale = transform.localScale;
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (interactors.Contains(args.interactorObject))
        {
            interactors.Remove(args.interactorObject);
        }

        if (interactors.Count < 2)
        {
            startingDistance = 0f;
        }
    }

    private void Update()
    {
        if (interactors.Count == 2 && startingDistance > 0f)
        {
            float currentDistance = GetControllerDistance();
            float scaleMultiplier = currentDistance / startingDistance;

            Vector3 newScale = startingScale * scaleMultiplier;

            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

            transform.localScale = newScale;
        }
    }

    private float GetControllerDistance()
    {
        Vector3 firstHand = interactors[0].transform.position;
        Vector3 secondHand = interactors[1].transform.position;

        return Vector3.Distance(firstHand, secondHand);
    }
}