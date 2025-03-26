using System.Collections;
using System.Collections.Generic;
using test;
using UnityEngine;
using static UnityEditor.Progress;

public class PackageController
{
    private PackageLocalData _data;
    private UIManager _uiManager;

    public PackageController(PackageLocalData data, UIManager uiManager)
    {
        _data = data;
        _uiManager = uiManager;
    }

    // 添加物品的完整逻辑
    public void AddItem(PackageLocalItem newItem)
    {
        // 1. 操作Model
        _data.AddItem(newItem);
        GameEvents.TriggerItemAdded(newItem);//触发物品添加事件
        GameEvents.TriggerInventoryChanged();//触发背包数据变化事件
    }

    public void RemoveItem(string uid)
    {
        PackageLocalItem target = PackageLocalData.Instance.item.Find(i => i.uid == uid);
        if (target != null)
        {
            _data.RemoveItem(target);
            GameEvents.TriggerItemRemoved(target); // 触发物品移除事件
            GameEvents.TriggerInventoryChanged(); // 触发背包数据变化事件

        }
    }

    public void UpdateItem(PackageLocalItem updatedItem)
    {
        var index = PackageLocalData.Instance.item.FindIndex(i => i.uid == updatedItem.uid);
        if (index != -1)
        {
            _data.UpdateItem(updatedItem);
            GameEvents.TriggerItemUpdated(updatedItem); // 触发物品更新事件
            GameEvents.TriggerInventoryChanged(); // 触发背包数据变化事件
        }
    }

    public void ClaerItem()
    {
        PackageLocalData.Instance.item = new List<PackageLocalItem>();
        GameEvents.TriggerInventoryChanged(); // 触发背包数据变化事件
    }

}
