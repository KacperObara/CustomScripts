using System;
using FistVR;
using UnityEngine;

namespace CustomScripts.Player
{
    public class PlayerData : MonoBehaviourSingleton<PlayerData>
    {
        public PowerUpIndicator PowerUpIndicator;

        public float DamageModifier = 1f;
        public float MoneyModifier = 1f;

        public bool InstaKill = false;

        public override void Awake()
        {
            base.Awake();

            RoundManager.OnRoundChanged -= OnRoundAdvance;
            RoundManager.OnRoundChanged += OnRoundAdvance;
        }

        private void OnRoundAdvance()
        {
            GM.CurrentPlayerBody.HealPercent(.25f);
        }

        private void OnDestroy()
        {
            RoundManager.OnRoundChanged -= OnRoundAdvance;
        }
    }
}