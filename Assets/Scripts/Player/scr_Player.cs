using UnityEngine;
using Mirror;
using System.Collections;

public class scr_Player : NetworkBehaviour {

    private PlayerInput input;

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SyncVar]
    private bool isDead = false;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    private void Awake() {
        input = new PlayerInput();
    }

    private void InitializeInputActions() {

        scr_PlayerMotor motor = GetComponent<scr_PlayerMotor>();
        scr_PlayerShoot shoot = GetComponent<scr_PlayerShoot>();

        input.Player.Movement.performed += e => motor.MovementInput(e.ReadValue<Vector2>());

        input.Player.View.performed += e => motor.ViewInput(e.ReadValue<Vector2>());

        input.Player.Jump.performed += e => motor.JumpInput(e);

        input.Player.SprintingStart.started += e => {
            Debug.Log("started");
            motor.StartSprinting(); };

        input.Player.SprintingStop.performed += e => {
            Debug.Log("performed");
            motor.StopSprintingByRelease(); };

        input.Weapon.FireStart.started += e => shoot.Shoot();
    }

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

        scr_PlayerMotor motor = GetComponent<scr_PlayerMotor>();
        motor.inputMovement = Vector2.zero;
        motor.inputView = Vector2.zero;

        input.Disable();
        motor.player.enabled = false;

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

        scr_PlayerMotor motor = GetComponent<scr_PlayerMotor>();
        motor.inputMovement = Vector2.zero;
        motor.inputView = Vector2.zero;

        InitializeInputActions();
        input.Enable();
        motor.player.enabled = true;
    }

    public bool IsDead {
        get { return isDead;  }
        protected set { isDead = value; }
    }
}
