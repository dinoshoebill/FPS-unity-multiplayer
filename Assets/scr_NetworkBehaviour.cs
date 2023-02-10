using UnityEngine;
using Mirror;

public class scr_NetworkBehaviour : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] componentsToDisable;
    [SerializeField]
    private Camera sceneCamera;

    private void Start() {
        if (!isLocalPlayer) {
            for (int i = 0; i < componentsToDisable.Length; i++)
                componentsToDisable[i].enabled = false;
        } else {
            sceneCamera = Camera.main;
            if(sceneCamera != null) {
                sceneCamera.gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable() {
        if (sceneCamera != null)
            sceneCamera.gameObject.SetActive(true);
    }
}
