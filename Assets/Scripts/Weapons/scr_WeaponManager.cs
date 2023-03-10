using UnityEngine;
using Mirror;

public class scr_WeaponManager : NetworkBehaviour {

    [SerializeField]
    private int selectedWeapon = 0;

    [SerializeField]
    public scr_Weapon currentWeapon;

    [SerializeField]
    private Transform weaponHolder;

    #region - Awake / Start / Update -
    private void Start() {
        currentWeapon = weaponHolder.GetChild(0).childCount > 0 ? weaponHolder.GetChild(0).GetChild(0).GetComponent<scr_Weapon>() : null;
        SelectWeapon();
    }
    #endregion

    #region - Weapon Switching -
    private void SelectWeapon() {

        if (!isLocalPlayer)
            return;

        int i = 0;

        foreach (Transform weapon in weaponHolder) {
            if (i == selectedWeapon) {
                weapon.gameObject.SetActive(true);

                if (weapon.childCount == 0) {
                    weapon.gameObject.SetActive(false);
                    i++;
                    continue;
                }

                currentWeapon = weapon.childCount > 0 ? weapon.GetChild(0).GetComponent<scr_Weapon>() : null;

                SetLayerRecursively();
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
    #endregion

    #region - Weapon Layer -
    private void SetLayerRecursively() {
        if (!currentWeapon || !isLocalPlayer)
            return;

        LayerMask weaponLayer = LayerMask.NameToLayer(scr_PlayerGlobals.weaponLayer);
        if (currentWeapon.gameObject.layer != weaponLayer.value) {
            currentWeapon.gameObject.layer = weaponLayer.value;
            foreach (Transform weaponPart in currentWeapon.transform) {
                weaponPart.gameObject.layer = weaponLayer.value;
            }
        }
    }
    #endregion
}
