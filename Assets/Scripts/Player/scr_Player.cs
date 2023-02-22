using UnityEngine;
using Mirror;
using System.Collections;

public class scr_Player : NetworkBehaviour {

    private PlayerInput input;

    [SyncVar]
    private int currentHealth;

    [SyncVar]
    private bool isDead = false;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    #region - Awake / Start / Update
    private void Awake() {
        input = new PlayerInput();
    }
    #endregion

    #region - Setup -
    public void Setup() {
        wasEnabled = new bool[disableOnDeath.Length];

        for (int i = 0; i < disableOnDeath.Length; i++) {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        InitializeInputActions();
        SetPlayerSettings();
    }
    #endregion

    #region - Input -
    private void InitializeInputActions() {

        scr_PlayerMotor motor = GetComponent<scr_PlayerMotor>();
        scr_PlayerShoot shoot = GetComponent<scr_PlayerShoot>();
        scr_WeaponManager weapon = GetComponent<scr_WeaponManager>();

        input.Player.Movement.performed += e =>
            motor.MovementInput(e.ReadValue<Vector2>());

        input.Player.View.performed += e =>
            motor.ViewInput(e.ReadValue<Vector2>());

        input.Player.Jump.performed += e =>
            motor.JumpInput(e);

        input.Player.SprintingStart.started += e =>
            motor.StartSprinting();

        input.Player.SprintingStop.performed += e =>
            motor.StopSprintingByRelease();

        input.Weapon.FireStart.started += e => shoot.Shoot();

        input.Weapon.WeaponSwitch.performed += e => weapon.SwitchWeapon(e.ReadValue<Vector2>().y);

        input.Enable();
    }
    #endregion

    #region - Damage -
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
    #endregion

    #region - Die / Respawn -
    private void Die() {

        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = false;
        }

        input.Disable();

        Debug.Log(transform.name + " DIED! :O");

        var children = this.GetComponentsInChildren<Transform>(includeInactive: true);

        foreach (var child in children) {
            child.gameObject.layer = LayerMask.NameToLayer(scr_PlayerGlobals.deadPlayerLayer);
        }

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn() {
        yield return new WaitForSeconds(scr_GameManager.instance.matchSettings.respawnTime);

        Transform spawnpoint = NetworkManager.singleton.GetStartPosition();

        scr_PlayerMotor motor = GetComponent<scr_PlayerMotor>();
        motor.player.enabled = false;
        transform.position = spawnpoint.position;
        transform.rotation = spawnpoint.rotation;
        motor.player.enabled = true;

        SetPlayerSettings();
    }
    #endregion

    #region - Player Settings -
    public void SetPlayerSettings() {

        isDead = false;
        currentHealth = scr_PlayerGlobals.health;

        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        var children = this.GetComponentsInChildren<Transform>(includeInactive: true);

        if (!isLocalPlayer)
            foreach (var child in children)
                child.gameObject.layer = LayerMask.NameToLayer(scr_PlayerGlobals.remotePlayerLayer);
        else
            foreach (var child in children)
                child.gameObject.layer = LayerMask.NameToLayer(scr_PlayerGlobals.localPlayerLayer);

        input.Enable();
    }
    #endregion

    public bool IsDead {
        get { return isDead;  }
        protected set { isDead = value; }
    }
}
