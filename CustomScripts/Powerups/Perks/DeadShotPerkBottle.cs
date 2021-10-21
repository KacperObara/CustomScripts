using CustomScripts.Managers;
using CustomScripts.Zombie;
using UnityEngine;

namespace CustomScripts
{
    public class DeadShotPerkBottle : MonoBehaviour, IModifier
    {
        public void ApplyModifier()
        {
            foreach (CustomZombieController zombie in ZombieManager.Instance.AllZombies)
            {
                zombie.HeadObject.GetComponent<BoxCollider>().size = new Vector3(1.25f, 1.25f, 1.25f);
            }

            AudioManager.Instance.DrinkSound.Play();
            Destroy(gameObject);
        }
    }
}