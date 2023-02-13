using UnityEngine;

public class scr_PlayerController : MonoBehaviour {

    private PlayerInput input;
    private scr_PlayerMotor motor;
    private scr_PlayerShoot shoot;

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

        input.Weapon.Fire.performed += e => shoot.Shoot();
    }
}
