using System.Collections;
using System.Collections.Generic;
using Test;
using UnityEngine;

namespace test
{
    public class UIManager
    {
        private static UIManager _instance;
        //界面配置路径表
        private Dictionary<string, string> pathDict;

        private Transform _uiRoot;
        //预制件缓存字典
        private Dictionary<string, GameObject> prefabDict;
        //已打开界面缓存字典
        public Dictionary<string, BasePanel> panelDict;

        private UIManager() 
        {
            this.InitDicts();
        }
        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UIManager();
                }
                
                 return _instance;
                
            }
        }

        //懒加载UIRoot
        public Transform UIRoot
        {
            get
            {
                if (this._uiRoot == null)
                {
                    this._uiRoot = GameObject.Find("Canvas").transform; //查找场景中名为 "Canvas" 的游戏对象，并获取它的 Transform 组件，赋值给 _uiRoot
                    
                }
                return this._uiRoot;
            }
        }
        private void InitDicts()
        {
            prefabDict=new Dictionary<string, GameObject>();
            panelDict=new Dictionary<string, BasePanel>();
            pathDict = new Dictionary<string, string>()
            {
                //界面名称，界面路径
                {UIConst.PackageObjectPanel, "Prefab/Panel/Package/PackageObjectPanel"},
                {UIConst.PackageWeaponPanel,"Prefab/Panel/Package/PackageWeaponPanel" }
            };
        }

        //打开界面
        public BasePanel OpenPanel(string name)
        {
            BasePanel panel = null;
            if(panelDict.TryGetValue(name, out panel))
            {
                Debug.LogError("界面已打开：" + name);
                panel= panelDict[name];
                return panel;
            }

            string path = "";
            if(!pathDict.TryGetValue(name, out path))
            {
                Debug.LogError("界面名称错误，或未配置路径：" + name);
                return null;
            }

            //使用缓存的预制件
            GameObject panelPrefab = null;
            if(!prefabDict.TryGetValue(name, out panelPrefab))
            {
                string realPath = "Prefab/Panel/Package/" + name;
                panelPrefab=Resources.Load<GameObject>(realPath);
                prefabDict.Add(name, panelPrefab);
            }

            //打开界面
            Time.timeScale = 0f;
            GameObject panelObject=GameObject.Instantiate(panelPrefab,UIRoot,false);//实例化界面
            panel=panelObject.GetComponent<BasePanel>();
            panelDict.Add(name,panel);
            return panel;
        }

        //关闭界面
        public bool ClosePanel(string name)
        {
            BasePanel panel=null;
            if(!panelDict.TryGetValue(name, out panel))
            {
                Debug.LogError("界面未打开" + name);
                return false;
            }
            
            panel.ClosePanel(name);
            return true;
        }
    }



    //存储名称的 常量表
    public class UIConst
    {
        public const string PackageObjectPanel = "PackageObjectPanel";

        public const string PackageWeaponPanel = "PackageWeaponPanel";
    }
}
