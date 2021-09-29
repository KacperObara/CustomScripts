using FistVR;
using UnityEngine;

namespace CustomScripts
{
    public class PerkShop : MonoBehaviour
    {
        public int Cost;

        public GameObject Bottle;
        public GameObject BuyTrigger;

        public Transform SpawnPoint;

        public void TryBuying()
        {
            if (GameManager.Instance.TryRemovePoints(Cost))
            {
                Bottle.transform.position = SpawnPoint.position;

                AudioManager.Instance.BuySound.Play();
                BuyTrigger.SetActive(false);
            }
        }
    }
}