using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controller for a player.
/// </summary>
[RequireComponent(typeof(Grounder))]
[RequireComponent(typeof(Jumper))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Runner))]
public class Player : MonoBehaviour, ISpawnable {
    #region Exposed Values
    /// <summary>
    /// The duration before the player actually falls and cannot perform a jump.
    /// </summary>
    public float CoyoteTime = 0.1f;
    #endregion

    #region Components
    private Rigidbody2D body;
    private Grounder grounder;
    private Jumper jumper;
    private Runner runner;
    #endregion

    #region Singletons
    private GameManager gameManager;
    private InputManager inputManager;
    private UIManager uiManager;
    #endregion

    #region Tracked Values
    private float coyoteTimer;
    private Vector2 inputVector;
    #endregion

    #region Unity Functions
    /// <inheritdoc />
    private void Awake() {
        GetComponents();
        AssignPlayer();
        AssignSingletons();
        InitializeTrackedValues();
        SubscribeEvents();
    }

    /// <inheritdoc />
    private void Update() {
        CheckGrounded();
        UpdateTrackedValues();
    }

    /// <inheritdoc />
    private void FixedUpdate() {
        Move();
    }

    /// <inheritdoc />
    private void OnEnable() {
        EnableAllInputs();
    }

    /// <inheritdoc />
    private void OnDisable() {
        DisableAllInputs();
    }
    #endregion

    /// <summary>
    /// Callback for when the player jumps.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void OnJump(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton()) {
            Jump();
        }
    }

    /// <summary>
    /// Callback for when the player starts moving.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void OnMoveStart(InputAction.CallbackContext context) {
        inputVector = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Callback for when the player stops moving.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void OnMoveStop(InputAction.CallbackContext context) {
        inputVector = Vector2.zero;
    }

    /// <inheritdoc />
    public void OnCreate() {

    }

    /// <inheritdoc />
    public void OnSpawn() {

    }

    /// <inheritdoc />
    public void OnDespawn() {

    }

    /// <inheritdoc />
    public void OnDestroy() {

    }

    /// <summary>
    /// Assign the player as the camera controller's current target.
    /// </summary>
    private void AssignPlayer() {
        CameraController.Target = transform;
    }

    /// <summary>
    /// Assign singletons to the script's local variables.
    /// </summary>
    private void AssignSingletons() {
        gameManager = GameManager.Instance;
        inputManager = InputManager.Instance;
        uiManager = UIManager.Instance;
    }

    /// <summary>
    /// Check whether the player is grounded, accounting for coyote time.
    /// </summary>
    private void CheckGrounded() {
        if (body.velocity.y <= 0 && grounder.WasGrounded && !grounder.IsGrounded()) {
            coyoteTimer = 0;
            jumper.StopGravity = true;
        }

        if (!inputManager.Jump.InputAction.IsPressed())
            jumper.CancelJump();
    }

    /// <summary>
    /// Disable all player inputs.
    /// </summary>
    private void DisableAllInputs() {
        DisableBaseInputs();
    }

    /// <summary>
    /// Disable only the player's base inputs.
    /// </summary>
    private void DisableBaseInputs() {
        inputManager.Jump.InputAction.performed -= OnJump;
        inputManager.Move.performed -= OnMoveStart;
        inputManager.Move.canceled -= OnMoveStop;
    }

    /// <summary>
    /// Enable all player inputs.
    /// </summary>
    private void EnableAllInputs() {
        EnableBaseInputs();
    }

    /// <summary>
    /// Enable only the player's base inputs.
    /// </summary>
    private void EnableBaseInputs() {
        inputManager.Jump.InputAction.performed += OnJump;
        inputManager.Move.performed += OnMoveStart;
        inputManager.Move.canceled += OnMoveStop;
    }

    /// <summary>
    /// Get all components on the player.
    /// </summary>
    private void GetComponents() {
        body = GetComponent<Rigidbody2D>();
        grounder = GetComponent<Grounder>();
        jumper = GetComponent<Jumper>();
        runner = GetComponent<Runner>();
    }

    /// <summary>
    /// Initialize the values that are tracked throughout the script's execution.
    /// </summary>
    private void InitializeTrackedValues() {
        coyoteTimer = CoyoteTime + 1;
    }

    /// <summary>
    /// Perform a jump.
    /// </summary>
    public void Jump() {
        if (grounder.IsGrounded() || coyoteTimer <= CoyoteTime) {
            jumper.Jump();
        }
    }

    /// <summary>
    /// Callback for when the player lands.
    /// </summary>
    private void OnLand() {
        if (inputManager.Jump.IsBuffered()) {
            Jump();
        }
    }

    /// <summary>
    /// Move the player.
    /// </summary>
    private void Move() {
        runner.Run(inputVector.x);
        jumper.StopGravity = coyoteTimer <= CoyoteTime;
    }

    /// <summary>
    /// Subscribe to events managed within the script.
    /// </summary>
    private void SubscribeEvents() {
        jumper.Landed += OnLand;
    }

    /// <summary>
    /// Update values that are to be tracked.
    /// </summary>
    private void UpdateTrackedValues() {
        coyoteTimer = Mathf.Clamp(coyoteTimer + Time.deltaTime, 0, CoyoteTime + 1);
    }
}
