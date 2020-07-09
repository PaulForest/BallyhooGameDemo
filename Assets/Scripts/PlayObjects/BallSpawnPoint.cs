using System;
using Balls;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayObjects
{
    public class BallSpawnPoint : MonoBehaviour
    {
        [Header("At most, this many balls will be generated per frame")] [SerializeField]
        private int maxBallsToGeneratePerFrame = 5;

        [SerializeField] public CollideOnlyOnceData collideOnlyOnceData;
        [SerializeField] public int spawnCount = 1;
        [SerializeField] public float radius = 0.1f;
        
        public static BallSpawnPoint AddNewInstance(GameObject go,
            Vector3 pos, CollideOnlyOnceData collideOnlyOnceData, int spawnCount, float radius)
        {
            var ballSpawnPoint = go.AddComponent<BallSpawnPoint>();
            ballSpawnPoint.collideOnlyOnceData = collideOnlyOnceData;
            ballSpawnPoint.spawnCount = spawnCount;
            ballSpawnPoint.radius = radius;

            return ballSpawnPoint;
        }

        private void Update()
        {
            if (spawnCount == 0)
            {
                Destroy(this);
                return;
            }

            var layerMask = 1 << LayerMask.NameToLayer("Default");

            var ballsToSpawnThisFrame = Math.Min(spawnCount, maxBallsToGeneratePerFrame);
            spawnCount -= ballsToSpawnThisFrame;

            var ballPool = BallPool.Instance;
            for (var i = 0; i < ballsToSpawnThisFrame; i++)
            {
                var newBall = ballPool.GetAvailableObject();
                if (null == newBall)
                {
                    Debug.LogError($"Could not get a new object from the pool");
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

                    if (!Physics.CheckSphere(newPos, radius, layerMask))
                    {
                        break;
                    }
                }

                if (j >= maxIteration)
                {
                    Debug.LogWarning($"Too many iterations: {j}, max {maxIteration}", this);
                    newPos = transform.position;
                }

                newBall.transform.SetPositionAndRotation(newPos, Quaternion.identity);
                newBall.UpdateCollideOnlyOnceDataFromExistingData(collideOnlyOnceData);
            }

            if (spawnCount != 0) return;
            Destroy(this);
        }
    }
}
