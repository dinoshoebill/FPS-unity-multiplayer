using UnityEngine;
using Mirror;

public class scr_PlayerShoot : NetworkBehaviour {

    public scr_PlayerWeapon weapon;

    [SerializeField]
    private Camera camera;

    private void Start() {
        if (camera == null) {
            this.enabled = false;
        }
    }

    public void Shoot() {

    }
}
