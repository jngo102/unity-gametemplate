using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindUI : MonoBehaviour {
    [SerializeField] private string actionName;
    [SerializeField] private bool isComposite;
    [SerializeField] private string compositeName;
    [SerializeField] private TextMeshProUGUI keyLabel;
    [SerializeField] private TextMeshProUGUI keyText;

    private int bindingIndex = -1;
    private InputAction inputAction;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    
    private void Start() {
        inputAction = InputManager.Instance.InputActions.Player.Get().actions.First(action => action.name == actionName);
        if (isComposite) {
            bindingIndex = inputAction.bindings.IndexOf(binding => binding.isPartOfComposite && binding.name == compositeName.ToLower());
        } else {
            bindingIndex = inputAction.bindings.IndexOf(binding => !binding.isComposite);
        }
        
        keyLabel.text = isComposite ? compositeName : actionName;
        keyText.text = InputControlPath.ToHumanReadableString(
            inputAction.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void StartRebind() {
        inputAction.Disable();
        keyText.text = "Listening...";
        rebindOperation = inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnMatchWaitForAnother(0.25f)
            .OnComplete(RebindComplete)
            .Start();
    }

    private void RebindComplete(InputActionRebindingExtensions.RebindingOperation operation) {
        keyText.text = operation.selectedControl.displayName;
        operation.Dispose();
        operation.action.Enable();
    }
}
