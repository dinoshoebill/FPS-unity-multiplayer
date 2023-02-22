using UnityEngine;
using Mirror;

[RequireComponent(typeof(scr_Player))]
[RequireComponent(typeof(scr_WeaponManager))]
public class scr_PlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] componentsToDisable;

    private Camera sceneCamera;

    [SerializeField]
    private GameObject playerUIPrefab;
    
    public GameObject playerUIInstance;

    #region - Awake / Start / Update
    private void Start() {

        sceneCamera = Camera.main;

        if (!isLocalPlayer) {
            DisableComponents();
            AssignRemoteLayer();
        } else {
            DisableSceneCamera();
        }

        playerUIInstance = Instantiate(playerUIPrefab, this.transform);
        playerUIInstance.name = playerUIPrefab.name;

        GetComponent<scr_Player>().Setup();
    }
    #endregion

    #region - Client Start -
    public override void OnStartClient() {
        base.OnStartClient();

        string id = GetComponent<NetworkIdentity>().netId.ToString();
        scr_Player player = GetComponent<scr_Player>();

        scr_GameManager.RegisterPlayer(id, player);
    }
    #endregion

    #region - Non Local Player -
    private void DisableSceneCamera() {
        if (sceneCamera != null) {
            sceneCamera.gameObject.SetActive(false);
        }
    }

    private void AssignRemoteLayer() {
        gameObject.layer = LayerMask.NameToLayer(scr_PlayerGlobals.remotePlayerLayer);
    }
    #endregion

    #region - Local Player -
    private void DisableComponents() {
        for (int i = 0; i < componentsToDisable.Length; i++)
            componentsToDisable[i].enabled = false;
    }
    #endregion

    #region - Enable / Disable -
    private void OnDisable() {

        Destroy(playerUIInstance);

        if (sceneCamera != null)
            sceneCamera.gameObject.SetActive(true);

        scr_GameManager.UnregisterPlayer(transform.name);
    }
    #endregion
}
