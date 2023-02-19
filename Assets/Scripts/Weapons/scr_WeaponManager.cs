using UnityEngine;
using Mirror;

public class scr_WeaponManager : NetworkBehaviour {

    [SerializeField]
    private int selectedWeapon = 0;

    [SerializeField]
    public scr_Weapon currentWeapon;

    [SerializeField]
    private Transform weaponHolder;

    private void Start() {
        weaponHolder = GameObject.Find("WeaponHolder").transform;
        currentWeapon = weaponHolder.GetChild(0)?.GetComponent<scr_Weapon>();
        SelectWeapon();
    }

    private void SelectWeapon() {

        int i = 0;

        foreach (Transform weapon in weaponHolder) {
            if (i == selectedWeapon) {
                weapon.gameObject.SetActive(true);
                currentWeapon = weapon.childCount > 0 ? weapon.GetChild(0).GetComponent<scr_Weapon>() : null;
                if (currentWeapon) {
                    LayerMask weaponLayer = LayerMask.NameToLayer(scr_PlayerGlobals.weaponLayer);
                    if(currentWeapon.gameObject.layer != weaponLayer) {
                        currentWeapon.gameObject.layer = weaponLayer;
                        foreach (Transform weaponPart in currentWeapon.transform) {
                            weaponPart.gameObject.layer = weaponLayer;
                        }
                    }
                }
            } else {
                weapon.gameObject.SetActive(false);
            }

            i++;
        }
    }

    public void SwitchWeapon(float direction) {
        if(direction > 0) {
            if(selectedWeapon >= weaponHolder.childCount - 1) {
                selectedWeapon = 0;
            } else {
                selectedWeapon++;
            }
        } else if (direction < 0) {
            if (selectedWeapon <= 0) {
                selectedWeapon = weaponHolder.childCount - 1;
            }
            else {
                selectedWeapon--;
            }
        } else {
            return;
        }

        SelectWeapon();
    }

}
