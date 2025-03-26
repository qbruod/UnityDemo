using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private Queue<T> pool = new Queue<T>();
    private T prefab;
    private Transform parent;

    // 初始化对象池
    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        for (int i = 0; i < initialSize; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    // 从池中获取对象
    public T Get()
    {
        if (pool.Count == 0)
        {
            ExpandPool(1);
        }
        T obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    // 将对象放回池中
    public void Return(T obj)
    {
        // 调用Cell的复位方法
        if (obj is PackageCell cell)
        {
            cell.ResetCell(); // 重置Cell状态
        }

        obj.gameObject.SetActive(false); // 禁用对象
        obj.transform.SetParent(parent); // 回到原始父级
        pool.Enqueue(obj); // 将对象重新加入池中
    }

    // 扩展池容量
    private void ExpandPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public int qq()
    {
        return pool.Count;
    }


}