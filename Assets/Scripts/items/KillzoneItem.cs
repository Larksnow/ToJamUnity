using UnityEngine;

[CreateAssetMenu(fileName = "KillzoneItem", menuName = "Items/KillzoneItem")]
public class KillzoneItemSO : ScriptableObject
{
    public GameObject killzonePrefab; // Assign in Inspector
    
    // Function to spawn a killzone at the opponent's position
    public void SpawnKillzone(int callerID)
    {
        // Find the opponent based on playerID
        int opponentID = (callerID == 0) ? 1 : 0;

        // Find the player objects based on their IDs
        Player opponent = FindPlayerByID(opponentID);
        if (opponent == null || killzonePrefab == null)
        {
            Debug.LogWarning("KillzonePrefab or opponent not found.");
            return;
        }

        // Instantiate the killzone at the opponent's position
        Vector3 spawnPos = opponent.transform.position;
        GameObject kz = Instantiate(killzonePrefab, spawnPos, Quaternion.identity);

        // Random Z rotation (2D game, so only rotate on Z axis)
        float randomZ = Random.Range(0f, 360f);
        kz.transform.rotation = Quaternion.Euler(0f, 0f, randomZ);

        // Set the targetID to the opponent's playerID
        Killzone killzoneScript = kz.GetComponent<Killzone>();
        if (killzoneScript != null)
        {
            killzoneScript.targetID = opponent.playerID;
        }
    }

    private Player FindPlayerByID(int playerID)
    {
        Player[] players = FindObjectsOfType<Player>(); // Find all Player objects in the scene
        foreach (Player player in players)
        {
            if (player.playerID == playerID)
            {
                return player;
            }
        }
        return null; // Return null if no player with the specified ID is found
    }
}
