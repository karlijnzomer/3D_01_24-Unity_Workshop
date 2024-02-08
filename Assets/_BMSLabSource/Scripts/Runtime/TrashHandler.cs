using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class TrashHandler : MonoBehaviour
{
    public UnityEvent<string> OnTrashDiscarded;

    public void DiscardTrash(GameObject go)
    {
        if (go.GetComponentInChildren<XRGrabInteractable>() != null)
        {
            Debug.Log(go.name + " touched the trash can.");

            OnTrashDiscarded?.Invoke(go.name);

            ShrinkGameObject(go, 1.5f);
            SetKinematic(go.GetComponent<Rigidbody>());
        }
    }

    private void ShrinkGameObject(GameObject go, float duration)
    {
        StartCoroutine(LerpScaleOverTime(go, duration));
    }

    private void SetKinematic(Rigidbody rigidBody)
    {
        rigidBody.isKinematic = true;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.detectCollisions = false;
    }

    private IEnumerator LerpScaleOverTime(GameObject go, float duration)
    {
        Vector3 startScale = go.transform.localScale;
        Vector3 startPosition = go.transform.localPosition;
        Vector3 targetScale = Vector3.zero;

        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float smoothedValue = Mathf.SmoothStep(0f, 1f, timeElapsed / duration);
            go.transform.localScale = Vector3.Lerp(startScale, targetScale, smoothedValue);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        go.transform.localScale = targetScale; //Lerp does not always reach target value

        go.SetActive(false);
    }
}
