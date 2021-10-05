using System;
using UnityEngine;
using UnityEngine.UI;

namespace CustomScripts
{
    public class GameSettingsView : MonoBehaviour
    {
        public Text MoreEnemiesText;
        public Text FasterEnemiesText;
        public Text WeakerEnemiesText;
        public Text LimitedAmmoText;


        private void Awake()
        {
            GameSettings.OnSettingsChanged -= UpdateSettingsView;
            GameSettings.OnSettingsChanged += UpdateSettingsView;
        }

        public void UpdateSettingsView()
        {
            MoreEnemiesText.text = GameSettings.MoreEnemies ? "Enabled" : "Disabled";
            FasterEnemiesText.text = GameSettings.FasterEnemies ? "Enabled" : "Disabled";
            WeakerEnemiesText.text = GameSettings.WeakerEnemies ? "Enabled" : "Disabled";
            LimitedAmmoText.text = GameSettings.LimitedAmmo ? "Enabled" : "Disabled";
        }

        private void OnDestroy()
        {
            GameSettings.OnSettingsChanged -= UpdateSettingsView;
        }
    }
}