using System;
using FistVR;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CustomScripts
{
    public class GameSettingsPanel : MonoBehaviour
    {
        //TODO: Set the OnClick actions
        public FVRPointableButton LimitedAmmo;
        public FVRPointableButton MoreEnemies;
        public FVRPointableButton FasterEnemies;
        public FVRPointableButton WeakerEnemies;

        public FVRPointableButton StartGame;
        
        private void Awake()
        {
            GameSettings.OnSettingsChanged += UpdateText;
        }

        private void OnDestroy()
        {
            GameSettings.OnSettingsChanged -= UpdateText;
        }

        private void UpdateText()
        {
            MoreEnemies.Text.text   = GameSettings.MoreEnemies ? "Enabled" : "Disabled";
            FasterEnemies.Text.text = GameSettings.FasterEnemies ? "Enabled" : "Disabled";
            WeakerEnemies.Text.text = GameSettings.WeakerEnemies ? "Enabled" : "Disabled";
            LimitedAmmo.Text.text   = GameSettings.LimitedAmmo ? "Enabled" : "Disabled";
        }
    }
}