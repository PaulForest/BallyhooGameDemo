using System;
using System.Collections.Generic;
using Balls;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayObjects
{
    class SpawnBallData
    {
        public Vector3 pos;
        public CollideOnlyOnceData collideOnlyOnceData;
        public int spawnCount;
        public float radius;
    }

    [RequireComponent(typeof(CollideOnlyOncePlayerBall), typeof(TMP_Text))]
    public class BallSplitter : MonoBehaviour
    {
        [Header("How many balls will this split into?")] [SerializeField]
        protected int mSplitCount;

        [SerializeField] private CollideOnlyOncePlayerBall collideOnlyOnce;
        [SerializeField] private TMP_Text splitCountLabel;
        [SerializeField] private int maxBallsToGeneratePerFrame = 5;

        private readonly List<SpawnBallData> _spawnBallData = new List<SpawnBallData>();

        private void Start()
        {
            collideOnlyOnce = GetComponent<CollideOnlyOncePlayerBall>();
            collideOnlyOnce.onCollisionEvent.AddListener(OnCollisionEvent);

            if (!splitCountLabel) splitCountLabel = GetComponent<TMP_Text>();
            if (!splitCountLabel) splitCountLabel = GetComponentInChildren<TMP_Text>();
            if (!splitCountLabel)
            {
                Debug.LogError("I need a text field set", this);
                return;
            }

            splitCountLabel.text = $"{mSplitCount} X";
        }

        private void OnDestroy()
        {
            collideOnlyOnce.onCollisionEvent.RemoveListener(OnCollisionEvent);
        }

        private void Update()
        {
            if (_spawnBallData.Count == 0) return;

            foreach (var spawnBallData in _spawnBallData)
            {
                if (spawnBallData.spawnCount == 0)
                {
                    // if (!spawnBallData.original)
                    // {
                    //     break;
                    // }

                    // Destroy(spawnBallData.original);
                    // spawnBallData.original = null;
                    break;
                }

                var layerMask = 1 >> LayerMask.NameToLayer("Default");

                var ballsToSpawnThisFrame = Math.Min(spawnBallData.spawnCount, maxBallsToGeneratePerFrame);
                spawnBallData.spawnCount -= ballsToSpawnThisFrame;

                for (var i = 0; i < ballsToSpawnThisFrame; i++)
                {
                    var newBall = BallPool.Instance.GetAvailableObject();
                    if (null == newBall)
                    {
                        break;
                    }

                    var pos = new Vector3();
                    const int maxIteration = 10;
                    int j;
                    for (j = 0; j < maxIteration; j++)
                    {
                        pos = Random.onUnitSphere;
                        pos.z = 0;
                        pos *= spawnBallData.radius;
                        pos += spawnBallData.pos;

                        if (!Physics.CheckSphere(pos, spawnBallData.radius, layerMask))
                        {
                            break;
                        }
                    }

                    if (j >= maxIteration)
                    {
                        Debug.LogWarning($"Too many iterations: {j}, max {maxIteration}", this);
                    }

                    newBall.transform.SetPositionAndRotation(pos, Quaternion.identity);

                    collideOnlyOnce.UpdateFromData(spawnBallData.collideOnlyOnceData);
                    collideOnlyOnce.SetCannotCollideWithT(newBall);
                }
            }
        }

        private void OnCollisionEvent(PlayerBall ball)
        {
            DoTheSplits(ball);
        }

        /// <summary>
        /// Clones the <see cref="originalBall"/> <see cref="mSplitCount"/> times.
        /// We're doing the splits, and there are balls around.  There's a Jean-Claude Van Damme joke in there somewhere. :)
        /// </summary>
        /// <param name="originalBall"/>
        private void DoTheSplits(PlayerBall originalBall)
        {
            if (!collideOnlyOnce.CanCollideWithBall(originalBall)) return;

            collideOnlyOnce.SetCannotCollideWithT(originalBall);

            // BallPool.Instance.GetAvailableObject()
            // var go = GameObject.Instantiate(originalBall.gameObject, transform.position, Quaternion.identity);
            // go.SetActive(false);

            _spawnBallData.Add(item: new SpawnBallData
            {
                pos = originalBall.transform.position,
                collideOnlyOnceData = collideOnlyOnce.GetData(),
                spawnCount = mSplitCount,
                radius = originalBall.transform.localScale.x
            });

            GlobalEvents.BallSplitEvent?.Invoke(originalBall, this);
        }
    }
}