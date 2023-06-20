using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>, IDataPersistence {
    [SerializeField] private float inputBufferTime = 0.15f;

    public PlayerInputActions InputActions { get; private set; }

    public BufferedInputAction Jump;

    [NonSerialized] public InputAction Move;
    [NonSerialized] public InputAction Cancel;

    protected override void OnAwake() {
        SetupInputActions();
    }

    private void OnEnable() {
        InputActions?.Enable();
    }

    private void OnDisable() {
        InputActions?.Disable();
    }

    private void SetupInputActions() {
        InputActions ??= new PlayerInputActions();

        Jump = new BufferedInputAction(InputActions.Player.Jump, inputBufferTime);

        Move = InputActions.Player.Move;

        Cancel = InputActions.UI.Cancel;
    }

    public void LoadData(SaveData saveData) {
        InputActions.Player.Get().asset.LoadFromJson(saveData.InputAsset);
        Debug.Log("Load data");
    }

    public void SaveData(SaveData saveData) {
        saveData.InputAsset = InputActions.Player.Get().asset.ToJson();
    }
}
