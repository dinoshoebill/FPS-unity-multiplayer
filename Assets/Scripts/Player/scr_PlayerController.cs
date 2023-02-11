using Mirror;
using UnityEngine;
using static scr_Settings;

public class scr_PlayerController : NetworkBehaviour {

    private PlayerInput input;
    private scr_PlayerMotor motor;

    private void Awake() {

        input = new PlayerInput();
        motor = GetComponent<scr_PlayerMotor>();

        InitializeInputActions();
        input.Enable();
    }

    private void InitializeInputActions() {
        input.Player.Movement.performed += e => motor.MovementInput(e.ReadValue<Vector2>());

        input.Player.View.performed += e => motor.ViewInput(e.ReadValue<Vector2>());

        input.Player.Jump.performed += e => motor.JumpInput(e);

        input.Player.Sprinting.started += e => motor.StartSprinting();

        input.Player.Sprinting.performed += e => motor.StopSprintingByRelease();

        input.Weapon.Fire.performed += e => Debug.Log(e);
    }
}
