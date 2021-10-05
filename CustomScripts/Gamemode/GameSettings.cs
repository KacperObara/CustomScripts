using System;
using FistVR;
using UnityEngine;
using WurstMod.Runtime;

namespace CustomScripts
{
    public class GameSettings : MonoBehaviourSingleton<GameSettings>
    {
        public static event Delegates.VoidDelegate OnSettingsChanged;

        public static bool MoreEnemies;
        public static bool FasterEnemies;
        public static bool WeakerEnemies;
        public static bool LimitedAmmo; // not implemented

        private void Start()
        {
            MoreEnemies     = false;
            FasterEnemies   = false;
            WeakerEnemies   = false;
            LimitedAmmo     = false;
        }

        public void ToggleMoreEnemies()
        {
            MoreEnemies.Switch();
            OnSettingsChanged?.Invoke();
        }

        public void ToggleFasterEnemies()
        {
            FasterEnemies.Switch();
            OnSettingsChanged?.Invoke();
        }

        public void ToggleWeakerEnemies()
        {
            WeakerEnemies.Switch();
            OnSettingsChanged?.Invoke();
        }

        public void ToggleLimitedAmmo() // Not working
        {
            LimitedAmmo.Switch();
            GM.CurrentSceneSettings.IsSpawnLockingEnabled = LimitedAmmo;
            OnSettingsChanged?.Invoke();
        }
    }
}