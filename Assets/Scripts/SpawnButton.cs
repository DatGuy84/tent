using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;

    private float cooldown = 0.5f;
    private float lastSpawnTime = -999f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Controller"))
            return;

        if (Time.time - lastSpawnTime < cooldown)
            return;

        lastSpawnTime = Time.time;

        Vector3 spawnPos = spawnPoint.position + Vector3.up * 0.5f;
        GameObject obj = Instantiate(prefabToSpawn, spawnPos, spawnPoint.rotation);

        SelectableItem selectable = obj.GetComponent<SelectableItem>();

        if (selectable == null)
        {
            selectable = obj.AddComponent<SelectableItem>();
        }

        selectable.ForceUnselected();

        if (obj.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = obj.AddComponent<Rigidbody>();
            rb.useGravity = true;
        }

        if (obj.GetComponent<Collider>() == null)
        {
            obj.AddComponent<BoxCollider>();
        }
    }
}