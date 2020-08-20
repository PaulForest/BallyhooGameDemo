using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Balls
{
    /// <summary>
    /// Maintains a generic collection of objects, utilizing object pooling.  That is, <see cref="initialCount"/>
    /// instances are initially, created but marked as not enabled.  When instances are requested via
    /// <see cref="GetAvailableObject"/>, a disabled instance is selected, <see cref="IPoolableObject.BeforeReset"/> is
    /// called on that object, it's enabled, <see cref="IPoolableObject.AfterReset"/> is called, and the object is
    /// returned.  Should there be insufficient objects available, more objects are created iff
    /// <see cref="allowExpansion"/> is true, and one is returned, else null is returned.
    /// <see cref="IPoolableObject"/>
    /// <see cref="IResettableNonStaticData"/>
    /// </summary>
    public class ObjectPool<T> : MonoBehaviour
        where T : IPoolableObject, IResettableNonStaticData
    {
        /// <summary>
        /// Whether we'll add more objects to the collection if there are not enough instances when requesting an
        /// instance.
        /// </summary>
        [SerializeField] protected bool allowExpansion = true;

        /// <summary>
        /// The initial number of objects to allocate.
        /// </summary>
        [SerializeField] protected int initialCount = 200;

        /// <summary>
        /// The objects themselves.
        /// </summary>
        protected readonly List<ObjectInstance> pool = new List<ObjectInstance>();

        /// <summary>
        /// The object to create.  Must be non-null.
        /// </summary>
        [SerializeField] protected GameObject prefab;

        /// <summary>
        /// Convenience method to get the number of active objects.  Note that this isn't the same as pool.Count, as not
        /// all objects will be active.  This is a cheaper version of:
        /// <code>pool.Count(instance => instance.gameObject.activeSelf) > 0;</code>,
        /// although it does mean manually keeping track of <see cref="_numberOfNumberOfObjectsInPlay"/>.
        /// </summary>
        public bool HasObjectsInPlay => _numberOfNumberOfObjectsInPlay > 0;
        private int _numberOfNumberOfObjectsInPlay;

        protected virtual void Awake()
        {
        }

        /// <summary>
        /// Initially create <see cref="initialCount"/> objects.
        /// </summary>
        protected virtual void Start()
        {
            if (null == prefab)
            {
                Debug.LogError($"{this} I need a prefab set", this);
                Debug.DebugBreak();
            }

            ExpandCapacity(initialCount);
        }

        /// <summary>
        /// Allocates enough new objects to allow <see cref="newMaxCapacity"/> objects.  Does nothing if
        /// <see cref="newMaxCapacity"/> is less than or equal to the existing number of objects.
        /// </summary>
        /// <param name="newMaxCapacity"></param>
        private void ExpandCapacity(int newMaxCapacity)
        {
            if (null == prefab)
            {
                Debug.LogError($"{this} I need a prefab set", this);
                return;
            }

            var min = pool.Count;
            var max = newMaxCapacity;

            if (max <= min)
            {
                Debug.LogError($"invalid value for newMaxCapacity: {newMaxCapacity}", this);
                return;
            }

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

            _numberOfNumberOfObjectsInPlay = 0;
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

                _numberOfNumberOfObjectsInPlay++;

                return myInstance.myComponent;
            }

            if (!allowExpansion) return default;

            var newCapacity = currentPoolCount * 2;
            Debug.Log($"{this}.GetAvailableObject(): expanding capacity from {currentPoolCount} to {newCapacity}");
            ExpandCapacity(newCapacity);

            myInstance = pool[i];
            myInstance.gameObject.SetActive(true);
            myInstance.myComponent.ResetNonStaticData();

            _numberOfNumberOfObjectsInPlay++;

            return myInstance.myComponent;
        }

        public virtual void ReturnObject(GameObject go)
        {
            go.SetActive(false);
            _numberOfNumberOfObjectsInPlay--;

            if (_numberOfNumberOfObjectsInPlay == 0) GlobalEvents.LastBallDestroyed?.Invoke();

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
