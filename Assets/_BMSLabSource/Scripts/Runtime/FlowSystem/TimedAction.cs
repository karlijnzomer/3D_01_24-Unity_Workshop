using System.Collections;
using UnityEngine;

namespace _BMSLabSource.Scripts.Runtime.FlowSystem
{
    /// <summary>
    /// An individual action throws events when it begins and when it is completed.
    /// Actions are managed by a FlowManager.
    /// A timed action extends ActionBase and completes based upon a timer.
    /// </summary>
    public class TimedAction : ActionBase
    {
        // TODO: Error when GO is deactivated: "Coroutine couldn't be started because the the game object 'TimedAction (trust value has increased) (ACTIVE)' is inactive!"

        [Header("Configuration")]
        [SerializeField, Tooltip("The time (in seconds) it takes for this action to auto-complete.")]
        private float _waitTime = 2f;

        public override void Initiate()
        {
            base.Initiate();
            StartCoroutine(StartTimer());
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSeconds(_waitTime);
            Complete();
        }

    }
}
