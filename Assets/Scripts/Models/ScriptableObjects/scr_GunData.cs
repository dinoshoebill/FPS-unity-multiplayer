using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class scr_GunData : ScriptableObject {

    [Header("Gun Info")]
    public string gunName;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Ammo")]
    public int magazineSize;
    public int fireRate;
    public float reloadTime;
    public int ammoMultiplier;
    [HideInInspector]
    public bool isReloading;
}