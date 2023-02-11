using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static scr_Settings;

public class scr_PlayerMotor : MonoBehaviour {

    private CharacterController controller;

    public Vector2 inputMovement;
    public Vector2 inputView;

    private Vector3 cameraRotation;
    private Vector3 playerRotation;

    private Vector2 speedVector;
    private Vector2 speedVelocity;

    [HideInInspector]
    public bool isSprinting;
    [HideInInspector]
    public bool wantsSprinting;
    private float jumpingForce;
    private float speed;

    private float stanceVelocityFloat;
    private Vector3 stanceVelocityVector;

    [Header("Preferences")]
    public Transform feetTransform;
    public Transform cameraHolder;

    [Header("Settings")]
    public PlayerSettingsModel settings;

    [Header("Player Mask")]
    public LayerMask playerMask;

    [Header("Stance")]
    public PlayerStance playerStance;
    private Vector3 playerCameraVelocity;
    public Transform positionStand;
    public Transform positionCrouch;
    public Transform positionProne;
    public PlayerStanceCollider stanceStand;
    public PlayerStanceCollider stanceCrouch;
    public PlayerStanceCollider stanceProne;

    private void Awake() {
        controller = GetComponent<CharacterController>();
        InitializePlayerSettings();
    }

    private void Update() {
        CalculateView();
        CalculateMovement();
        SetCameraPosition();
        ApplyGravity();
    }

    #region - View / Movement -
    public void ViewInput(Vector2 input) {
        inputView = input;
    }

    public void MovementInput(Vector2 input) {
        inputMovement = input;

        if (wantsSprinting) {
            StartSprinting();
        }
    }

    private void CalculateMovement() {
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
        cameraRotation.x = Mathf.Clamp(
            cameraRotation.x,
            playerStance == PlayerStance.Prone ? settings.viewProneClampYMin : settings.viewClampYMin,
            playerStance == PlayerStance.Prone ? settings.viewProneClampYMax : settings.viewClampYMax);

        cameraHolder.localRotation = Quaternion.Euler(cameraRotation);
    }

    private void SetCameraPosition() {
        if (playerStance == PlayerStance.Crouch) {
            cameraHolder.transform.localPosition =
                Vector3.SmoothDamp(cameraHolder.transform.localPosition, positionCrouch.transform.localPosition, ref playerCameraVelocity, settings.stanceSmoothing * Time.deltaTime);
            controller.height =
                Mathf.SmoothDamp(controller.height, stanceCrouch.stanceCollider.height, ref stanceVelocityFloat, settings.stanceSmoothing * Time.deltaTime);
            controller.center =
                Vector3.SmoothDamp(controller.center, stanceCrouch.stanceCollider.center, ref stanceVelocityVector, settings.stanceSmoothing * Time.deltaTime);
        }
        else if (playerStance == PlayerStance.Prone) {
            cameraHolder.transform.localPosition =
                Vector3.SmoothDamp(cameraHolder.transform.localPosition, positionProne.transform.localPosition, ref playerCameraVelocity, settings.stanceSmoothing * Time.deltaTime);
            controller.height =
                Mathf.SmoothDamp(controller.height, stanceProne.stanceCollider.height, ref stanceVelocityFloat, settings.stanceSmoothing * Time.deltaTime);
            controller.center =
                Vector3.SmoothDamp(controller.center, stanceProne.stanceCollider.center, ref stanceVelocityVector, settings.stanceSmoothing * Time.deltaTime);
        }
        else {
            cameraHolder.transform.localPosition =
                Vector3.SmoothDamp(cameraHolder.transform.localPosition, positionStand.transform.localPosition, ref playerCameraVelocity, settings.stanceSmoothing * Time.deltaTime);
            controller.height =
                Mathf.SmoothDamp(controller.height, stanceStand.stanceCollider.height, ref stanceVelocityFloat, settings.stanceSmoothing * Time.deltaTime);
            controller.center =
                Vector3.SmoothDamp(controller.center, stanceStand.stanceCollider.center, ref stanceVelocityVector, settings.stanceSmoothing * Time.deltaTime);
        }
    }
    #endregion

    #region - Jump / Gravity -
    public void JumpInput(UnityEngine.InputSystem.InputAction.CallbackContext e) {
        if (playerStance == PlayerStance.Stand)
            if (e.interaction is UnityEngine.InputSystem.Interactions.TapInteraction)
                Jump(settings.jumpPower);
            else
                Jump(settings.jumpPower * settings.doubleJumpMultiplier);
        else
            SetPlayerStance(PlayerStance.Stand);
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

    #region - Player Stance -
    public void SetPlayerStance(PlayerStance nextPlayerStance) {
        if (nextPlayerStance == PlayerStance.Stand) {
            if (CanChangeStance(stanceStand.stanceCollider.height)) {
                return;
            }
        }
        else if (nextPlayerStance == PlayerStance.Crouch) {
            if (nextPlayerStance.CompareTo(playerStance) == 0) {
                if (CanChangeStance(stanceStand.stanceCollider.height)) {
                    return;
                }
                else {
                    nextPlayerStance = PlayerStance.Stand;
                }
            }
            else if (CanChangeStance(stanceCrouch.stanceCollider.height) || !controller.isGrounded) {
                return;
            }
        }

        playerStance = nextPlayerStance;
        SetPlayerSpeed(playerStance);
    }

    private void SetPlayerSpeed(PlayerStance newPlayerStance) {
        if (newPlayerStance == PlayerStance.Stand)
            if (isSprinting)
                speed = settings.speedSprint;
            else
                speed = settings.speedStand;
        else if (newPlayerStance == PlayerStance.Crouch)
            speed = settings.speedCrouch;
        else if (newPlayerStance == PlayerStance.Prone)
            speed = settings.speedProne;

    }

    private bool CanChangeStance(float stanceCheckHeight) {
        Vector3 start = new Vector3(feetTransform.position.x, feetTransform.position.y + controller.radius + 0.01f, feetTransform.position.z);
        Vector3 end = new Vector3(feetTransform.position.x, feetTransform.position.y - controller.radius - 0.01f + stanceCheckHeight, feetTransform.position.z);
        return Physics.CheckCapsule(start, end, controller.radius, playerMask);
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
        settings.speedCrouch = 4;
        settings.speedProne = 2;

        settings.viewClampYMin = -70;
        settings.viewClampYMax = 70;
        settings.viewProneClampYMin = -50;
        settings.viewProneClampYMax = 50;

        settings.jumpPower = 15;

        settings.gravity = -10;
        settings.gravityMultiplier = 5;

        settings.stanceSmoothing = 12f;
        settings.movementSmoothing = 0.3f;
        settings.airTimeSmoothing = 0.05f;

        settings.speedStrafeMultiplier = 0.7f;
        settings.speedStrafeSprintMultiplier = 0.5f;

        settings.doubleJumpMultiplier = 1.3f;

        isSprinting = false;
        wantsSprinting = false;

        SetPlayerStance(PlayerStance.Stand);
    }
    #endregion

    #region - Sprinting -
    public void StopSprinting() {
        isSprinting = false;
        wantsSprinting = false;
        SetPlayerSpeed(playerStance);
    }

    public void WantsSprinting() {
        wantsSprinting = true;
        StartSprinting();
    }

    public void StartSprinting() {
        if (isSprinting) {
            return;
        } else if ((!CanChangeStance(stanceStand.stanceCollider.height) || playerStance == PlayerStance.Stand) && inputMovement.y > 0) {
            wantsSprinting = false;
            isSprinting = true;
            SetPlayerStance(PlayerStance.Stand);
        }
    }
    #endregion
}
