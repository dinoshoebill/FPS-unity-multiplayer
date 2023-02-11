using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Weapon : MonoBehaviour {

    [Header("References")]
    [SerializeField]
    public scr_GunData gunData;

    public void Shoot() {
        Debug.Log("Pew Pew");
    }
}
