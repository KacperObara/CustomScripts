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

        public List<ItemSpawner> ItemSpawners;

        private bool alreadyUsed = false;

        private void Start()
        {
            CostText.text = Cost.ToString();
        }

        public void TryBuying()
        {
            if (alreadyUsed)
                return;

            if (GameManager.Instance.TryRemovePoints(Cost))
            {
                alreadyUsed = true;

                foreach (ItemSpawner spawner in ItemSpawners)
                {
                    spawner.Spawn();
                }

                AudioManager.Instance.BuySound.Play();
            }
        }
    }
}