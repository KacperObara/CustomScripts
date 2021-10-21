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
        public static bool BackgroundMusic;

        public static bool UseZosigs;

        public static bool LimitedAmmo;

        private void Start()
        {
            MoreEnemies = false;
            FasterEnemies = false;
            WeakerEnemies = false;
            LimitedAmmo = false;
            BackgroundMusic = false;
            UseZosigs = false;
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

        public void ToggleBackgroundMusic()
        {
            BackgroundMusic.Switch();
            OnSettingsChanged?.Invoke();
        }

        public void ToggleUseZosigs()
        {
            UseZosigs.Switch();
            OnSettingsChanged?.Invoke();
        }

        public void ToggleLimitedAmmo()
        {
            LimitedAmmo.Switch();
            GM.CurrentSceneSettings.IsSpawnLockingEnabled = !LimitedAmmo;
            OnSettingsChanged?.Invoke();
        }
    }
}