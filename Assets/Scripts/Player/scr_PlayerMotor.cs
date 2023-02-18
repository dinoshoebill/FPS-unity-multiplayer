using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class scr_PlayerMotor : MonoBehaviour {

    #region - Variables -
    public CharacterController player;

    [HideInInspector]
    public Vector2 inputMovement;
    [HideInInspector]
    public Vector2 inputView;

    private Vector3 cameraRotation;
    private Vector3 playerRotation;

    private Vector2 speedVector;
    private Vector2 speedVelocity;

    private bool isSprinting;
    private bool wantsSprinting;
    private float jumpingForce;
    private float speed;
    #endregion

    #region - Headers -
    [Header("Player Camera")]
    [SerializeField] private Transform cameraHolder;

    #endregion

    #region - Awake / Update -
    private void Awake() {
        // player = GetComponent<CharacterController>();
        InitializePlayerSettings();
    }

    private void Update() {
        CalculateView();
        CalculateMovement();
        ApplyGravity();
    }
    #endregion

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
            speed * (isSprinting ? scr_PlayerGlobals.speedStrafeSprintMultiplier : scr_PlayerGlobals.speedStrafeMultiplier) * inputMovement.x * Time.deltaTime),
        ref speedVelocity,
        scr_PlayerGlobals.movementSmoothing, player.isGrounded ? scr_PlayerGlobals.movementSmoothing : scr_PlayerGlobals.airTimeSmoothing);

        Vector3 newPlayerMovement = new Vector3(speedVector.y, jumpingForce * Time.deltaTime, speedVector.x);
        newPlayerMovement = transform.TransformDirection(newPlayerMovement);
        player.Move(newPlayerMovement);
    }

    private void CalculateView() {
        playerRotation.y += scr_PlayerGlobals.viewXSensitivity * inputView.x * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(playerRotation);

        cameraRotation.x += scr_PlayerGlobals.viewYSensitivity * -inputView.y * Time.deltaTime;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, scr_PlayerGlobals.viewClampYMin, scr_PlayerGlobals.viewClampYMax);

        cameraHolder.localRotation = Quaternion.Euler(cameraRotation);
    }
    #endregion

    #region - Jump / Gravity -
    public void JumpInput(UnityEngine.InputSystem.InputAction.CallbackContext e) {
        if (e.interaction is UnityEngine.InputSystem.Interactions.TapInteraction)
            Jump(scr_PlayerGlobals.jumpPower);
        else
            Jump(scr_PlayerGlobals.jumpPower * scr_PlayerGlobals.doubleJumpMultiplier);
    }

    private void ApplyGravity() {
        if (player.isGrounded) {
            jumpingForce = -1f;
        }
        else {
            jumpingForce += scr_PlayerGlobals.gravity * scr_PlayerGlobals.gravityMultiplier * Time.deltaTime;
        }
    }

    private void Jump(float jumpStrength) {

        if (!player.isGrounded) {
            return;
        }

        jumpingForce += jumpStrength;
    }
    #endregion

    #region - Initializers -
    private void InitializePlayerSettings() {

        playerRotation = transform.localRotation.eulerAngles;
        cameraRotation = cameraHolder.localRotation.eulerAngles;

        isSprinting = false;
        wantsSprinting = false;
        speed = scr_PlayerGlobals.speedStand;
    }
    #endregion

    #region - Sprinting -
    public void StopSprinting() {
        isSprinting = false;
        speed = scr_PlayerGlobals.speedStand;
    }

    public void StopSprintingByRelease() {
        isSprinting = false;
        wantsSprinting = false;
        speed = scr_PlayerGlobals.speedStand;
    }

    public void StartSprinting() {
        if (isSprinting) {
            return;
        } else {
            wantsSprinting = true;
            isSprinting = true;
            speed = scr_PlayerGlobals.speedSprint;
        }
    }
    #endregion

    private void OnEnable() {
        jumpingForce = -1f;
        player.Move(Vector3.zero);
    }
}
