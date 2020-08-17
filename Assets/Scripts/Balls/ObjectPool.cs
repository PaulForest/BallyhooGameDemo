using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Balls
{
    public class ObjectPool<T> : MonoBehaviour
        where T : IPoolableObject
    {
        private int _numberOfNumberOfBallsInPlay;
        [SerializeField] protected bool allowExpansion = true;
        [SerializeField] protected int initialCount = 200;

        protected readonly List<ObjectInstance> pool = new List<ObjectInstance>();
        [SerializeField] protected GameObject prefab;

        public bool HasBallsInPlay => _numberOfNumberOfBallsInPlay > 0;

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
            ExpandCapacity(initialCount);
        }

        private void ExpandCapacity(int newMaxCapacity)
        {
            if (!prefab)
            {
                Debug.LogError($"{this} I need a prefab set", this);
                return;
            }

            var min = pool.Count;
            var max = newMaxCapacity + min;
            for (var i = min; i < max; i++)
            {
                var go = Instantiate(prefab, transform, true);
                var t = go.GetComponent<T>();
                if (null == t)
                {
                    Debug.LogError($"prefab '{prefab}' needs an component of type {typeof(T).FullName}");
                    break;
                }

                t.BeforeReset();
                go.SetActive(false);
                t.AfterReset();

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
                instance.myComponent.BeforeReset();
                instance.gameObject.SetActive(false);
                instance.myComponent.AfterReset();
            }

            _numberOfNumberOfBallsInPlay = 0;
        }


        [CanBeNull]
        public virtual T GetAvailableObject()
        {
            ObjectInstance myInstance;

            var currentPoolCount = pool.Count;
            int i;
            for (i = 0; i < currentPoolCount; i++)
            {
                myInstance = pool[i];
                if (!myInstance.gameObject || myInstance.gameObject.activeSelf) continue;

                myInstance.gameObject.SetActive(true);
                myInstance.myComponent.ResetNonStaticData();

                _numberOfNumberOfBallsInPlay++;

                return myInstance.myComponent;
            }

            if (!allowExpansion) return default;

            var newCapacity = currentPoolCount * 2;
            Debug.Log($"{this}.GetAvailableObject(): expanding capacity from {currentPoolCount} to {newCapacity}");
            ExpandCapacity(newCapacity);

            myInstance = pool[i];
            myInstance.gameObject.SetActive(true);
            myInstance.myComponent.ResetNonStaticData();

            _numberOfNumberOfBallsInPlay++;

            return myInstance.myComponent;
        }

        public virtual void ReturnObject(GameObject go)
        {
            go.SetActive(false);
            _numberOfNumberOfBallsInPlay--;

            if (_numberOfNumberOfBallsInPlay == 0) GlobalEvents.LastBallDestroyed?.Invoke();

#if UNITY_EDITOR
            if (!pool.Exists(instance => instance.gameObject == go))
                Debug.LogError($"GameObject {go} wasn't contained in the pool");
#endif
        }

        protected struct ObjectInstance
        {
            public T myComponent;
            public GameObject gameObject;
        }
    }
}
