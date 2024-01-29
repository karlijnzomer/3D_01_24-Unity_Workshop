using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class LocomotionManager : MonoBehaviour
{
    //to do: create function to disable teleport on grab
    //to do: make sure the providers are active as needed

    public enum LocomotionType
    {
        Teleport,
        Continuous,
        None,
    }

    public enum TurnType
    {
        SnapTurn,
        ContinuousTurn,
        None,
    }

    [Serializable]
    public class ControllerConfig
    {
        public LocomotionType LocomotionType = new();
        public TurnType TurnType = new();
        public ActionBasedControllerManager Manager { get; set; }
    }

    [Header("References")]
    [SerializeField]
    private LocomotionSystem _locomotionSystem;
    [SerializeField]
    private ActionBasedControllerManager _leftControllerManager;
    [SerializeField]
    private ActionBasedControllerManager _rightControllerManager;

    [Space(10)]
    [SerializeField]
    private GameObject _leftControllerTeleportInteractor;
    [SerializeField]
    private GameObject _rightControllerTeleportInteractor;

    [Space(20)]
    [Header("Configuration")]
    [SerializeField, Tooltip("Log messages to the console.")]
    private bool _debug = false;
    [SerializeField]
    private ControllerConfig _leftController;
    [SerializeField]
    private ControllerConfig _rightController;
    [SerializeField]
    private bool _enableStrafe;

    private DynamicMoveProvider _dynamicMoveProvider;
    private TeleportationProvider _teleportationProvider;
    private ActionBasedContinuousTurnProvider _continuousTurnProvider;
    private ActionBasedSnapTurnProvider _snapTurnProvider;

    private LocomotionProvider[] _locomotionProviders;
    private string[] _locomotionProviderNames;

    private void Awake()
    {
        _leftController.Manager = _leftControllerManager;
        _rightController.Manager = _rightControllerManager;

        _dynamicMoveProvider = _locomotionSystem.GetComponentInChildren<DynamicMoveProvider>();
        _teleportationProvider = _locomotionSystem.GetComponentInChildren<TeleportationProvider>();
        _continuousTurnProvider = _locomotionSystem.GetComponentInChildren<ActionBasedContinuousTurnProvider>();
        _snapTurnProvider = _locomotionSystem.GetComponentInChildren<ActionBasedSnapTurnProvider>();

        _locomotionProviders = new LocomotionProvider[] { _dynamicMoveProvider, _teleportationProvider, _continuousTurnProvider, _snapTurnProvider };
        _locomotionProviderNames = new string[] { "Move", "Teleport", "Continuous Turn", "Snap Turn" };
    }

    private void Start()
    {
        SwitchLocomotionConfig(_leftController);
        SwitchLocomotionConfig(_rightController);
        SwitchLocomotionProvider(_leftController, _rightController);
        CheckLocomotionProviders(_locomotionProviders, _locomotionProviderNames);
        _dynamicMoveProvider.enableStrafe = _enableStrafe;
    }

    private void SwitchLocomotionConfig(ControllerConfig controller)
    {
        string controllerName = controller.Manager.name;

        switch (controller.LocomotionType)
        {
            case LocomotionType.Teleport:

                DisableSmoothMotion(controller);
                if (_debug)
                    Debug.Log(controllerName + ": Locomotion type Teleport active", gameObject);
                break;
            case LocomotionType.Continuous:
                EnableSmoothMotion(controller);
                if (_debug)
                    Debug.Log(controllerName + ": Locomotion type Continuous active", gameObject);
                break;
            case LocomotionType.None:
                DisableSmoothMotion(controller);
                DisableControllerTeleportInteractor(controllerName);
                if (_debug)
                    Debug.Log(controllerName + ": Locomotion type None active", gameObject);
                break;
        }

        switch (controller.TurnType)
        {
            case TurnType.ContinuousTurn:
                EnableSmoothTurn(controller);
                if (_debug)
                    Debug.Log(controllerName + ": Continuous turn active", gameObject);
                break;
            case TurnType.SnapTurn:
                DisableSmoothTurn(controller);
                if (_debug)
                    Debug.Log(controllerName + ": Snap turn active", gameObject);
                break;
            case TurnType.None:
                DisableControllerTurn(controllerName);
                DisableSmoothTurn(controller);
                if (_debug)
                    Debug.Log(controllerName + ": None active", gameObject);
                break;
        }
    }

    private void DisableControllerTeleportInteractor(string controllerName)
    {
        if (controllerName.Contains("Left"))
        {
            if (_debug)
                Debug.Log("Left teleport disabled.", gameObject);
            _leftControllerTeleportInteractor.SetActive(false);
            _leftControllerTeleportInteractor.GetComponent<XRRayInteractor>().enabled = false;
        }
        else if (controllerName.Contains("Right"))
        {
            if (_debug)
                Debug.Log("Right teleport disabled.", gameObject);
            _rightControllerTeleportInteractor.GetComponent<XRRayInteractor>().enabled = false;
        }
    }

    private void DisableControllerTurn(string controllerName)
    {
        // assumes naming does not change
        if (controllerName.Contains("Left"))
        {
            if (_debug)
                Debug.Log("Left turn disabled.", gameObject);
            _snapTurnProvider.leftHandSnapTurnAction.action.Disable();
        }
        else if (controllerName.Contains("Right"))
        {
            if (_debug)
                Debug.Log("Right turn disabled.", gameObject);
            _snapTurnProvider.rightHandSnapTurnAction.action.Disable();
        }
    }

    private void SwitchLocomotionProvider(ControllerConfig leftController, ControllerConfig rightController)
    {
        if (leftController.LocomotionType == LocomotionType.None && rightController.LocomotionType == LocomotionType.None)
        {
            _dynamicMoveProvider.enabled = false;
        }

        if (leftController.TurnType == TurnType.None && rightController.TurnType == TurnType.None)
        {
            _continuousTurnProvider.enabled = false;
            _snapTurnProvider.enabled = false;
        }
    }

    private void EnableSmoothMotion(ControllerConfig controller)
    {
        controller.Manager.smoothMotionEnabled = true;
    }

    private void DisableSmoothMotion(ControllerConfig controller)
    {
        controller.Manager.smoothMotionEnabled = false;
    }

    private void EnableSmoothTurn(ControllerConfig controller)
    {
        controller.Manager.smoothTurnEnabled = true;
    }

    private void DisableSmoothTurn(ControllerConfig controller)
    {
        controller.Manager.smoothTurnEnabled = false;
    }

    private void CheckLocomotionProviders(LocomotionProvider[] providers, string[] names)
    {
        for (int i = 0; i < providers.Length; i++)
        {
            IsProviderActive(providers[i], names[i]);
        }
    }

    private bool IsProviderActive(LocomotionProvider provider, string name)
    {
        if (provider != null && provider.enabled && provider.gameObject != null && provider.gameObject.activeInHierarchy)
        {
            return true;
        }
        else
        {
            throw new InvalidOperationException(name + " Locomotion Provider is not active in the scene.");
        }
    }

}


