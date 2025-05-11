using UnityEngine;
using UnityEngine.UI;
public abstract class ItemSO : ScriptableObject
{
    public Sprite icon;
    // This method will be overridden by all item types
    public abstract void UseItem(Player user);
} 