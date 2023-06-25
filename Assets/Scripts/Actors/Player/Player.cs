using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Controller for a player.
/// </summary>
[RequireComponent(typeof(Grounder))]
[RequireComponent(typeof(Jumper))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Runner))]
[RequireComponent(typeof(PlayerInputManager))]
public class Player : MonoBehaviour, ISpawnable {
    #region Exposed Values

    /// <summary>
    ///     The duration before the player actually falls and cannot perform a jump.
    /// </summary>
    [SerializeField] private float coyoteTime = 0.1f;

    #endregion

    public PlayerInputManager InputManager { get; private set; }

    /// <inheritdoc />
    public void OnCreate() { }

    /// <inheritdoc />
    public void OnSpawn() { }

    /// <inheritdoc />
    public void OnDespawn() { }

    /// <inheritdoc />
    public void OnDelete() { }

    /// <summary>
    ///     Callback for when the player starts a jump.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void OnJumpStart(InputAction.CallbackContext context) {
        Jump();
    }

    /// <summary>
    ///     Callback for when the player ends a jump.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void OnJumpStop(InputAction.CallbackContext context) {
        jumper.CancelJump();
    }

    /// <summary>
    ///     Callback for when the player starts moving.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void OnMoveStart(InputAction.CallbackContext context) {
        inputVector = context.ReadValue<Vector2>();
        runner.Run(inputVector.x);
    }

    /// <summary>
    ///     Callback for when the player stops moving.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void OnMoveStop(InputAction.CallbackContext context) {
        runner.StopRun();
    }

    /// <summary>
    ///     Assign the player as the camera controller's current target.
    /// </summary>
    private void AssignPlayer() {
        FindObjectOfType<CameraController>(true).Target = transform;
    }

    /// <summary>
    ///     Check whether the player is grounded, accounting for coyote time.
    /// </summary>
    private void CheckGrounded() {
        if (body.velocity.y <= 0 && grounder.WasGrounded && !grounder.IsGrounded()) {
            coyoteTimer = 0;
            jumper.StopGravity = true;
        }

        if (InputManager.IsEnabled && !InputManager.Jump.InputAction.IsPressed()) jumper.CancelJump();
    }

    /// <summary>
    ///     Enable all player inputs.
    /// </summary>
    public void EnableAllInputs() {
        EnableBaseInputs();
    }

    /// <summary>
    ///     Enable only the player's base inputs.
    /// </summary>
    private void EnableBaseInputs() {
        InputManager.Jump.InputAction.performed += OnJumpStart;
        InputManager.Jump.InputAction.canceled += OnJumpStop;
        InputManager.Move.performed += OnMoveStart;
        InputManager.Move.canceled += OnMoveStop;
    }

    /// <summary>
    ///     Disable all player inputs.
    /// </summary>
    public void DisableAllInputs() {
        DisableBaseInputs();
    }

    /// <summary>
    ///     Disable only the player's base inputs.
    /// </summary>
    private void DisableBaseInputs() {
        InputManager.Jump.InputAction.performed -= OnJumpStart;
        InputManager.Jump.InputAction.canceled -= OnJumpStop;
        InputManager.Move.performed -= OnMoveStart;
        InputManager.Move.canceled -= OnMoveStop;
    }

    /// <summary>
    ///     Get all components on the player.
    /// </summary>
    private void GetComponents() {
        body = GetComponent<Rigidbody2D>();
        grounder = GetComponent<Grounder>();
        jumper = GetComponent<Jumper>();
        runner = GetComponent<Runner>();
        InputManager = GetComponent<PlayerInputManager>();
    }

    /// <summary>
    ///     Initialize the values that are tracked throughout the script's execution.
    /// </summary>
    private void InitializeTrackedValues() {
        coyoteTimer = coyoteTime + 1;
    }

    /// <summary>
    ///     Perform a jump.
    /// </summary>
    public void Jump() {
        if (grounder.IsGrounded() || coyoteTimer <= coyoteTime) {
            coyoteTimer = coyoteTime + 1;
            jumper.Jump();
        }
    }

    /// <summary>
    ///     Callback for when the player lands.
    /// </summary>
    private void OnLand() {
        if (InputManager.IsEnabled && InputManager.Jump.IsBuffered()) Jump();
    }

    /// <summary>
    ///     Stop all movement of the player.
    /// </summary>
    public void StopMovement() {
        jumper.CancelJump();
        runner.StopRun();
    }

    /// <summary>
    ///     Handle the player's coyote time.
    /// </summary>
    private void HandleCoyoteTime() {
        jumper.StopGravity = coyoteTimer <= coyoteTime;
    }

    /// <summary>
    ///     Subscribe to events managed within the script.
    /// </summary>
    private void SubscribeEvents() {
        jumper.Landed += OnLand;
    }

    /// <summary>
    ///     Update values that are to be tracked.
    /// </summary>
    private void UpdateTrackedValues() {
        coyoteTimer = Mathf.Clamp(coyoteTimer + Time.deltaTime, 0, coyoteTime + 1);
    }

    #region Components

    private Rigidbody2D body;
    private Grounder grounder;
    private Jumper jumper;
    private Runner runner;

    #endregion

    #region Tracked Values

    private float coyoteTimer;
    private Vector2 inputVector;

    #endregion

    #region Unity Functions

    private void Awake() {
        GetComponents();
        AssignPlayer();
        InitializeTrackedValues();
        SubscribeEvents();
    }

    private void Update() {
        HandleCoyoteTime();
        CheckGrounded();
        UpdateTrackedValues();
    }

    private void OnEnable() {
        EnableAllInputs();
    }

    private void OnDisable() {
        DisableAllInputs();
    }

    #endregion
}