using UnityEngine;
using Mirror;
using System.Collections;

public class scr_Player : NetworkBehaviour {

    [SyncVar]
    private bool isDead = false;

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    public void Setup() {
        wasEnabled = new bool[disableOnDeath.Length];

        for (int i = 0; i < disableOnDeath.Length; i++) {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetPlayerSettings();
    }

    [ClientRpc]
    public void RpcTakeDamage(int damage) {

        if (isDead)
            return;

        currentHealth -= damage;

        Debug.Log(transform.name + " took " + damage + " and now has " + currentHealth + " health!");
    
        if(currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = false;
        }

        GetComponent<CharacterController>().enabled = false;

        Debug.Log(transform.name + " DIED! :O");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn() {
        yield return new WaitForSeconds(scr_GameManager.instance.matchSettings.respawnTime);

        Transform spawnpoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnpoint.position;
        transform.rotation = spawnpoint.rotation;
        SetPlayerSettings();
    }

    public void SetPlayerSettings() {
        isDead = false;

        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        GetComponent<CharacterController>().enabled = true;
    }

    public bool IsDead {
        get { return isDead;  }
        protected set { isDead = value; }
    }
}
