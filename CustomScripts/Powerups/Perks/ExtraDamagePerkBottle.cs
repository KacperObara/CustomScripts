using CustomScripts.Player;
using FistVR;
using UnityEngine;

namespace CustomScripts
{
    public class ExtraDamagePerkBottle : MonoBehaviour, IPerkBottle
    {
        public float DamageMultiplier = 1.5f;
        public GameObject ParentObject;

        public void ApplyModifier()
        {
            PlayerData.Instance.DamageModifier = DamageMultiplier;
            AudioManager.Instance.DrinkSound.Play();
            Destroy(ParentObject);
        }
    }
}