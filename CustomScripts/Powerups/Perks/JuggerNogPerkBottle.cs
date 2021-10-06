using FistVR;
using UnityEngine;

namespace CustomScripts
{
    public class JuggerNogPerkBottle: MonoBehaviour, IPerkBottle
    {
        public float NewHealth = 10000;

        public void ApplyModifier()
        {
            GM.CurrentPlayerBody.SetHealthThreshold(NewHealth);
            GM.CurrentPlayerBody.ResetHealth();

            AudioManager.Instance.DrinkSound.Play();
            Destroy(gameObject);
        }
    }
}