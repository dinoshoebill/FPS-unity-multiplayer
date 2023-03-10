using UnityEngine;
using Mirror;

[RequireComponent(typeof(scr_WeaponManager))]
public class scr_PlayerShoot : NetworkBehaviour {

    [SerializeField]
    private const string PLAYER_TAG = "Player";

    [SerializeField] private LayerMask mask;

    [SerializeField]
    private Transform barrel;

    [SerializeField]
    private scr_WeaponSway weaponSway;

    [SerializeField]
    private scr_WeaponManager weaponManager;

    private void Start() {
    }

    [Client]
    public void Shoot() {

        if (!weaponManager.currentWeapon)
            return;

        RaycastHit ray;

        Debug.Log("Shooting");

        barrel.localRotation = weaponSway.transform.localRotation;

        if (Physics.Raycast(barrel.position, barrel.forward, out ray, weaponManager.currentWeapon.weaponData.range, mask)) {
            if (ray.collider.tag == PLAYER_TAG) {
                CmdPlayerShot(ray.collider.name, weaponManager.currentWeapon.weaponData.damage);
            }
        }
    }

    [Command]
    private void CmdPlayerShot(string id, int damage) {
        Debug.Log("Player " + id + " has been hit");

        scr_Player player = scr_GameManager.GetPlayer(id);
        player.RpcTakeDamage(damage);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.K)) {
            scr_Player player = GetComponent<scr_Player>();
            player.RpcTakeDamage(999);
        }
    }
}
