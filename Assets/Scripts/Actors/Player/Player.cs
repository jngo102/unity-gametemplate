using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Grounder))]
[RequireComponent(typeof(Jumper))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Runner))]
public class Player : MonoBehaviour, ISpawnable {
    #region Exposed Values
    public float CoyoteTime = 0.1f;
    #endregion

    #region Events

    #endregion

    #region Components
    private Rigidbody2D body;
    private Grounder grounder;
    private Jumper jumper;
    private Runner runner;
    #endregion

    private GameManager gameManager;
    private InputManager inputManager;
    private UIManager uiManager;

    #region Tracked Values
    private float coyoteTimer;
    private Vector2 inputVector;
    #endregion

    #region Unity Functions
    private void Awake() {
        GetComponents();
        AssignPlayer();
        AssignSingletons();
        InitializeTrackedValues();
        SubscribeEvents();
    }

    private void Update() {
        CheckGrounded();
        UpdateTrackedValues();
    }

    private void FixedUpdate() {
        Move();
    }

    private void OnEnable() {
        EnableAllInputs();
    }

    private void OnDisable() {
        DisableAllInputs();
    }
    #endregion

    private void OnJump(InputAction.CallbackContext ctx) {
        if (ctx.ReadValueAsButton()) {
            Jump();
        }
    }

    private void OnMoveStart(InputAction.CallbackContext ctx) {
        inputVector = ctx.ReadValue<Vector2>();
    }

    private void OnMoveStop(InputAction.CallbackContext ctx) {
        inputVector = Vector2.zero;
    }

    public void OnCreate() {

    }

    public void OnSpawn() {

    }

    public void OnDespawn() {

    }

    public void OnDestroy() {

    }

    private void AssignPlayer() {
        CameraController.Target = transform;
    }

    private void AssignSingletons() {
        gameManager = GameManager.Instance;
        inputManager = InputManager.Instance;
        uiManager = UIManager.Instance;
    }

    private void CheckGrounded() {
        if (body.velocity.y <= 0 && grounder.WasGrounded && !grounder.IsGrounded()) {
            coyoteTimer = 0;
            jumper.StopGravity = true;
        }

        if (!inputManager.Jump.InputAction.IsPressed())
            jumper.CancelJump();
    }

    private void DisableAllInputs() {
        DisableBaseInputs();
    }

    private void DisableBaseInputs() {
        inputManager.Jump.InputAction.performed -= OnJump;
        inputManager.Move.performed -= OnMoveStart;
        inputManager.Move.canceled -= OnMoveStop;
    }

    private void EnableAllInputs() {
        EnableBaseInputs();
    }

    private void EnableBaseInputs() {
        inputManager.Jump.InputAction.performed += OnJump;
        inputManager.Move.performed += OnMoveStart;
        inputManager.Move.canceled += OnMoveStop;
    }

    private void GetComponents() {
        body ??= GetComponent<Rigidbody2D>();
        grounder ??= GetComponent<Grounder>();
        jumper ??= GetComponent<Jumper>();
        runner ??= GetComponent<Runner>();
    }

    private void InitializeTrackedValues() {
        coyoteTimer = CoyoteTime + 1;
    }

    public void Jump() {
        if (grounder.IsGrounded() || coyoteTimer <= CoyoteTime) {
            jumper.Jump();
        }
    }

    private void OnLand(Jumper _) {
        if (inputManager.Jump.IsBuffered()) {
            Jump();
        }
    }

    private void Move() {
        runner.Run(inputVector.x);
        jumper.StopGravity = coyoteTimer <= CoyoteTime;
    }

    private void SubscribeEvents() {
        jumper.Landed += OnLand;
    }

    private void UpdateTrackedValues() {
        coyoteTimer = Mathf.Clamp(coyoteTimer + Time.deltaTime, 0, CoyoteTime + 1);
    }
}
