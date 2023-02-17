using UnityEngine;

public class scr_WeaponSway : MonoBehaviour {

    [SerializeField]
    private scr_PlayerMotor motor;

    private Vector3 newWeaponRotation;
    private Vector3 newWeaponRotationVelocity;

    private Vector3 targetWeaponRotation;
    private Vector3 targetWeaponRotationVelocity;

    private void Update() {

        targetWeaponRotation.y += scr_PlayerGlobals.swayAmount * motor.inputView.x * Time.deltaTime;
        targetWeaponRotation.x += scr_PlayerGlobals.swayAmount * -motor.inputView.y * Time.deltaTime;

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, scr_PlayerGlobals.swaySmoothing);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, scr_PlayerGlobals.swaySmoothing);

        newWeaponRotation.x = Mathf.Clamp(newWeaponRotation.x, -scr_PlayerGlobals.swayClampX, scr_PlayerGlobals.swayClampX);
        newWeaponRotation.y = Mathf.Clamp(newWeaponRotation.y, -scr_PlayerGlobals.swayClampY, scr_PlayerGlobals.swayClampY);
        transform.localRotation = Quaternion.Euler(newWeaponRotation);
    }
}
