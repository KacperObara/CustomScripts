using FistVR;
using UnityEngine;

namespace CustomScripts
{
    public class ExtraHealthPerkBottle: MonoBehaviour, IPerkBottle
    {
        public float NewHealth = 10000;
        public GameObject ParentObject;

        public void ApplyModifier()
        {
            GM.CurrentPlayerBody.SetHealthThreshold(NewHealth);
            GM.CurrentPlayerBody.ResetHealth();

            AudioManager.Instance.DrinkSound.Play();
            Destroy(ParentObject);
        }
    }
}