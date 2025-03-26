using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 全局事件系统，用于模块间通信
public static class GameEvents
{
    //背包事件系统
    public static event Action OnInventoryDataChanged;//背包数据发生变化时触发
    public static event Action<PackageLocalItem> OnItemAdded; // 当添加新物品时触发
    public static event Action<PackageLocalItem> OnItemRemoved; // 当移除物品时触发
    public static event Action<PackageLocalItem> OnItemUpdated; // 当物品更新时触发

    // UI系统事件
    public static event Action<string> OnPanelOpened; // 当面板打开时触发
    public static event Action<string> OnPanelClosed; // 当面板关闭时触发


    //触发背包数据变化事件
    public static void TriggerInventoryChanged() => OnInventoryDataChanged?.Invoke();

    //触发物品添加事件
    ///<param name = "item" > 被添加的物品 </ param >
    public static void TriggerItemAdded(PackageLocalItem item) => OnItemAdded?.Invoke(item);

    //触发物品移除事件
    /// <param name="item">被移除的物品</param>
    public static void TriggerItemRemoved(PackageLocalItem item) => OnItemRemoved?.Invoke(item);

    //触发物品更新事件
    /// <param name="item">被更新的物品</param>
    public static void TriggerItemUpdated(PackageLocalItem item) => OnItemUpdated?.Invoke(item);

    //触发面板打开事件
    /// <param name="panelName">被打开的面板名称</param>
    public static void TriggerPanelOpen(string panelName) => OnPanelOpened?.Invoke(panelName);

    //触发面板关闭事件
    /// <param name="panelName">被关闭的面板名称</param>
    public static void TriggerPanelClose(string panelName) => OnPanelClosed?.Invoke(panelName);

}
