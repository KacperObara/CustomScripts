using System;
using FistVR;
using UnityEngine;

namespace CustomScripts.Player
{
    public class PlayerData : MonoBehaviourSingleton<PlayerData>
    {
        public PowerUpIndicator DoublePointsPowerUpIndicator;
        public PowerUpIndicator InstaKillPowerUpIndicator;
        public PowerUpIndicator DeathMachinePowerUpIndicator;

        public float DamageModifier = 1f;
        public float MoneyModifier = 1f;

        public bool InstaKill = false;

        public bool DeadShotPerkActivated = false;
        public bool DoubleTapPerkActivated = false;

        public override void Awake()
        {
            base.Awake();

            RoundManager.OnRoundChanged -= OnRoundAdvance;
            RoundManager.OnRoundChanged += OnRoundAdvance;
        }

        private void OnRoundAdvance()
        {
            GM.CurrentPlayerBody.HealPercent(1f);
        }

        private void OnDestroy()
        {
            RoundManager.OnRoundChanged -= OnRoundAdvance;
        }
    }
}