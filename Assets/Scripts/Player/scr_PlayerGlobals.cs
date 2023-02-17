using UnityEngine;

public static class scr_PlayerGlobals {

    [Header("Health")]
    public static int health = 100;

    [Header("Layers")]
    public static string localPlayerLayer = "Player";
    public static string remotePlayerLayer = "RemotePlayer";
    public static string deadPlayerLayer = "DeadPlayer";
    public static string weaponLayer = "Weapon";

    [Header("View")]
    public static float viewXSensitivity = 12;
    public static float viewYSensitivity = 12;
    public static float viewClampYMin = -70;
    public static float viewClampYMax = 70;

    [Header("Speed")]
    public static float speedSprint = 12;
    public static float speedStand = 8;
    public static float speedStrafeMultiplier = 0.7f;
    public static float speedStrafeSprintMultiplier = 0.3f;

    [Header("Movement Smoothing")]
    public static float movementSmoothing = 0.3f;
    public static float airTimeSmoothing = 0.1f;

    [Header("Jump")]
    public static float jumpPower = 15;
    public static float doubleJumpMultiplier = 1.3f;

    [Header("Gravity")]
    public static float gravity = -10;
    public static float gravityMultiplier = 5;

    [Header("Weapon Sway")]
    public static float swayAmount = 1;
    public static float swayClampX = 3;
    public static float swayClampY = 3;

    [Header("Weapon Smoothing")]
    public static float swaySmoothing = 0.1f;

}
