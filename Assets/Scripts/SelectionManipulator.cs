using UnityEngine;

public class SelectionManipulator : MonoBehaviour
{
    public Transform controller;
    public Transform head;
    public LineRenderer rayLine;

    private SelectableItem currentTouchItem;
    private SelectableItem selectedItem;

    private bool manipulating = false;
    private bool scaling = false;

    private Vector3 grabPositionOffset;
    private Quaternion grabRotationOffset;

    private float startControllerHeadDistance;
    private Vector3 startScale;

    void Update()
    {
        // Selection method 1: touch object and press trigger
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (currentTouchItem != null)
            {
                SelectItem(currentTouchItem);
            }
            else
            {
                TryRaySelect();
            }
        }

        // Start move/rotate with grip only
        if (selectedItem != null && OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            StartManipulation();
        }

        // While grip is held, move and rotate
        if (manipulating && selectedItem != null)
        {
            MoveAndRotateSelected();
        }

        // If trigger is also held while gripping, scale
        if (manipulating && selectedItem != null && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (!scaling)
            {
                StartScaling();
            }

            ScaleSelected();
        }
        else
        {
            scaling = false;
        }

        // Release grip to drop object
        if (manipulating && OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
        {
            StopManipulation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SelectableItem item = other.GetComponent<SelectableItem>();

        if (item == null)
            item = other.GetComponentInParent<SelectableItem>();

        if (item != null)
        {
            currentTouchItem = item;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SelectableItem item = other.GetComponent<SelectableItem>();

        if (item == null)
            item = other.GetComponentInParent<SelectableItem>();

        if (item != null && item == currentTouchItem)
        {
            currentTouchItem = null;
        }
    }

    void SelectItem(SelectableItem item)
    {
        if (selectedItem != null)
        {
            selectedItem.Deselect();
        }

        selectedItem = item;
        selectedItem.Select();
    }

    void TryRaySelect()
    {
        Ray ray = new Ray(controller.position, controller.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            SelectableItem item = hit.collider.GetComponent<SelectableItem>();

            if (item == null)
                item = hit.collider.GetComponentInParent<SelectableItem>();

            if (item != null)
            {
                SelectItem(item);
            }
        }

        if (rayLine != null)
        {
            rayLine.SetPosition(0, controller.position);
            rayLine.SetPosition(1, controller.position + controller.forward * 10f);
        }
    }

    void StartManipulation()
    {
        manipulating = true;

        Rigidbody rb = selectedItem.GetRigidbody();

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        grabPositionOffset = selectedItem.transform.position - controller.position;
        grabRotationOffset = Quaternion.Inverse(controller.rotation) * selectedItem.transform.rotation;
    }

    void MoveAndRotateSelected()
    {
        selectedItem.transform.position = controller.position + grabPositionOffset;
        selectedItem.transform.rotation = controller.rotation * grabRotationOffset;
    }

    void StartScaling()
    {
        scaling = true;
        startScale = selectedItem.transform.localScale;
        startControllerHeadDistance = Vector3.Distance(controller.position, head.position);
    }

    void ScaleSelected()
    {
        float currentDistance = Vector3.Distance(controller.position, head.position);
        float scaleFactor = currentDistance / startControllerHeadDistance;

        selectedItem.transform.localScale = startScale * scaleFactor;
    }

    void StopManipulation()
    {
        manipulating = false;
        scaling = false;

        Rigidbody rb = selectedItem.GetRigidbody();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}