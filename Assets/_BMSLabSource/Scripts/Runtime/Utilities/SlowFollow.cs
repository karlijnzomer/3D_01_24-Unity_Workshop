using UnityEngine;

namespace _BMSLabSource.Scripts.Runtime.Utilities
{
    public class SlowFollow : UtilityBase
    {
        public float SmoothSpeed = 1.5f; // The speed of following (lower values make it smoother)

        private void LateUpdate()
        {
            if (_active == true && _target != null)
            {
                // Calculate the desired position by interpolating between the current position and the target's position
                Vector3 desiredPosition = new(_target.transform.position.x, transform.position.y, _target.transform.position.z);

                // Use SmoothDamp to smoothly move to the desired position
                transform.position = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed * Time.deltaTime);
            }
        }
    }
}