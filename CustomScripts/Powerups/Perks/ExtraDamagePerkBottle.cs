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
            Player.Instance.DamageModifier = DamageMultiplier;
            AudioManager.Instance.DrinkSound.Play();
            Destroy(ParentObject);
        }
    }
}