using UnityEngine;
using Mirror;

[RequireComponent(typeof(scr_Player))]
public class scr_NetworkBehaviour : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] componentsToDisable;

    private Camera sceneCamera;

    [SerializeField]
    private string remotePayerLayer = "RemotePlayer";

    private void Start() {

        sceneCamera = Camera.main;

        if (!isLocalPlayer) {
            DisableComponents();
            AssignRemoteLayer();
        } else {
            DisableSceneCamera();
        }
    }

    public override void OnStartClient() {
        base.OnStartClient();

        string id = GetComponent<NetworkIdentity>().netId.ToString();
        scr_Player player = GetComponent<scr_Player>();

        scr_GameManager.RegisterPlayer(id, player);
    }

    private void DisableSceneCamera() {
        if (sceneCamera != null) {
            sceneCamera.gameObject.SetActive(false);
        }
    }

    private void AssignRemoteLayer() {
        gameObject.layer = LayerMask.NameToLayer(remotePayerLayer);
    }

    private void DisableComponents() {
        for (int i = 0; i < componentsToDisable.Length; i++)
            componentsToDisable[i].enabled = false;
    }

    private void OnDisable() {
        if (sceneCamera != null)
            sceneCamera.gameObject.SetActive(true);

        scr_GameManager.UnregisterPlayer(transform.name);
    }
}
