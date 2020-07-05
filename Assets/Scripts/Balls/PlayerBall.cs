using PhysSound;
using UnityEngine;

namespace Balls
{
    [RequireComponent(typeof(PhysSoundObject))]
    public class PlayerBall : OnlyTouchOnce, IPoolableObject
    {

        private PhysSoundObject _physSoundObject;

        private void Awake()
        {
            _physSoundObject = GetComponent<PhysSoundObject>();
            if (!_physSoundObject)
            {
                Debug.LogError($"{this}: I need a PhysSoundObject component", this);
            }
        }
        
        private void OnEnable()
        {
            NumberOfNumberOfBallsInPlay++;
            ResetOnlyTouchOnceData();
            
            _physSoundObject.SetEnabled(true);
        }

        private void OnDisable()
        {
            NumberOfNumberOfBallsInPlay--;
            GlobalEvents.BallDestroyed?.Invoke(this);

            if (!HasBallsInPlay)
            {
                GlobalEvents.LastBallDestroyed?.Invoke();
            }
            _physSoundObject.SetEnabled(false);
        }

        private static bool HasBallsInPlay => NumberOfNumberOfBallsInPlay > 0;
        private static int NumberOfNumberOfBallsInPlay { get; set; }
    }
}
