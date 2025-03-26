using System;
using System.Collections;
using System.Collections.Generic;
using test;
using Test;
using UnityEngine;
using UnityEngine.UI;

public class PackagePanel:BasePanel
{
    private Transform UIMenuObjectBtn;
    private Transform UIMenuWeaponBtn;
    private Transform UICloseBtn;

    private Transform UIDetailPanel;
    private Transform UIScrollView;
    private Transform UICenter;

    public GameObject PackageUIItemPrefab;
    public GameObject UIDetailPanelPrefab;
    public GameObject currentSelectedItem;
    //记录鼠标选中的uid
    public string _chooseUid;

    // 对象池，用于管理PackageCell的复用
    private ObjectPool<PackageCell> cellPool;

    // 当前激活的Cell列表，用于跟踪正在使用的对象
    private List<PackageCell> activeCells = new List<PackageCell>();


    //属性：从外部获取or修改Uid
    public string chooseUid
    {
        get
        {
            return _chooseUid;
        }
        set
        {
            _chooseUid = value;
            RefreshDetail();
        }
    }



    protected override void Awake()
    {
        base.Awake();
        InitUI();

    }

    private void Start()
    {
        RefreshUI();
    }

    //当面板启用时，订阅相关事件
    protected override void OnEnable()
    {
        base.OnEnable();
        // 确保PackageUIItemPrefab已正确赋值
        if (PackageUIItemPrefab != null && cellPool == null)
        {
            // 创建独立池容器，避免影响滚动视图布局
            var poolParent = new GameObject("CellPool").transform;
            poolParent.SetParent(UIManager.Instance.UIRoot);
            poolParent.gameObject.SetActive(false); // 隐藏池容器

            cellPool = new ObjectPool<PackageCell>(
                PackageUIItemPrefab.GetComponent<PackageCell>(),
                initialSize: 10,
                parent: poolParent // 池对象存放在独立父级
            );
        }
        // 订阅背包数据变化事件
        GameEvents.OnInventoryDataChanged += HandleInventoryUpdate;
        // 订阅面板状态变化事件
        GameEvents.OnPanelOpened += HandlePanelStateChange;
        GameEvents.OnPanelClosed += HandlePanelStateChange;
    }

    //当面板禁用时，取消订阅事件
    protected override void OnDisable()
    {
        base.OnDisable();
        // 取消订阅背包数据变化事件
        GameEvents.OnInventoryDataChanged -= HandleInventoryUpdate;
        // 取消订阅面板状态变化事件
        GameEvents.OnPanelOpened -= HandlePanelStateChange;
        GameEvents.OnPanelClosed -= HandlePanelStateChange;
    }


    //初始化UI
    private void InitUI()
    {
        InitUIName();
        InitClick();
    }

    //刷新UI
    public void RefreshUI()
    {
        RefreshScroll();
    }


    //处理背包数据更新事件
    private void HandleInventoryUpdate()
    {
        RefreshScroll(); // 刷新滚动视图
        RefreshDetail(); // 刷新详情面板
    }

    //处理面板状态变化事件
    /// <param name="panelName">发生变化的面板名称</param>
    private void HandlePanelStateChange(string panelName)
    {
        if (panelName == this.name) // 如果当前面板状态变化
        {
            RefreshUI(); // 刷新UI
        }
    }


    //打开面板时触发事件
    /// <param name="name">面板名称</param>
    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);
        GameEvents.TriggerPanelOpen(name); // 触发面板打开事件
    }

    //关闭面板时触发事件
    /// <param name="name">面板名称</param>
    public override void ClosePanel(string name)
    {
        base.ClosePanel(name);
        GameEvents.TriggerPanelClose(name); // 触发面板关闭事件
    }



    //刷新滚动容器
    private void RefreshScroll()
    {
        RectTransform scrollContent = UIScrollView.GetComponent<ScrollRect>().content;

        // 回收所有已激活的Cell
        foreach (var cell in activeCells)
        {
            cellPool.Return(cell); // 将Cell放回池中
            cell.transform.SetParent(null); // 解除与滚动容器的关联
        }
        activeCells.Clear(); // 清空激活列表
        UIManager.Instance.ClearPackageCellDict(); // 清空UI管理器中的缓存

        // 清空滚动容器
        foreach (Transform child in scrollContent)
        {
            Destroy(child.gameObject);
        }

        //拿到背包数据并初始化滚动容器
        foreach (PackageLocalItem localData in GameManager.Instance.GetPackageLoaclData())
        {

            if (localData.type == GameConst.PackageTypeFood && UIManager.Instance.panelDict.ContainsKey(UIConst.PackageObjectPanel))
            {
                if(UIManager.Instance.packageCellIdDict.ContainsKey(localData.id))
                {
                    PackageCell packageCell = UIManager.Instance.packageCellIdDict[localData.id];
                    UIManager.Instance.packageCountNumDict[localData.id] += localData.num;
                    packageCell.Refresh(packageCell.PackageLocalItem, this);
                }
                else 
                {
                    // 从池中获取一个Cell
                    PackageCell cell = cellPool.Get();
                    cell.transform.SetParent(scrollContent); // 设置父级
                    cell.transform.localPosition = Vector3.zero;
                    cell.transform.localScale = Vector3.one; // 重置缩放
                    UIManager.Instance.packageCountNumDict.Add(localData.id, localData.num);
                    cell.Refresh(localData, this); // 刷新Cell数据
                    UIManager.Instance.AddPackageCellDict(localData, cell); // 添加到UI管理器
                    activeCells.Add(cell);
                }

            }
            else if(localData.type == GameConst.PackageTypeWeapon &&UIManager.Instance.panelDict.ContainsKey(UIConst.PackageWeaponPanel))
            {
                // 从池中获取一个Cell
                PackageCell cell = cellPool.Get();
                cell.transform.SetParent(scrollContent); // 设置父级
                cell.transform.localPosition = Vector3.zero;
                cell.transform.localScale = Vector3.one; // 重置缩放
                cell.Refresh(localData, this); // 刷新Cell数据
                activeCells.Add(cell);
            }
            
        }
        // 强制刷新布局
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollContent);
        Debug.Log($"池中可用对象: {cellPool.qq()} | 激活对象: {activeCells.Count}");
    }

    //刷新详情界面
    private void RefreshDetail()
    {
        RectTransform scrollContent = UIScrollView.GetComponent<ScrollRect>().content;
        //清空详情页中的物体
        if (scrollContent.childCount == 0) 
        {
            for (int i = 0; i < UIDetailPanel.childCount; i++)
            {
                Destroy(UIDetailPanel.GetChild(i).gameObject);

            }
        }
        else
        {
            //拿到物品数据并初始化详情页
            PackageLocalItem loaclItem = GameManager.Instance.GetPackageLoaclItemByUid(chooseUid);
            UIDetailPanel.GetComponent<PackageDetail>().Refresh(loaclItem, this);
        }
        

        
    }

    //初始化UI
    private void InitUIName()
    {
        UIMenuObjectBtn = transform.Find("RightTop/ObjectButton");
        UIMenuWeaponBtn = transform.Find("RightTop/WeaponButton");
        UICloseBtn = transform.Find("RightTop/CloseButton");

        UIDetailPanel = transform.Find("Center/DetailPanel");
        UICenter = transform.Find("Center");

        UIScrollView = transform.Find("Center/Scroll View");
    }

    //绑定按钮和事件
    private void InitClick()
    {
        UIMenuObjectBtn.GetComponent<Button>().onClick.AddListener(OnClickObject);
        UIMenuWeaponBtn.GetComponent<Button>().onClick.AddListener(OnClickWeapon);
        UICloseBtn.GetComponent<Button>().onClick.AddListener(OnClickClose);
    }


    private void OnClickClose()
    {
        if (UIManager.Instance.panelDict.ContainsKey(UIConst.PackageObjectPanel))
        {
            ClosePanel(UIConst.PackageObjectPanel);
            GameEvents.TriggerPanelClose(UIConst.PackageObjectPanel);
            Time.timeScale = 1;
        }
        else if(UIManager.Instance.panelDict.ContainsKey(UIConst.PackageWeaponPanel))
        {
            ClosePanel(UIConst.PackageWeaponPanel);
            GameEvents.TriggerPanelClose(UIConst.PackageWeaponPanel);
            Time.timeScale = 1;
        }
    }

    private void OnClickWeapon()
    {
        UIManager.Instance.OpenPanel(UIConst.PackageWeaponPanel);
        GameEvents.TriggerPanelOpen(UIConst.PackageWeaponPanel);
        ClosePanel(UIConst.PackageObjectPanel);
        GameEvents.TriggerPanelClose(UIConst.PackageObjectPanel);
    }

    private void OnClickObject()
    {
        UIManager.Instance.OpenPanel(UIConst.PackageObjectPanel);
        GameEvents.TriggerPanelOpen(UIConst .PackageObjectPanel);
        ClosePanel(UIConst.PackageWeaponPanel);
        GameEvents.TriggerPanelClose (UIConst .PackageWeaponPanel);
    }
}


