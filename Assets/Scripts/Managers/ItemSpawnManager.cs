using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    [Header("Spawn Timing")]
    public float persistentSpawnInterval = 3f;
    public float normalSpawnInterval = 3f;

    [Header("Spawn Prefabs")]
    public GameObject persistentItemPrefab;
    public List<GameObject> normalItemPrefabs;

    [Header("Spawn Area")]
    public float minX = -5f, maxX = 5f;
    public float minY = -3f, maxY = 3f;
    public float minDistance = 2f;
    public int maxAttempts = 10;

    [Header("Spawn Limits")]
    public int maxNormalItems = 3;
    private int lastNormalItemIndex = -1;

    private GameObject persistentItemInstance;
    private List<GameObject> normalItems = new List<GameObject>();

    void Start()
    {
        StartCoroutine(PersistentItemSpawnLoop());
        StartCoroutine(NormalItemSpawnLoop());
    }

    IEnumerator PersistentItemSpawnLoop()
    {
        while (true)
        {
            TrySpawnPersistentItem();
            yield return new WaitForSeconds(persistentSpawnInterval);
        }
    }

    IEnumerator NormalItemSpawnLoop()
    {
        while (true)
        {
            TrySpawnNormalItem();
            yield return new WaitForSeconds(normalSpawnInterval);
        }
    }

    void TrySpawnPersistentItem()
    {
        if (persistentItemInstance == null)
        {
            Vector3 pos = GetRandomSpawnPosition();
            if (pos != Vector3.positiveInfinity)
            {
                persistentItemInstance = Instantiate(persistentItemPrefab, pos, Quaternion.identity);
            }
        }
    }

    void TrySpawnNormalItem()
    {
        normalItems.RemoveAll(item => item == null);
        if (normalItems.Count >= maxNormalItems) return;

        if (normalItemPrefabs.Count == 0) return;

        int newIndex = lastNormalItemIndex;
        int attempts = 0;

        // Try to get a different prefab index than the last one
        while (newIndex == lastNormalItemIndex && attempts < 10)
        {
            newIndex = Random.Range(0, normalItemPrefabs.Count);
            attempts++;
        }

        GameObject prefab = normalItemPrefabs[newIndex];
        Vector3 pos = GetRandomSpawnPosition();
        if (pos != Vector3.positiveInfinity)
        {
            GameObject newItem = Instantiate(prefab, pos, Quaternion.identity);
            normalItems.Add(newItem);
            lastNormalItemIndex = newIndex;
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 candidate = new Vector3(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                0f
            );

            if (IsFarFromOthers(candidate))
            {
                return candidate;
            }
        }
        return Vector3.positiveInfinity;
    }

    bool IsFarFromOthers(Vector3 candidate)
    {
        if (persistentItemInstance != null &&
            Vector3.Distance(candidate, persistentItemInstance.transform.position) < minDistance)
            return false;

        foreach (var item in normalItems)
        {
            if (item != null && Vector3.Distance(candidate, item.transform.position) < minDistance)
                return false;
        }

        return true;
    }
}
