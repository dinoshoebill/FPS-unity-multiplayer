using UnityEngine;
using Mirror;

public class scr_PlayerShoot : NetworkBehaviour {

    [SerializeField]
    private scr_WeaponData weapon;

    private const string PLAYER_TAG = "Player";

    public LayerMask mask;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Transform barrelEnd;

    [SerializeField]
    private scr_WeaponSway weaponSway;

    private void Start() {
        if (cam == null) {
            this.enabled = false;
        }
    }

    [Client]
    public void Shoot() {
        RaycastHit ray;

        Debug.Log("Shooting");

        barrelEnd.localRotation = weaponSway.transform.localRotation;

        if (Physics.Raycast(barrelEnd.position, barrelEnd.forward, out ray, weapon.range, mask)) {
            if (ray.collider.tag == PLAYER_TAG) {
                CmdPlayerShot(ray.collider.name);
            }
        }
    }

    [Command]
    private void CmdPlayerShot(string id) {
        Debug.Log("Player " + id + " has been hit");
    }
}
