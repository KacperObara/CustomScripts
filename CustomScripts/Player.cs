using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

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

        public bool IsInvincible = false;

        public float DamageModifier = 1f;

        public void OnPlayerHit()
        {
            StartCoroutine(InvincibilityDuration());
        }

        private IEnumerator InvincibilityDuration()
        {
            IsInvincible = true;
            yield return new WaitForSeconds(2f);
            IsInvincible = false;
        }
    }
}