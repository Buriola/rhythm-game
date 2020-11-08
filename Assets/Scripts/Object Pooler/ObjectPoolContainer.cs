using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolContainer<T>
{
    private T item;

    public bool Used { get; private set; }

    public void Consume() { Used = true; }

    public T Item { get { return item; } set { item = value; } }

    public void Release() { Used = false; }
}
