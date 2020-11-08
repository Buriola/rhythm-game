using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Buriola.Pooler
{
    public class PoolManager : Singleton<PoolManager>
    {
        public bool LogStatus;
        public Transform Root;

        private Dictionary<GameObject, ObjectPool<GameObject>> _prefabLookup;
        private Dictionary<GameObject, ObjectPool<GameObject>> _instanceLookup;

        private bool _dirty;

        private void Awake()
        {
            _prefabLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
            _instanceLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();

            if (Instance != null && Instance != this)
                Destroy(gameObject);

            SceneManager.sceneLoaded += OnSceneLoadedCallback;
        }

        private void OnSceneLoadedCallback(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex != 1) return;

            _prefabLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
            _instanceLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
        }

        private void Update()
        {
            if (LogStatus && _dirty)
            {
                PrintStatus();
                _dirty = false;
            }
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoadedCallback;
        }

        private void warmPool(GameObject prefab, int size)
        {
            if (_prefabLookup.ContainsKey(prefab))
            {
                throw new System.Exception("Pool for prefab " + prefab.name + " has already been created");
            }

            var pool = new ObjectPool<GameObject>(() => InstantiatePrefab(prefab), size);
            _prefabLookup[prefab] = pool;

            _dirty = true;
        }

        private GameObject spawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (!_prefabLookup.ContainsKey(prefab))
                WarmPool(prefab, 1);

            var pool = _prefabLookup[prefab];

            var clone = pool.GetItem();
            clone.transform.position = position;
            clone.transform.rotation = rotation;
            clone.SetActive(true);

            _instanceLookup.Add(clone, pool);
            _dirty = true;
            return clone;
        }

        private void releaseObject(GameObject clone)
        {
            clone.SetActive(false);

            if (_instanceLookup.ContainsKey(clone))
            {
                _instanceLookup[clone].ReleaseItem(clone);
                _instanceLookup.Remove(clone);
                _dirty = true;
            }
            else
            {
                Debug.LogWarning("No pool contains the object: " + clone.name);
            }
        }

        private GameObject InstantiatePrefab(GameObject prefab)
        {
            var go = Instantiate(prefab);
            if (Root != null) go.transform.parent = Root;
            return go;
        }

        private void PrintStatus()
        {
            foreach (KeyValuePair<GameObject, ObjectPool<GameObject>> keyVal in _prefabLookup)
            {
                Debug.Log($"Object Pool for Prefab: {keyVal.Key.name} In Use: {keyVal.Value.CountUsedItems} Total {keyVal.Value.Count}");
            }
        }

        #region Static API

        public static void WarmPool(GameObject prefab, int size)
        {
            Instance.warmPool(prefab, size);
        }

        public static GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return Instance.spawnObject(prefab, position, rotation);
        }

        public static void ReleaseObject(GameObject clone)
        {
            Instance.releaseObject(clone);
        }

        #endregion
    }
}
