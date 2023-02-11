using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static scr_Settings;

public class scr_PlayerMotor : MonoBehaviour {

    #region - Variables -
    private CharacterController controller;

    [HideInInspector]
    public Vector2 inputMovement;
    [HideInInspector]
    public Vector2 inputView;

    private Vector3 cameraRotation;
    private Vector3 playerRotation;

    private Vector2 speedVector;
    private Vector2 speedVelocity;

    [HideInInspector]
    public bool isSprinting;
    private bool wantsSprinting;
    private float jumpingForce;
    private float speed;
    #endregion

    #region - Headers -
    [Header("Preferences")]
    public Transform feetTransform;
    public Transform cameraHolder;

    [Header("Settings")]
    public PlayerSettingsModel settings;

    [Header("Player Mask")]
    public LayerMask playerMask;
    #endregion

    #region - Awake / Update -
    private void Awake() {
        controller = GetComponent<CharacterController>();
        InitializePlayerSettings();
    }
    #endregion

    private void Update() {
        CalculateView();
        CalculateMovement();
        ApplyGravity();
    }

    #region - View / Movement -
    public void ViewInput(Vector2 input) {
        inputView = input;
    }

    public void MovementInput(Vector2 input) {
        inputMovement = input;

        if(wantsSprinting && inputMovement.y > 0) {
            StartSprinting();
        }
    }

    private void CalculateMovement() {

        if (inputMovement.y <= 0 && wantsSprinting) {
            StopSprinting();
        }

        speedVector = Vector2.SmoothDamp(speedVector,
        new Vector2(
            speed * inputMovement.y * Time.deltaTime,
            speed * (isSprinting ? settings.speedStrafeSprintMultiplier : settings.speedStrafeMultiplier) * inputMovement.x * Time.deltaTime),
        ref speedVelocity,
        settings.movementSmoothing, controller.isGrounded ? settings.movementSmoothing : settings.airTimeSmoothing);

        Vector3 newPlayerMovement = new Vector3(speedVector.y, jumpingForce * Time.deltaTime, speedVector.x);
        newPlayerMovement = transform.TransformDirection(newPlayerMovement);
        controller.Move(newPlayerMovement);
    }

    private void CalculateView() {
        playerRotation.y += settings.viewXSensitivity * inputView.x * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(playerRotation);

        cameraRotation.x += settings.viewYSensitivity * -inputView.y * Time.deltaTime;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, settings.viewClampYMin, settings.viewClampYMax);

        cameraHolder.localRotation = Quaternion.Euler(cameraRotation);
    }
    #endregion

    #region - Jump / Gravity -
    public void JumpInput(UnityEngine.InputSystem.InputAction.CallbackContext e) {
        if (e.interaction is UnityEngine.InputSystem.Interactions.TapInteraction)
            Jump(settings.jumpPower);
        else
            Jump(settings.jumpPower * settings.doubleJumpMultiplier);
    }

    private void ApplyGravity() {
        if (controller.isGrounded) {
            jumpingForce = -1f;
        }
        else {
            jumpingForce += settings.gravity * settings.gravityMultiplier * Time.deltaTime;
        }
    }

    private void Jump(float jumpStrength) {

        if (!controller.isGrounded) {
            return;
        }

        jumpingForce += jumpStrength;
    }
    #endregion

    #region - Initializers -
    private void InitializePlayerSettings() {

        playerRotation = transform.localRotation.eulerAngles;
        cameraRotation = cameraHolder.localRotation.eulerAngles;

        settings.viewXSensitivity = 12;
        settings.viewYSensitivity = 12;

        settings.speedSprint = 10;
        settings.speedStand = 7;

        settings.viewClampYMin = -70;
        settings.viewClampYMax = 70;

        settings.jumpPower = 15;

        settings.gravity = -10;
        settings.gravityMultiplier = 5;

        settings.movementSmoothing = 0.3f;
        settings.airTimeSmoothing = 0.05f;

        settings.speedStrafeMultiplier = 0.7f;
        settings.speedStrafeSprintMultiplier = 0.5f;

        settings.doubleJumpMultiplier = 1.3f;

        isSprinting = false;
        wantsSprinting = false;
        speed = settings.speedStand;
    }
    #endregion

    #region - Sprinting -
    public void StopSprinting() {
        isSprinting = false;
        speed = settings.speedStand;
    }

    public void StopSprintingByRelease() {
        isSprinting = false;
        wantsSprinting = false;
        speed = settings.speedStand;
    }

    public void StartSprinting() {
        if (isSprinting) {
            return;
        } else {
            wantsSprinting = true;
            isSprinting = true;
            speed = settings.speedSprint;
        }
    }
    #endregion
}
