using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Balls
{
    public class ObjectPool<T> : MonoBehaviour
        where T : IPoolableObject, IResettableNonStaticData
    {
        [SerializeField] protected int initialCount = 200;
        [SerializeField] protected bool allowExpansion = true;
        [SerializeField] protected GameObject prefab;

        public struct ObjectInstance
        {
            public T myComponent;
            public GameObject gameObject;
        }

        protected readonly List<ObjectInstance> pool = new List<ObjectInstance>();

        protected virtual void Awake()
        {
            pool.Clear();

            ExpandCapacity(initialCount);
        }

        protected void ExpandCapacity(int newMaxCapacity)
        {
            var min = pool.Count;
            var max = newMaxCapacity + min;
            for (var i = min; i < max; i++)
            {
                var go = GameObject.Instantiate(prefab, transform, true);
                var t = go.GetComponent<T>();
                if (null == t)
                {
                    Debug.LogError($"prefab '{prefab}' needs an component of type {typeof(T).FullName}");
                    break;
                }

                go.SetActive(false);
                pool.Add(new ObjectInstance
                {
                    myComponent = t,
                    gameObject = go
                });
            }
        }

        public virtual void ResetData()
        {
            foreach (var instance in pool)
            {
                instance.gameObject.SetActive(false);
            }
        }


        [CanBeNull]
        public virtual T GetAvailableObject()
        {
            ObjectInstance myInstance;

            var currentPoolCount = pool.Count;
            var i = 0;
            for (i = 0; i < currentPoolCount; i++)
            {
                myInstance = pool[i];
                if (myInstance.gameObject.activeSelf) continue;

                myInstance.gameObject.SetActive(true);
                myInstance.myComponent.ResetNonStaticData();
                return myInstance.myComponent;
            }

            if (!allowExpansion)
            {
                return default;
            }

            var newCapacity = currentPoolCount * 2;
            Debug.Log($"{this}.GetAvailableObject(): expanding capacity from {currentPoolCount} to {newCapacity}");
            ExpandCapacity(newCapacity);

            myInstance = pool[i];
            myInstance.gameObject.SetActive(true);
            myInstance.myComponent.ResetNonStaticData();
            return myInstance.myComponent;
        }

        public virtual void ReturnObject(GameObject go)
        {
            go.SetActive(false);

#if UNITY_EDITOR
            if (!pool.Exists(instance => instance.gameObject == go))
            {
                Debug.LogError($"GameObject {go} wasn't contained in the pool");
            }
#endif
        }
    }
}
