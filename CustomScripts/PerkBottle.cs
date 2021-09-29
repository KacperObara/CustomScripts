using FistVR;
using UnityEngine;

namespace CustomScripts
{
    public class PerkBottle : MonoBehaviour
    {
        public void ApplyModifier()
        {
            Player.Instance.DamageModifier = 1.5f;
            // GM.CurrentPlayerBody.SetHealthThreshold(10000);
            // GM.CurrentPlayerBody.ResetHealth();
            AudioManager.Instance.DrinkSound.Play();
            Destroy(gameObject);
        }
    }
}