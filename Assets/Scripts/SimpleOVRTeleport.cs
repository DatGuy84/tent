using UnityEngine;

public class SimpleOVRTeleport : MonoBehaviour
{
    public Transform leftController;
    public Transform centerEye;
    public Transform playerRig;

    public float maxDistance = 20f;
    public LineRenderer lineRenderer;

    [Header("Ray Visual")]
    public float rayWidth = 0.02f;

    [Header("Orientation")]
    public float turnMultiplier = 2.0f;

    void Update()
    {
        if (leftController == null || centerEye == null || playerRig == null)
            return;

        Ray ray = new Ray(leftController.position, leftController.forward);

        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, maxDistance);

        DrawRay(hitSomething, hit);

        // Left trigger
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (hitSomething && hit.normal.y > 0.5f)
            {
                TeleportTo(hit.point);
            }
        }
    }

    void DrawRay(bool hitSomething, RaycastHit hit)
    {
        if (lineRenderer == null)
            return;

        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;

        lineRenderer.startWidth = rayWidth;
        lineRenderer.endWidth = rayWidth;

        lineRenderer.SetPosition(0, leftController.position);

        if (hitSomething)
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, leftController.position + leftController.forward * maxDistance);
        }
    }

    void TeleportTo(Vector3 targetPoint)
    {
        // Move the OVRCameraRig so the headset lands near the teleport point
        Vector3 headOffset = centerEye.position - playerRig.position;

        Vector3 newRigPosition = targetPoint - new Vector3(headOffset.x, 0, headOffset.z);
        newRigPosition.y = playerRig.position.y;

        playerRig.position = newRigPosition;

        // Get current headset facing direction, ignoring up/down tilt
        Vector3 headForward = centerEye.forward;
        headForward.y = 0;

        // Get left controller facing direction, ignoring up/down tilt
        Vector3 controllerForward = leftController.forward;
        controllerForward.y = 0;

        if (headForward.sqrMagnitude < 0.001f || controllerForward.sqrMagnitude < 0.001f)
            return;

        headForward.Normalize();
        controllerForward.Normalize();

        // Find how much the controller is turned compared to the headset
        float turnAmount = Vector3.SignedAngle(headForward, controllerForward, Vector3.up);

        // Make the turn stronger
        float finalTurn = turnAmount * turnMultiplier;

        // Rotate the whole player rig
        playerRig.Rotate(0, finalTurn, 0);
    }
}