using UnityEngine;

[RequireComponent(typeof(scr_PlayerMotor))]
[RequireComponent(typeof(scr_PlayerShoot))]
public class scr_PlayerController : MonoBehaviour {

    private scr_PlayerMotor motor;
    private scr_PlayerShoot shoot;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.K)) {
            GetComponent<scr_Player>().RpcTakeDamage(999);
        }
    }

    private void Awake() {
        motor = GetComponent<scr_PlayerMotor>();
        shoot = GetComponent<scr_PlayerShoot>();
    }
}
