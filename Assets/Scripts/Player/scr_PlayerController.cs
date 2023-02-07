using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(scr_PlayerMotor))]
public class scr_PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;

    private scr_PlayerMotor motor;

    private void Awake() {
        motor = GetComponent<scr_PlayerMotor>();   
    }

    private void Update() {
        
    }
}
