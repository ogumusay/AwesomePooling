using System;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomePooling
{
    public class PoolingSystem : MonoBehaviour
    {
        private static PoolingSystem _instance;

        #region Singleton

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            DontDestroyOnLoad(_instance.gameObject);
        }

        public static PoolingSystem GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            return CreateNewInstance();
        }

        private static PoolingSystem CreateNewInstance()
        {
            GameObject instance = new GameObject("Pooling System", typeof(PoolingSystem));
            _instance = instance.GetComponent<PoolingSystem>();
            return _instance;
        }

        #endregion

        #region Public-Methods

        public void PoolObjects<T>(T poolableGameObject, int poolCount) where T : MonoBehaviour, IPoolable
        {
            for (int i = 0; i < poolCount; i++)
            {
                PoolObject<T>(poolableGameObject);
            }
        }

        public T GetFromPool<T>(bool forceCreate = true) where T : MonoBehaviour, IPoolable
        {
            if (PoolingHandler<T>.PooledObjects.ContainsKey(typeof(T)))
            {
                foreach (var obj in PoolingHandler<T>.PooledObjects[typeof(T)])
                {
                    if (!obj.IsInUse)
                    {
                        obj.gameObject.SetActive(true);
                        obj.IsInUse = true;
                        return obj;
                    }
                }

                if (forceCreate)
                {
                    Transform poolParent = GetPoolTransform<T>();
                    var obj = Instantiate(PoolingHandler<T>.PooledObjectPrefabs[typeof(T)], poolParent);
                    obj.IsInUse = true;
                    PoolingHandler<T>.PooledObjects[typeof(T)].Add(obj);
                    return obj;
                }

                return PoolingHandler<T>.PooledObjects[typeof(T)][0];
            }

            throw new Exception(typeof(T).ToString() + " couldn't found in the pool");
        }


        public void SendToPool<T>(T poolableGameObject) where T : MonoBehaviour, IPoolable
        {
            if (PoolingHandler<T>.PooledObjects.ContainsKey(typeof(T)))
            {
                Transform poolParent = GetPoolTransform<T>();
                poolableGameObject.transform.parent = poolParent;
                poolableGameObject.gameObject.SetActive(false);
                poolableGameObject.IsInUse = false;
            }
            else
            {
                throw new Exception("First you must pool " + typeof(T).ToString());
            }
        }

        public void SendAllObjectsToPool<T>() where T : MonoBehaviour, IPoolable
        {
            if (PoolingHandler<T>.PooledObjects.ContainsKey(typeof(T)))
            {
                Transform poolParent = GetPoolTransform<T>();

                foreach (var obj in PoolingHandler<T>.PooledObjects[typeof(T)])
                {
                    obj.transform.parent = poolParent;
                    obj.gameObject.SetActive(false);
                    obj.IsInUse = false;
                }
            }
            else
            {
                throw new Exception("First you must pool " + typeof(T).ToString());
            }
        }

        #endregion

        #region Helper-Methods

        private void PoolObject<T>(T poolableGameObject) where T : MonoBehaviour, IPoolable
        {
            if (!PoolingHandler<T>.PooledObjects.ContainsKey(typeof(T)))
            {
                GameObject poolParent = new GameObject(typeof(T).ToString() + " Pool");
                poolParent.transform.parent = transform;
                var obj = Instantiate(poolableGameObject, poolParent.transform);
                obj.gameObject.SetActive(false);
                obj.IsInUse = false;

                var newList = new List<T>();
                newList.Add(obj);
                PoolingHandler<T>.PooledObjects.Add(typeof(T), newList);
                PoolingHandler<T>.PooledObjectPrefabs.Add(typeof(T), poolableGameObject);
            }
            else
            {
                Transform poolParent = GetPoolTransform<T>();
                var obj = Instantiate(poolableGameObject, poolParent);
                obj.gameObject.SetActive(false);
                obj.IsInUse = false;

                PoolingHandler<T>.PooledObjects[typeof(T)].Add(obj);
            }
        }


        private Transform GetPoolTransform<T>() where T : MonoBehaviour, IPoolable
        {
            Transform poolParent = transform.Find(typeof(T).ToString() + " Pool");
            return poolParent;
        }

        #endregion

        private class PoolingHandler<T> where T : MonoBehaviour, IPoolable
        {
            public static Dictionary<Type, List<T>> PooledObjects = new Dictionary<Type, List<T>>();
            public static Dictionary<Type, T> PooledObjectPrefabs = new Dictionary<Type, T>();
        }
    }
}