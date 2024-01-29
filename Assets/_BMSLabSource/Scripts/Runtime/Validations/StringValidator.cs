using UnityEngine;
using UnityEngine.Events;

public class StringValidator : MonoBehaviour
{
    [SerializeField]
    private string _description;

    public OperatorType Operator;

    [SerializeField]
    public string _currentValue;

    [SerializeField]
    private string _referenceValue;
    public enum OperatorType { EQUAL };

    public UnityEvent ValidatationValid, ValidationInvalid;

    public void Validate()
    {
        switch (Operator)
        {
            case OperatorType.EQUAL:
                if (_currentValue == _referenceValue)
                {
                    ValidatationValid.Invoke();
                }
                else
                {
                    ValidationInvalid.Invoke();
                }
                break;
        }
    }

    public void SetCurrentValue(string value)
    {
        _currentValue = value;
    }
}
