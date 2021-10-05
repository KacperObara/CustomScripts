using UnityEngine;

namespace CustomScripts.Player
{
    public class PlayerData : MonoBehaviour
    {
        public static PlayerData Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public PowerUpIndicator PowerUpIndicator;

        public float DamageModifier = 1f;
        public float MoneyModifier = 1f;
    }
}