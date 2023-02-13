using UnityEngine;
using Mirror;

public class scr_PlayerShoot : NetworkBehaviour {

    [SerializeField]
    private scr_WeaponData weapon;

    public LayerMask mask;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private Transform barrelEnd;

    [SerializeField]
    private scr_WeaponSway weaponSway;

    private void Start() {
        if (camera == null) {
            this.enabled = false;
        }
    }

    public void Shoot() {
        RaycastHit ray;

        Debug.Log("Shooting");

        barrelEnd.localRotation = weaponSway.transform.localRotation;

        if(Physics.Raycast(barrelEnd.position, barrelEnd.forward, out ray, weapon.range, mask)) {
            Debug.Log("Hit: " + ray.collider.name);
        }
    }
}
