using UnityEngine;

[CreateAssetMenu(fileName = "KillzoneItem", menuName = "Items/SpeedUp")]
public class SpeedUpSO : ItemSO
{

    // This method will be overridden by all item types
    public override void UseItem(Player user)
    {
        AudioManager.main.PostEvent("Play_SpeedUpMusic");
    }
}