using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _movementCurve;

    private Vector3 _originalPos;

    private bool _gotOriginalPosition = false;

    private void OnEnable()
    {
        if (_gotOriginalPosition == true)
            return;

        _originalPos = transform.localPosition;
        _gotOriginalPosition = true;
    }
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, _originalPos.y + _movementCurve.Evaluate((Time.time % _movementCurve.length)), transform.localPosition.z);
    }
}