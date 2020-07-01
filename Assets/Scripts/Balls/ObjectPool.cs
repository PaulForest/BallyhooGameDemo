using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Balls
{
    public class ObjectPool<T> : MonoBehaviour
        where T : IPoolableObject
    {
        [SerializeField] protected int initialCount = 200;
        [SerializeField] protected GameObject prefab;

        protected readonly List<GameObject> pool = new List<GameObject>();

        protected virtual void Awake()
        {
            pool.Clear();

            for (var i = 0; i < initialCount; i++)
            {
                var go = GameObject.Instantiate(prefab, transform, true);
                var t = go.GetComponent<T>();
                if (null == t)
                {
                    Debug.LogError($"prefab '{prefab}' needs an component of type {typeof(T).FullName}");
                    break;
                }

                go.SetActive(false);
                pool.Add(go);
            }
        }

        public virtual void ResetData()
        {
            foreach (var instance in pool)
            {
                instance.SetActive(false);
            }
        }


        [CanBeNull]
        public virtual T GetAvailableObject()
        {
            var max = pool.Count;
            for (var i = 0; i < max; i++)
            {
                var go = pool[i];
                if (go.activeSelf) continue;

                go.SetActive(true);
                return go.GetComponent<T>();
            }

            return default;
        }

        public virtual void ReturnObject(GameObject go)
        {
            go.SetActive(false);

#if UNITY_EDITOR
            if (!pool.Contains(go))
            {
                Debug.LogError($"GameObject {go} wasn't contained in the pool");
            }
#endif
        }
    }
}