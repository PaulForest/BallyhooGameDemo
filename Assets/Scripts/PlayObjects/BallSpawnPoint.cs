using System;
using System.Threading.Tasks;
using Balls;
using Level;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayObjects
{
    public class BallSpawnPoint : MonoBehaviour, IResettableNonStaticData
    {
        private CollideOnlyOnceData collideOnlyOnceData;

        [Header("At most, this many balls will be generated per frame")] [SerializeField]
        private int maxBallsToGeneratePerFrame = 5;

        [SerializeField] private float radius = 0.1f;
        [SerializeField] private int spawnCount = 1;

        public enum SpawnBehaviour
        {
            SpawnImmediately,
            WaitForLevelStart
        }

        [SerializeField] private SpawnBehaviour spawnBehaviour = SpawnBehaviour.SpawnImmediately;

        private int _localSpawnCount = 1;

        private void Start()
        {
            switch (spawnBehaviour)
            {
                case SpawnBehaviour.SpawnImmediately:
                    SpawnOverTime();
                    break;
                case SpawnBehaviour.WaitForLevelStart:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnEnable()
        {
            switch (spawnBehaviour)
            {
                case SpawnBehaviour.SpawnImmediately:
                    break;
                case SpawnBehaviour.WaitForLevelStart:
                    GlobalEvents.LevelStart.AddListener(OnLevelStart);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDisable()
        {
            if (spawnBehaviour == SpawnBehaviour.WaitForLevelStart)
            {
                GlobalEvents.LevelStart.RemoveListener(OnLevelStart);
            }
        }

        private void OnLevelStart(LevelData arg0)
        {
            SpawnOverTime();
        }

        public static BallSpawnPoint AddNewInstance(GameObject go,
            Vector3 pos, CollideOnlyOnceData collideOnlyOnceData, int spawnCount, float radius,
            SpawnBehaviour spawnBehaviour)
        {
            go.transform.position = pos;

            var ballSpawnPoint = go.AddComponent<BallSpawnPoint>();
            ballSpawnPoint.collideOnlyOnceData = collideOnlyOnceData;
            ballSpawnPoint.spawnCount = spawnCount;
            ballSpawnPoint.radius = radius;
            ballSpawnPoint.spawnBehaviour = spawnBehaviour;

            return ballSpawnPoint;
        }

        private async void SpawnOverTime()
        {
            _localSpawnCount = spawnCount;
            var layerMask = 1 << LayerMask.NameToLayer("Default");

            while (_localSpawnCount > 0)
            {
                var ballsToSpawnThisFrame = Math.Min(_localSpawnCount, maxBallsToGeneratePerFrame);
                _localSpawnCount -= ballsToSpawnThisFrame;

                var ballPool = BallPool.Instance;
                for (var i = 0; i < ballsToSpawnThisFrame; i++)
                {
                    var newBall = ballPool.GetAvailableObject();
                    if (null == newBall)
                    {
                        Debug.LogError("Could not get a new object from the pool");
                        break;
                    }

                    var newPos = new Vector3();
                    const int maxIteration = 10;
                    int j;
                    for (j = 0; j < maxIteration; j++)
                    {
                        newPos = Random.onUnitSphere;
                        newPos.z = 0;
                        newPos *= radius * 2;
                        newPos += transform.position;

                        if (!Physics.CheckSphere(newPos, radius, layerMask)) break;
                    }

                    if (j >= maxIteration)
                    {
                        Debug.LogWarning($"Too many iterations: {j}, max {maxIteration}", this);
                        newPos = transform.position;
                    }

                    newBall.transform.SetPositionAndRotation(newPos, Quaternion.identity);
                    newBall.UpdateCollideOnlyOnceDataFromExistingData(collideOnlyOnceData);

                    if (_localSpawnCount != 0)
                    {
                        await Task.Yield();
                    }
                }
            }
        }

        public void ResetNonStaticData()
        {
            SpawnOverTime();
        }
    }
}
