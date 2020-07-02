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

        [SerializeField] public Vector3 pos;
        [SerializeField] public CollideOnlyOnceData collideOnlyOnceData;
        [SerializeField] public int spawnCount;
        [SerializeField] public float radius;

        public static BallSpawnPoint AddNewInstance(GameObject go,
            Vector3 pos, CollideOnlyOnceData collideOnlyOnceData, int spawnCount = 1, float radius = 0.1f)
        {
            var ballSpawnPoint = go.AddComponent<BallSpawnPoint>();
            ballSpawnPoint.pos = pos;
            ballSpawnPoint.collideOnlyOnceData = collideOnlyOnceData;
            ballSpawnPoint.spawnCount = spawnCount;
            ballSpawnPoint.radius = radius;

            return ballSpawnPoint;
        }

        // private void Start()
        // {
        //     var ball = BallPool.Instance.GetAvailableObject();
        //     if (ball == null) return;
        //     
        //     ball.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        //     ball.UpdateCollideOnlyOnceDataFromExistingData(collideOnlyOnceData);
        // }

        private void Update()
        {
            if (spawnCount == 0)
            {
                Destroy(this);
                return;
            }

            var layerMask = 1 >> LayerMask.NameToLayer("Default");

            var ballsToSpawnThisFrame = Math.Min(spawnCount, maxBallsToGeneratePerFrame);
            spawnCount -= ballsToSpawnThisFrame;

            for (var i = 0; i < ballsToSpawnThisFrame; i++)
            {
                var newBall = BallPool.Instance.GetAvailableObject();
                if (null == newBall)
                {
                    break;
                }

                var newPos = new Vector3();
                const int maxIteration = 10;
                int j;
                for (j = 0; j < maxIteration; j++)
                {
                    newPos = Random.onUnitSphere;
                    newPos.z = 0;
                    newPos *= radius;
                    newPos += pos;

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
        }
    }
}