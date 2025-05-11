using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawnManager : MonoBehaviour
{
    [Header("Regular Items")]
    public List<GameObject> itemPrefabs;
    public int maxItemsOnScreen = 4;

    [Header("Special Persistent Item")]
    public GameObject persistentItemPrefab;
    private GameObject persistentItemInstance;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public float destroyAfterSeconds = 10f;
    public float overlapThreshold = 1f;

    private List<GameObject> spawnedItems = new List<GameObject>();
    private float lastSpawnTime;

    void Update()
    {
        // Spawn regular items if under limit
        if (Time.time - lastSpawnTime >= spawnInterval && spawnedItems.Count < maxItemsOnScreen)
        {
            TrySpawnRegularItem();
            lastSpawnTime = Time.time;
        }

        // Check persistent item and respawn if needed
        if (persistentItemInstance == null)
        {
            TrySpawnPersistentItem();
        }
    }

    void TrySpawnRegularItem()
    {
        if (itemPrefabs.Count == 0) return;

        Vector2 spawnPos = GenerateNonOverlappingPosition();
        if (spawnPos == Vector2.negativeInfinity) return;

        GameObject prefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
        GameObject item = Instantiate(prefab, spawnPos, Quaternion.identity);
        spawnedItems.Add(item);

        StartCoroutine(DestroyAfterTime(item, destroyAfterSeconds));
    }

    void TrySpawnPersistentItem()
    {
        if (persistentItemPrefab == null) return;

        Vector2 spawnPos = GenerateNonOverlappingPosition();
        if (spawnPos == Vector2.negativeInfinity) return;

        persistentItemInstance = Instantiate(persistentItemPrefab, spawnPos, Quaternion.identity);
    }

    IEnumerator DestroyAfterTime(GameObject item, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (item != null)
        {
            spawnedItems.Remove(item);
            Destroy(item);
        }
    }

    Vector2 GenerateNonOverlappingPosition()
    {
        for (int i = 0; i < 10; i++) // Try 10 times to find a non-overlapping position
        {
            float x = Random.Range(-9.5f, 9.5f);
            float y = Random.Range(0f, 5f);
            Vector2 tryPos = new Vector2(x, y);

            bool overlapping = false;

            foreach (GameObject obj in spawnedItems)
            {
                if (obj == null) continue;
                if (Vector2.Distance(tryPos, obj.transform.position) < overlapThreshold)
                {
                    overlapping = true;
                    break;
                }
            }

            if (persistentItemInstance != null &&
                Vector2.Distance(tryPos, persistentItemInstance.transform.position) < overlapThreshold)
            {
                overlapping = true;
            }

            if (!overlapping)
                return tryPos;
        }

        return Vector2.negativeInfinity; // Could not find a valid position
    }

    public void NotifyPersistentItemDestroyed()
    {
        persistentItemInstance = null; // Called externally when it’s picked up
    }
}
