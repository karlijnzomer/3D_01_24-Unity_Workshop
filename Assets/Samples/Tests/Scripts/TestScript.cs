using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public int Health = 100;
    public float Speed = 85.2f;
    [SerializeField]
    private float _accelerationRate = 12.5f;

    private void Awake()
    {
        if (_accelerationRate == 0)
        {
            _accelerationRate = 1;
        }

    }
}
