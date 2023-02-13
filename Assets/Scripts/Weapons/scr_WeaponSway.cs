using UnityEngine;

public class scr_WeaponSway : MonoBehaviour {

    [SerializeField]
    private scr_PlayerMotor motor;

    private float swayAmount;
    private float movementSwayAmount;

    private Vector3 newWeaponRotation;
    private Vector3 newWeaponRotationVelocity;

    private Vector3 targetWeaponRotation;
    private Vector3 targetWeaponRotationVelocity;

    private Vector3 newWeaponMovementRotation;
    private Vector3 newWeaponMovementRotationVelocity;

    private Vector3 targetWeaponMovementRotation;
    private Vector3 targetWeaponMovementRotationVelocity;

    private float swaySmoothing;
    private float swayResetSmoothing;

    public float swayAmountA;
    public float swayAmountB;
    public float swayScale;
    public float swayLerpSpeed;
    public float swayClampX;
    public float swayClampY;
    public float swayClampZ;

    private void Awake() {
        InitializeWeaponSettings();
    }

    private void Update() {

        targetWeaponRotation.y += swayAmount * motor.inputView.x * Time.deltaTime;
        targetWeaponRotation.x += swayAmount * -motor.inputView.y * Time.deltaTime;

        targetWeaponMovementRotation.z += movementSwayAmount * motor.inputMovement.x * Time.deltaTime;
        targetWeaponMovementRotation.x += movementSwayAmount * -motor.inputMovement.y * Time.deltaTime;

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, swayResetSmoothing);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, swaySmoothing);

        targetWeaponMovementRotation = Vector3.SmoothDamp(targetWeaponMovementRotation, Vector3.zero, ref targetWeaponMovementRotationVelocity, swayResetSmoothing);
        newWeaponMovementRotation = Vector3.SmoothDamp(newWeaponMovementRotation, targetWeaponMovementRotation, ref newWeaponMovementRotationVelocity, swaySmoothing);

        Vector3 combinedWeaponRotation = newWeaponRotation + newWeaponMovementRotation;
        combinedWeaponRotation.x = Mathf.Clamp(combinedWeaponRotation.x, -swayClampX, swayClampX);
        combinedWeaponRotation.y = Mathf.Clamp(combinedWeaponRotation.y, -swayClampY, swayClampY);
        combinedWeaponRotation.z = Mathf.Clamp(combinedWeaponRotation.z, -swayClampZ, swayClampZ);
        transform.localRotation = Quaternion.Euler(combinedWeaponRotation);
    }

    private void InitializeWeaponSettings() {

        swayAmount = 1;
        movementSwayAmount = 5;

        swayClampX = 3;
        swayClampY = 3;
        swayClampZ = 3;

        swaySmoothing = 0.1f;
        swayResetSmoothing = 0.1f;
    }
}
