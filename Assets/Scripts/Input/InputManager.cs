using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager> {
    [SerializeField] private float inputBufferTime = 0.15f;

    private PlayerInputActions inputActions;
    
    public BufferedInputAction Jump;

    [NonSerialized] public InputAction Move;
    [NonSerialized] public InputAction Cancel;

    protected override void OnAwake() {
        SetupInputActions();
    }

    private void OnEnable() {
        inputActions.Enable();
    }

    private void OnDisable() {
        inputActions.Disable();
    }

    private void SetupInputActions() {
        inputActions = new PlayerInputActions();
        
        Jump = new BufferedInputAction(inputActions.Player.Jump, inputBufferTime);

        Move = inputActions.Player.Move;

        Cancel = inputActions.UI.Cancel;
    }
}
