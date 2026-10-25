using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Pool<T> where T : Component
{
    private readonly Queue<T> _items = new();

    [SerializeField] private T prefab;
    [SerializeField] private Transform parent;


    public T Get()
    {
        if (_items.Count == 0)
        {
            return Object.Instantiate(prefab, parent);
        }

        var item = _items.Dequeue();
        item.gameObject.SetActive(true);
        return item;
    }


    public T Get(Action<T> action)
    {
        var item = Get();
        action.Invoke(item);
        return item;
    }

    public void BackToPool(T item)
    {
        item.gameObject.SetActive(false);
        _items.Enqueue(item);
    }
}