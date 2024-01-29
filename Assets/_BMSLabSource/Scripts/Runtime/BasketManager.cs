using MyBox;
using UnityEngine;

public class BasketManager : MonoBehaviour
{
    // todo: set basket to left/right hand preset if needed
    // todo: implement relevant Debug.Logs + _debug bool
    public enum Hand
    {
        LeftHand,
        RightHand,
    }

    [Header("References")]
    [MustBeAssigned]
    public GameObject Basket;
    [MustBeAssigned]
    public GameObject LeftController;
    [MustBeAssigned]
    public GameObject RightController;

    [Space(20)]
    [Header("Configuration")]
    [SerializeField, Tooltip("Select true, if the basket should be permanently attached to the primary hand. It will override the existing parent.")]
    private bool _permanentAttach;
    [ConditionalField(nameof(_permanentAttach)), Tooltip("If permanent attach is true, the basket will be attached to the primary hand.")]
    public Hand PrimaryHand;
    [SerializeField, ConditionalField(nameof(_permanentAttach))]
    private Vector3 _localPositionOffset;
    [SerializeField, ConditionalField(nameof(_permanentAttach))]
    private Vector3 _localRotationOffset;

    [Space(10)]
    [SerializeField, Tooltip("Destroy products upon entering the basket's collider.")]
    private bool _destroyProductsOnEnter;

    private BasketController _basketController;
    private bool _startMethodExecuted = false;

    private void Awake()
    {
        _basketController = Basket.GetComponent<BasketController>();
        _basketController.Initialize(_permanentAttach, _destroyProductsOnEnter);
    }

    void Start()
    {
        SwitchBasketToPrimaryHand();
        ApplyLocalOffset();
        _startMethodExecuted = true;
    }

    private void OnValidate()
    {
        if (_startMethodExecuted)
        {
            ApplyLocalOffset();
        }
    }

    private void SwitchBasketToPrimaryHand()
    {
        if (_permanentAttach)
        {
            if (PrimaryHand == Hand.RightHand)
            {
                Basket.transform.SetParent(RightController.transform, false);
            }
            else if (PrimaryHand == Hand.LeftHand)
            {
                Basket.transform.SetParent(LeftController.transform, false);
            }
            ResetLocalTransform();
        }
    }

    private void ResetLocalTransform()
    {
        Basket.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    private void ApplyLocalOffset()
    {
        if (_permanentAttach)
        {
            Basket.transform.localPosition = _localPositionOffset;
            Basket.transform.localEulerAngles = _localRotationOffset;
        }
    }
}

