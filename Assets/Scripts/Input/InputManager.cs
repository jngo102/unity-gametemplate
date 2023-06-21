using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Singleton that manages the game's inputs.
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

    /// <inheritdoc />
    protected override void OnAwake() {
        SetupInputActions();
    }

    /// <inheritdoc />
    private void OnEnable() {
        InputActions?.Enable();
    }

    /// <inheritdoc />
    private void OnDisable() {
        InputActions?.Disable();
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
}
