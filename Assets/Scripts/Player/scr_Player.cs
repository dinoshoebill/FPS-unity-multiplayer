using UnityEngine;
using Mirror;

public class scr_Player : NetworkBehaviour {

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    private void Awake() {
        SetPlayerSettings();
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;

        Debug.Log(transform.name + " took " + damage + " and now has " + currentHealth + " health!");
    }

    public void SetPlayerSettings() {
        currentHealth = maxHealth;
    }
}
