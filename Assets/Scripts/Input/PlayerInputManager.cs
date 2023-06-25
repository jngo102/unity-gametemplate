using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Singleton that manages global inputs.
/// </summary>
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
    /// Whether the input manager is enabled.
    /// </summary>
    public bool IsEnabled => InputActions.asset.enabled;

    private void Awake() {
        SetupInputActions();
    }

    private void OnDisable() {
        Disable();
    }

    private void OnEnable() {
        Enable();
    }

    /// <summary>
    /// Initialize input actions.
    /// </summary>
    private void SetupInputActions() {
        InputActions ??= new PlayerInputActions();
        var overridesJson = UIManager.Instance.ReferencePlayerActions.asset.SaveBindingOverridesAsJson();
        InputActions.asset.LoadBindingOverridesFromJson(overridesJson);

        Jump = new BufferedInputAction(InputActions.Player.Jump, inputBufferTime);

        Move = InputActions.Player.Move;
    }

    /// <summary>
    /// Disable all input.
    /// </summary>
    public void Disable() {
        InputActions.Disable();
    }

    /// <summary>
    /// Enable all input.
    /// </summary>
    public void Enable() {
        InputActions.Enable();
    }
}
