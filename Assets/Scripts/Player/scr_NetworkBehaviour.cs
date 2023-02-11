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
        if (!isLocalPlayer) {
            DisableComponents();
            AssignRemoteLayer();
        } else {
            DisableSceneCamera();
        }
    }

    private void DisableSceneCamera() {
        sceneCamera = Camera.main;
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
