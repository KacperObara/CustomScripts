using UnityEngine;

namespace CustomScripts
{
    public class BiggerZombieHitBoxPerkBottle : MonoBehaviour, IPerkBottle
    {
        public GameObject ParentObject;

        public void ApplyModifier()
        {
            foreach (ZombieController zombie in GameManager.Instance.AllZombies)
            {
                zombie.HeadObject.GetComponent<BoxCollider>().size = new Vector3(1.25f, 1.25f, 1.25f);
            }

            AudioManager.Instance.DrinkSound.Play();
            Destroy(ParentObject);
        }
    }
}