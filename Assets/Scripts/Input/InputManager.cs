using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Singleton that manages global inputs.
/// </summary>
public class InputManager : Singleton<InputManager>, IDataPersistence {
    /// <summary>
    /// The duration that an input may be buffered for.
    /// </summary>
    [SerializeField] private float inputBufferTime = 0.15f;

    /// <summary>
    /// The player input actions instance.
    /// </summary>
    public PlayerInputActions InputActions { get; private set; }

    /// <summary>
    /// The buffered input action for jumping.
    /// </summary>
    public BufferedInputAction Jump;

    /// <summary>
    /// The input action for moving.
    /// </summary>
    [NonSerialized] public InputAction Move;

    /// <summary>
    /// The input action for canceling an action.
    /// </summary>
    [NonSerialized] public InputAction Cancel;

    /// <summary>
    /// Whether the input manager is enabled.
    /// </summary>
    public bool IsEnabled => InputActions.asset.enabled;

    /// <inheritdoc />
    protected override void OnAwake() {
        SetupInputActions();
    }

    /// <inheritdoc />
    private void OnDisable() {
        Disable();
    }

    /// <inheritdoc />
    private void OnEnable() {
        Enable();
    }

    /// <summary>
    /// Initialize input actions.
    /// </summary>
    private void SetupInputActions() {
        InputActions ??= new PlayerInputActions();

        Jump = new BufferedInputAction(InputActions.Player.Jump, inputBufferTime);

        Move = InputActions.Player.Move;

        Cancel = InputActions.UI.Cancel;
    }

    /// <inheritdoc />
    public void LoadData(SaveData saveData) {
        if (string.IsNullOrEmpty(saveData.BindingOverrides)) return;

        InputActions.asset.LoadBindingOverridesFromJson(saveData.BindingOverrides);
    }

    /// <inheritdoc />
    public void SaveData(SaveData saveData) {
        saveData.BindingOverrides = InputActions.asset.SaveBindingOverridesAsJson();
    }

    /// <summary>
    /// Disable all input.
    /// </summary>
    public void Disable() {
        InputActions?.Disable();
    }

    /// <summary>
    /// Enable all input.
    /// </summary>
    public void Enable() {
        InputActions?.Enable();
    }
}
