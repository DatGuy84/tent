using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    public void SpawnItem(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        if (obj.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = obj.AddComponent<Rigidbody>();
            rb.useGravity = true;
        }

        if (obj.GetComponent<Collider>() == null)
        {
            obj.AddComponent<BoxCollider>();
        }

        if (obj.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>() == null)
        {
            obj.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        }
    }
}