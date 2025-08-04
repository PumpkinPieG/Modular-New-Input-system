using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public interface IInteractable
    {
        void Interact();
    }

    public bool isInteracting = false;
    public Transform interactorSource;
    public float interactRange;

    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string actionMapName = "Player";
    [SerializeField] private string actionName = "Interact";

    private InputAction _interactAction;

    private void OnEnable()
    {
        var map = inputActions.FindActionMap(actionMapName);
        if (map == null)
        {
            Debug.LogError($"Action Map '{actionMapName}' not found.");
            return;
        }

        _interactAction = map.FindAction(actionName);
        if (_interactAction == null)
        {
            Debug.LogError($"Action '{actionName}' not found in '{actionMapName}'.");
            return;
        }

        _interactAction.Enable();

        _interactAction.performed += ctx => isInteracting = true;
        _interactAction.canceled += ctx => isInteracting = false;
    }

    private void OnDisable()
    {
        if (_interactAction != null)
        {
            _interactAction.performed -= ctx => isInteracting = true;  // This won’t unsubscribe properly!
            _interactAction.canceled -= ctx => isInteracting = false;  // This won’t unsubscribe properly!
            _interactAction.Disable();
        }
    }

    private void Update()
    {
        if (isInteracting)
        {
            Ray r = new Ray(interactorSource.position, interactorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
}
