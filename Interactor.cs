using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    //interaction system specific, remove in other systems.
    public interface IInteractable
    {
        void Interact();
    }

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
        _interactAction.performed += OnInteractInput;
    }

    private void OnDisable()
    {
        if (_interactAction != null)
        {
            _interactAction.performed -= OnInteractInput;
            _interactAction.Disable();
        }
    }

    //interaction system specific, remove in other systems.
    private void OnInteractInput(InputAction.CallbackContext ctx)
    {
        Collider[] hits = Physics.OverlapSphere(interactorSource.position, interactRange);

        foreach (var hit in hits)
        {
            // Skip self
            if (hit.transform == interactorSource)
                continue;

            // Try to interact
            if (hit.gameObject.TryGetComponent<IInteractable>(out var interactObj))
            {
                if(interactObj.GetType().GetMethod("Interact") != null)
                {
                    interactObj.Interact();
                }
                
             
                
                
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (interactorSource == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactorSource.position, interactRange);
    }

    
}
