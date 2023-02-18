using UnityEngine;
using Mirror;

public class scr_PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private scr_Weapon weapon;
    [SerializeField]
    private GameObject weaponGFX;

    [Header("Layer Mask")]
    [SerializeField] private LayerMask mask;

    [SerializeField]
    private Transform barrel;

    [SerializeField]
    private scr_WeaponSway weaponSway;

    private void Start() {
        barrel = GameObject.Find("BarrelEnd").transform;
        weaponSway = GetComponentInChildren<scr_WeaponSway>();
        weaponGFX = weapon.gameObject;
        weaponGFX.layer = LayerMask.NameToLayer(scr_PlayerGlobals.weaponLayer);
    }

    [Client]
    public void Shoot() {
        RaycastHit ray;

        Debug.Log("Shooting");

        barrel.localRotation = weaponSway.transform.localRotation;

        if (Physics.Raycast(barrel.position, barrel.forward, out ray, weapon.weaponData.range, mask)) {
            if (ray.collider.tag == PLAYER_TAG) {
                CmdPlayerShot(ray.collider.name, weapon.weaponData.damage);
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
