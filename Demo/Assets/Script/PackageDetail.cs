using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PackageDetail : MonoBehaviour
{
    private Transform UIIcon;
    private Transform UITitleText;
    private Transform UIDetailText;
    private Transform UIUsingBtn;

    private PackageLoaclItem packageLoaclData;
    private PackageTableItem packageTableItem;

    private PackagePanel uiParent;

    private void Awake()
    {
        InitUIName();
        Test();
    }

    private void Test()
    {
        Refresh(GameManager.Instance.GetPackageLoaclData()[0],null);
    }

    private void InitUIName()
    {
        UIIcon = transform.Find("Top/ObjectIcon");
        UITitleText = transform.Find("Top/ObjectTitleText (TMP)");
        UIDetailText = transform.Find("Bottom/ObjectDetailText (TMP)");
        UIUsingBtn = transform.Find("Bottom/UsingButton");
    }

    //刷新详情界面
    public void Refresh(PackageLoaclItem packageLoaclData,PackagePanel uiParent)
    {
        //初始化信息
        this.uiParent = uiParent;
        this.packageTableItem = GameManager.Instance.GetPackageItemById(packageLoaclData.id);
        this.packageLoaclData = packageLoaclData;

        //图片加载
        Texture2D t=(Texture2D)Resources.Load(this.packageTableItem.imagePath);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        UIIcon.GetComponent<Image>().sprite = temp;

        //名称加载
        UITitleText.GetComponent<TextMeshProUGUI>().text = packageTableItem.name;

        //详情加载
        UIDetailText.GetComponent<TextMeshProUGUI>().text=packageTableItem.detailDescription;
    }
}
