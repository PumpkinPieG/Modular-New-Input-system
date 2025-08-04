using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{


    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string actionMapName = "Player"; // the further left in the Input Action
    [SerializeField] private string actionName = "Interact"; // the middle in the input action

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
        _interactAction.performed += OnInteract;
    }

    private void OnDisable()
    {
        if (_interactAction != null)
        {
            _interactAction.performed -= OnInteract;
            _interactAction.Disable();
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact triggered.");
        // Do interaction logic here
    }
}
