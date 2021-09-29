using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WurstMod.MappingComponents.Generic;

namespace CustomScripts
{
    public class Shop : MonoBehaviour
    {
        public int Cost;
        public Text CostText;
        public GameObject BuyTrigger;

        public List<ItemSpawner> ItemSpawners;

        private void Start()
        {
            CostText.text = Cost.ToString();
        }

        public void TryBuying()
        {
            if (GameManager.Instance.TryRemovePoints(Cost))
            {
                foreach (ItemSpawner spawner in ItemSpawners)
                {
                    spawner.Spawn();
                }

                AudioManager.Instance.BuySound.Play();
                BuyTrigger.SetActive(false);
            }
        }
    }
}