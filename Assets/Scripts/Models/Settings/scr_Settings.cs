using UnityEngine;
using System;

public static class scr_Settings {

    #region - Player -
    [Serializable]
    public class PlayerSettingsModel {

        [Header("View Settings")]
        public float viewXSensitivity;
        public float viewYSensitivity;
        public float viewClampYMin;
        public float viewClampYMax;

        [Header("Movement Settings")]
        public float speedSprint;
        public float speedStand;

        [Header("Movement Smoothing")]
        public float movementSmoothing;
        public float airTimeSmoothing;

        [Header("Jump Settings")]
        public float jumpPower;

        [Header("Gravity Settings")]
        public float gravity;

        [Header("Multipliers")]
        public float speedStrafeMultiplier;
        public float speedStrafeSprintMultiplier;
        public float gravityMultiplier;
        public float doubleJumpMultiplier;
    }
    #endregion

    #region - Weapons -
    [Serializable]
    public class WeaponsSettingsModel {

        [Header("Weapon Sway")]
        public float swayAmount;
        public float swayClampX;
        public float swayClampY;
        public float swayClampZ;
        public float swayAmountA;
        public float swayAmountB;
        public float swayLerpSpeed;
        public float swayScale;

        [Header("Weapon Movement Sway")]
        public float movementSwayAmount;

        [Header("Weapon Smoothing")]
        public float swaySmoothing;

        [Header("Multipliers")]
        public float animationSpeedMultiplier;
    }
    #endregion

}
