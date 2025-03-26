using System;
using System.Collections;
using System.Collections.Generic;
using test;
using Test;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    private Transform UIPackage;

    protected override void Awake()
    {
        base.Awake();
        InitUI();
    }

    private void InitUI()
    {
        UIPackage = transform.Find("PackageBtn");
        UIPackage.GetComponent<Button>().onClick.AddListener(OnBtnUIPackage);
    }

    private void OnBtnUIPackage()
    {
        if (!UIManager.Instance.panelDict.ContainsKey(UIConst.PackageWeaponPanel))
        {
            UIManager.Instance.OpenPanel(UIConst.PackageObjectPanel);
            GameEvents.TriggerPanelOpen(UIConst.PackageObjectPanel);
        }
    }
}
