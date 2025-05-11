using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public int mute = 0;
    public int speedup = 0;
    public int slowdown = 0;
    public int attack = 0;

        // 处理物品捡起的逻辑
    public void ItemPickUp(PickupItem item)
    {
        if (item.itemName == "mute")
        {
            // 增加数量
            mute++;
            Debug.Log("You picked up a mute item, Total mute: " + mute);
        }

        if (item.itemName == "speedup")
        {
            speedup++;
            Debug.Log("You picked up a health potion! Total speedup: " + speedup);
        }

                if (item.itemName == "slowdown")
        {
            slowdown++;
            Debug.Log("You picked up a health potion! Total slowdown: " + slowdown);
        }

                if (item.itemName == "attack")
        {
            attack++;
            Debug.Log("You picked up a health potion! Total attack: " + attack);
        }
       
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
