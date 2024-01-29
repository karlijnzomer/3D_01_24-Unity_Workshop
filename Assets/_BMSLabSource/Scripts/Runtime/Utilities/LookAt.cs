using UnityEngine;

namespace _BMSLabSource.Scripts.Runtime.Utilities
{
    public class LookAt : UtilityBase
    {

        private void LateUpdate()
        {
            if (_active && _target != null)
            {
                // Calculate the direction to the target
                Vector3 direction = _target.position - transform.position;

                // Calculate the rotation to look at the target
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                // Apply the rotation to make the GameObject look at the target
                transform.rotation = lookRotation;
            }
        }
    }
}