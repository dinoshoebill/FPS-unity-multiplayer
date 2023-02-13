using UnityEngine;
using Mirror;

public class scr_NetworkBehaviour : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] componentsToDisable;
    [SerializeField]
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

        RegisterPlayer();
    }

    public void RegisterPlayer() {
        string id = "Player " + GetComponent<NetworkIdentity>().netId;
        transform.name = id;
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
    }
}
