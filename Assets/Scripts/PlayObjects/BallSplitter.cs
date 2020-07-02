using System;
using System.Collections.Generic;
using Balls;
using TMPro;
using UnityEngine;

namespace PlayObjects
{
    [RequireComponent(typeof(CollideOnlyOncePlayerBall), typeof(TMP_Text))]
    public class BallSplitter : MonoBehaviour
    {
        [Header("How many balls will this create?")] [SerializeField]
        protected int mSplitCount;

        [SerializeField] private CollideOnlyOncePlayerBall collideOnlyOnce;
        [SerializeField] private TMP_Text splitCountLabel;

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

            BallSpawnPoint.AddNewInstance(gameObject, transform.position, collideOnlyOnce.GetData(),
                mSplitCount, originalBall.transform.localScale.x);

            // BallPool.Instance.GetAvailableObject()
            // var go = GameObject.Instantiate(originalBall.gameObject, transform.position, Quaternion.identity);
            // go.SetActive(false);

            // var ballSpawnPoint = gameObject.AddComponent<BallSpawnPoint>();
            // ballSpawnPoint.

            // _spawnBallData.Add(item: new SpawnBallData
            // {
            //     pos = originalBall.transform.position,
            //     collideOnlyOnceData = collideOnlyOnce.GetData(),
            //     spawnCount = mSplitCount,
            //     radius = originalBall.transform.localScale.x
            // });

            GlobalEvents.BallSplitEvent?.Invoke(originalBall, this);
        }
    }
}