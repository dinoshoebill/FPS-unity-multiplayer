using System.Collections.Generic;
using UnityEngine;

public class scr_GameManager : MonoBehaviour {

    #region - Variables -
    public static scr_GameManager instance;
    public scr_MatchSettings matchSettings;

    private const string PLAYER_ID_PREFIX = "P";

    private static Dictionary<string, scr_Player> players = new Dictionary<string, scr_Player>();
    #endregion

    #region - Awake / Update -
    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than 1 instance of game manager is running");
        } else {
            instance = this;
        }
    }
    #endregion

    #region - Player Register / Unregister -
    public static void RegisterPlayer(string id, scr_Player player) {
        string newId = PLAYER_ID_PREFIX + id;
        players.Add(newId, player);
        player.transform.name = newId;
    }

    public static void UnregisterPlayer(string id) {
        players.Remove(id);
    }

    public static scr_Player GetPlayer(string id) {
        return players[id];
    }
    #endregion

    //private void OnGUI() {
    //    GUILayout.BeginArea(new Rect(200, 200, 200, 500));
    //    GUILayout.BeginVertical();

    //    foreach(string id in players.Keys) {
    //        GUILayout.Label(id + "   -   " + players[id].transform.name);
    //    }

    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}

}
