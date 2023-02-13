using System.Collections;
using System.Collections.Generic;
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

    private Vector3 swayPosition;
    private float swayTime;
    private float swaySmoothing;
    private float swayResetSmoothing;
    private float swayAmountA;
    private float swayAmountB;
    private float swayScale;
    private float swayLerpSpeed;
    private float swayClampX;
    private float swayClampY;
    private float swayClampZ;

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
        transform.localRotation = Quaternion.Euler(newWeaponRotation + newWeaponMovementRotation);
    }

    private void InitializeWeaponSettings() {

        swayAmount = 1;
        movementSwayAmount = 5;

        swayClampX = 3;
        swayClampY = 3;
        swayClampZ = 3;

        swaySmoothing = 0.1f;
        swayResetSmoothing = 0.1f;

        swayScale = 400;
        swayAmountA = 1;
        swayAmountB = 2;
        swayLerpSpeed = 1;

        swayTime = 0;
    }

    private void CalculateWeaponSway() {
        Vector3 targetPosition = LissajousCurve(swayTime, swayAmountA, swayAmountB) / swayScale;
        swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);

        swayTime += Time.deltaTime;

        if (swayTime > 6.5f) {
            swayTime = 0;
        }


        this.transform.localPosition = swayPosition;
    }

    private Vector3 LissajousCurve(float Time, float A, float B) {
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time * Mathf.PI));
    }
}
