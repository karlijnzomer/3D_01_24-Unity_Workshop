using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody), typeof(XRGrabInteractable))]
public class BasketController : MonoBehaviour
{
    //to do: constraint product when in basket
    //to do: check if randomized position works with scaling products, if beyond a speecific scale just center it

    [Header("References")]
    [SerializeField]
    private GameObject _shoppedProductsHolder;

    [Header("Configuration")]
    [SerializeField, Tooltip("Toggle to print console messages.")]
    private bool _debug = false;
    [SerializeField, Range(0.1f, 2.5f), Tooltip("The duration it takes for the product to shrink from its original size to negative infinity, when it is being shopped.")]
    private float _shrinkDuration = 1.5f;

    [Header("Events")]
    public UnityEvent<XRGrabInteractable> OnProductHoverEntered = new();
    public UnityEvent<XRGrabInteractable> OnProductHoverExited = new();
    public UnityEvent<string> OnProductShopped = new();

    private int _productLayer;
    private Rigidbody _rigidbody;
    private XRGrabInteractable _xrGrabInteractable;
    private XRGrabInteractable _selectedProduct = null;
    private bool _permanentAttach;
    private bool _destroyProduct = true;

    private void Awake()
    {
        _productLayer = LayerMask.NameToLayer("Product");
        _rigidbody = GetComponent<Rigidbody>();
        _xrGrabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        if (_permanentAttach)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
            DisableXRGrabbable();
        }
        else
        {
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
            EnableXRGrabbable();
        }
    }

    public void Initialize(bool permanentAttach, bool destroyProduct)
    {
        _permanentAttach = permanentAttach;
        _destroyProduct = destroyProduct;
    }

    private void DisableXRGrabbable()
    {
        _xrGrabInteractable.enabled = false;
    }

    private void EnableXRGrabbable()
    {
        _xrGrabInteractable.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        RegisterHoveringProduct(other);
    }

    private void RegisterHoveringProduct(Collider other)
    {
        if (other.gameObject.layer == _productLayer)
        {
            var hoveredGrabInteractable = other.GetComponent<XRGrabInteractable>();

            if (hoveredGrabInteractable != null && hoveredGrabInteractable.isSelected)
            {
                _selectedProduct = hoveredGrabInteractable;
                OnProductHoverEntered?.Invoke(_selectedProduct);

                _selectedProduct.selectExited.AddListener(OnProductShoppped);
            }
        }
    }

    /// <summary>
    /// When a currently selected (held) product is being unselected (released),
    /// while the product is hovering the basket, then we want to 'shop' it.
    /// Thus, the shrink method is called and the event that the product has been shopped is invoked.
    /// </summary>
    /// <param name="args">The arguments that are sent via the SelectExit event.</param>
    private void OnProductShoppped(SelectExitEventArgs args)
    {
        var product = args.interactableObject.transform.gameObject;

        if (_destroyProduct)
        {
            Destroy(product);
        }
        else
        {
            product.GetComponent<XRGrabInteractable>().enabled = false;
            product.transform.SetParent(_shoppedProductsHolder.transform, true);

            ShrinkGameObject(product, _shrinkDuration);
            SetKinematic(product.GetComponent<Rigidbody>());
        }

        if (_debug)
            Debug.Log(product.name + " has been shopped!", gameObject);

        OnProductShopped?.Invoke(product.name);

    }

    private void OnTriggerExit(Collider other)
    {
        DeregisterHoveringProduct(other);
    }

    private void DeregisterHoveringProduct(Collider other)
    {
        if (other.gameObject.layer == _productLayer)
        {
            if (other.gameObject.GetComponent<XRGrabInteractable>() == _selectedProduct)
            {
                OnProductHoverExited?.Invoke(_selectedProduct);

                if (_selectedProduct == null)
                    return;

                _selectedProduct.selectExited?.RemoveListener(OnProductShoppped);

                _selectedProduct = null;
            }
        }
    }

    private void SetKinematic(Rigidbody rigidBody)
    {
        rigidBody.isKinematic = true;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.detectCollisions = false;
    }

    private void ShrinkGameObject(GameObject product, float duration)
    {
        StartCoroutine(LerpScaleOverTime(product, duration));
    }

    private IEnumerator LerpScaleOverTime(GameObject product, float duration)
    {
        Vector3 startScale = product.transform.localScale;
        Vector3 startPosition = product.transform.localPosition;
        Vector3 target = Vector3.zero;

        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float smoothedValue = Mathf.SmoothStep(0f, 1f, timeElapsed / duration);
            product.transform.localScale = Vector3.Lerp(startScale, target, smoothedValue);
            product.transform.localPosition = Vector3.Lerp(startPosition, target, smoothedValue);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        product.transform.localScale = target; //Lerp does not always reach target value
        product.transform.SetLocalPositionAndRotation(target, Quaternion.identity);

        product.SetActive(false);
    }

}
