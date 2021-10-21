using System;
using System.Collections.Generic;
using System.Linq;
using CustomScripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace CustomScripts
{
    /// <summary>
    /// Blocks both player and zombies, when it's removed, it checks and enables unlockable spawn points for zombies.
    /// There is no reason to spawn zombies on the blocked side of the map
    /// </summary>
    public class Blockade : MonoBehaviour
    {
        public List<ZombieSpawnPoint> UnlockableZombieSpawnPoints;

        public List<Text> CostTexts;

        public int Cost;

        private bool alreadyUsed = false;

        private void OnValidate()
        {
            foreach (Text text in CostTexts)
            {
                text.text = Cost.ToString();
            }
        }

        public void Buy()
        {
            if (alreadyUsed)
                return;

            if (!GameManager.Instance.TryRemovePoints(Cost))
                return;

            alreadyUsed = true;

            //LINQ! - Just a less verbose way of writing things <3
            foreach (ZombieSpawnPoint zombieSP in UnlockableZombieSpawnPoints.Where(zombieSP => !zombieSP.Unlocked))
            {
                zombieSP.Unlocked = true;
                ZombieManager.Instance.ZombieSpawnPoints.Add(zombieSP);
            }

            AudioManager.Instance.BuySound.Play();
            gameObject.SetActive(false);
        }
    }
}