using UnityEngine;

namespace CustomScripts
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }

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