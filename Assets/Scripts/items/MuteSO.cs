using UnityEngine;
[CreateAssetMenu(fileName = "KillzoneItem", menuName = "Items/Mute")]
public class MuteSO : ItemSO
{

    // This method will be overridden by all item types
    public override void UseItem(Player user)
    {
        AudioManager.main.PostEvent("Stop_MusicTwoSeconds");
    }
}