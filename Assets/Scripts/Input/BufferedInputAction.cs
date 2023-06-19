using UnityEngine;
using UnityEngine.InputSystem;

public class BufferedInputAction {
    public InputAction InputAction;
    private float timeOfLastInput;
    private readonly float inputBufferTime;

    public BufferedInputAction(InputAction inputAction, float inputBufferTime) {
        InputAction = inputAction;
        this.inputBufferTime = inputBufferTime;

        InputAction.performed += OnPerformed;
    }

    private void OnPerformed(InputAction.CallbackContext ctx) {
        if (ctx.ReadValueAsButton()) {
            timeOfLastInput = Time.time;
        }
    }

    public bool IsBuffered() {
        return Time.time - timeOfLastInput < inputBufferTime;
    }
}
