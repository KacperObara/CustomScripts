using CustomScripts.Player;
using UnityEngine;

namespace CustomScripts.Powerups.Perks
{
    public class StaminUpPerkBottle : MonoBehaviour, IModifier
    {
        public void ApplyModifier()
        {
            PlayerData.Instance.StaminUpPerkActivated = true;

            AudioManager.Instance.DrinkSound.Play();
            Destroy(gameObject);
        }
    }
}