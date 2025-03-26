using System.Collections;
using System.Collections.Generic;
using test;
using UnityEngine;
using static UnityEditor.Progress;

namespace Test
{
    public class BasePanel : MonoBehaviour
    {

        protected bool isRemove = false;//当前页面是否被关闭
        protected new string name;//页面名称

        public virtual void OpenPanel(string name)
        {
            this.name = name;
            gameObject.SetActive(true);
        }

        public virtual void ClosePanel(string name)
        {
            Debug.Log("444");

            if (UIManager.Instance.panelDict.ContainsKey(name))
            {
                isRemove = true;
                gameObject.SetActive(false);
                
                UIManager.Instance.panelDict.Remove(name);
                List<PackageLocalItem> readItem = PackageLocalData.Instance.LoadPackage();

                UIManager.Instance.ClearPackageCellDict();



            }
        }
        //当面板启用时调用，供子类重写
        protected virtual void OnEnable()
        {
            // 用于子类的订阅逻辑
        }


        //当面板禁用时调用，供子类重写
        protected virtual void OnDisable()
        {
            // 用于子类的取消订阅逻辑
        }

        protected virtual void Awake()
        {

        }


    }
}
