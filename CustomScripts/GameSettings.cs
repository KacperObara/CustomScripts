using System;
using UnityEngine;
using WurstMod.Runtime;

namespace CustomScripts
{
    public class GameSettings : MonoBehaviourSingleton<GameSettings>
    {
        public static Action OnSettingsChanged;

        public static bool MoreEnemies;
        public static bool FasterEnemies;
        public static bool WeakerEnemies;
        public static bool LimitedAmmo; // not implemented

        private void Start()
        {
            MoreEnemies = false;
            FasterEnemies = false;
            WeakerEnemies = false;
            LimitedAmmo = false;
        }

        public void ToggleMoreEnemies()
        {
            MoreEnemies = !MoreEnemies;
            OnSettingsChanged?.Invoke();
        }

        public void ToggleFasterEnemies()
        {
            FasterEnemies = !FasterEnemies;
            OnSettingsChanged?.Invoke();
        }

        public void ToggleWeakerEnemies()
        {
            WeakerEnemies = !WeakerEnemies;
            OnSettingsChanged?.Invoke();
        }

        public void ToggleLimitedAmmo() // Not working
        {
            LimitedAmmo = !LimitedAmmo;
            ObjectReferences.FVRSceneSettings.IsSpawnLockingEnabled = LimitedAmmo;
            OnSettingsChanged?.Invoke();
        }
    }
}