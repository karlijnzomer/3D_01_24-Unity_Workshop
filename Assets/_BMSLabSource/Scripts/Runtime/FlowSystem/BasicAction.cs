using MyBox;
using UnityEngine;

namespace _BMSLabSource.Scripts.Runtime.FlowSystem
{
    /// <summary>
    /// An individual action throws events when it begins and when it is completed.
    /// Actions are managed by a FlowManager.
    /// A basic action extends ActionBase and completes based upon a completion counter.
    /// </summary>
    public class BasicAction : ActionBase
    {
        [Header("Configuration")]
        [SerializeField, ReadOnly]
        private int _currentCount = 0;
        [SerializeField]
        private int _completionCount = 1;

        public override void Initiate()
        {
            base.Initiate();
            CheckForCompletion();
        }

        [ButtonMethod]
        public void IncrementCount()
        {
            _currentCount++;

            CheckForCompletion();
        }

        private void CheckForCompletion()
        {
            if (_currentCount >= _completionCount)
            {
                _currentCount = _completionCount;
                Complete();
            }
        }
    }
}
