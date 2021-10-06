using UnityEngine;

namespace CustomScripts.Player
{
    public class PlayerData : MonoBehaviourSingleton<PlayerData>
    {
        public PowerUpIndicator PowerUpIndicator;

        public float DamageModifier = 1f;
        public float MoneyModifier = 1f;
    }
}