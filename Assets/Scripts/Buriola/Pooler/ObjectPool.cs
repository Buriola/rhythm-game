using System.Collections.Generic;
using UnityEngine;
using System;

namespace Buriola.Pooler
{
    public class ObjectPool<T>
    {
        private readonly List<ObjectPoolContainer<T>> _list;
        private readonly Dictionary<T, ObjectPoolContainer<T>> _lookup;
        private readonly Func<T> _factoryFunc;
        private int _lastIndex ;

        public ObjectPool(Func<T> factoryFunc, int initialSize)
        {
            _factoryFunc = factoryFunc;

            _list = new List<ObjectPoolContainer<T>>(initialSize);
            _lookup = new Dictionary<T, ObjectPoolContainer<T>>(initialSize);

            Warm(initialSize);
        }

        private void Warm(int capacity)
        {
            for (int i = 0; i < capacity; i++)
            {
                CreateContainer();
            }
        }

        private ObjectPoolContainer<T> CreateContainer()
        {
            var container = new ObjectPoolContainer<T> {Item = _factoryFunc()};
            _list.Add(container);
            return container;
        }

        public T GetItem()
        {
            ObjectPoolContainer<T> container = null;
            foreach (var t in _list)
            {
                _lastIndex++;
                if (_lastIndex > _list.Count - 1) _lastIndex = 0;

                if (_list[_lastIndex].Used) continue;
                
                container = _list[_lastIndex];
                break;
            }

            if (container == null)
                container = CreateContainer();

            container.Consume();
            _lookup.Add(container.Item, container);
            return container.Item;
        }

        public void ReleaseItem(T item)
        {
            if (_lookup.ContainsKey(item))
            {
                var container = _lookup[item];
                container.Release();
                _lookup.Remove(item);
            }
            else
            {
                Debug.LogWarning("This object pool does not contain the item provided: " + item);
            }
        }

        public int Count => _list.Count;

        public int CountUsedItems => _lookup.Count;
    }
}
