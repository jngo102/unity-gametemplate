using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages player inputs.
/// </summary>
[RequireComponent(typeof(Player))]
public class PlayerInputManager : MonoBehaviour {
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
    private void Awake() {
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
