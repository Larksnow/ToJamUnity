using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // 检查是否是玩家角色
        if (other.CompareTag("Player"))
        {
            // 调用玩家脚本的拾取物品方法
            other.GetComponent<PlayerInteraction>().ItemPickUp(this);

            // 销毁道具物体（假设物品被拾取后消失）
            Destroy(gameObject);
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
