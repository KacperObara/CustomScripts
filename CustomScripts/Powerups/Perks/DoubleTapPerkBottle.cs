using CustomScripts.Player;
using FistVR;
using UnityEngine;

namespace CustomScripts
{
    public class DoubleTapPerkBottle : MonoBehaviour, IModifier
    {
        public float DamageMultiplier = 1.5f;
        public void ApplyModifier()
        {
            PlayerData.Instance.DamageModifier = DamageMultiplier;
            AudioManager.Instance.DrinkSound.Play();
            Destroy(gameObject);
        }
    }
}