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



    protected void Awake()
    {
        
        InitUI();
    }

    private void Start()
    {
        RefreshUI();
        
    }

    private void InitUI()
    {
        InitUIName();
        InitClick();
    }

    //刷新UI
    private void RefreshUI()
    {
        RefreshScroll();
    }

    //刷新滚动容器
    private void RefreshScroll()
    {
        //清空滚动容器中原本的物体
        RectTransform scrollContent = UIScrollView.GetComponent<ScrollRect>().content;
        for(int i = 0; i < scrollContent.childCount; i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }

        //拿到背包数据并初始化滚动容器
        foreach(PackageLoaclItem localData in GameManager.Instance.GetPackageLoaclData())
        {
            Transform PackageUIItem=GameObject.Instantiate(PackageUIItemPrefab.transform,scrollContent) as Transform;
            PackageCell packageCell=PackageUIItem.GetComponent<PackageCell>();
            packageCell.Refresh(localData, this);
        }
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
            PackageLoaclItem loaclItem = GameManager.Instance.GetPackageLoaclItemByUid(chooseUid);
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
        ClosePanel(UIConst.PackageObjectPanel);
        Time.timeScale = 1;
    }

    private void OnClickWeapon()
    {
        UIManager.Instance.OpenPanel(UIConst.PackageWeaponPanel);
        ClosePanel(UIConst.PackageObjectPanel);
    }

    private void OnClickObject()
    {
        UIManager.Instance.OpenPanel(UIConst.PackageObjectPanel);
        ClosePanel(UIConst.PackageWeaponPanel);
    }
}


