using UnityEngine;
using Mirror;

public class scr_PlayerShoot : NetworkBehaviour {

    [SerializeField]
    private scr_WeaponData weapon;

    private const string PLAYER_TAG = "Player";

    public LayerMask mask;

    [SerializeField]
    private Transform barrel;

    [SerializeField]
    private scr_WeaponSway weaponSway;

    private void Start() {
        if (barrel == null) {
            this.enabled = false;
        }
    }

    [Client]
    public void Shoot() {
        RaycastHit ray;

        Debug.Log("Shooting");

        barrel.localRotation = weaponSway.transform.localRotation;

        if (Physics.Raycast(barrel.position, barrel.forward, out ray, weapon.range, mask)) {
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
