using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomScripts
{
    /// <summary>
    /// Blocks both player and zombies, when it's removed, it checks and enables unlockable spawn points for zombies.
    /// There is no reason to spawn zombies on the blocked side of the map
    /// </summary>
    public class Blockade : MonoBehaviour
    {
        public List<Transform> UnlockableZombieSpawnPoints;

        private bool alreadyUsed = false;
        public int Cost;

        public void Buy()
        {
            if (alreadyUsed)
                return;

            if (!GameManager.Instance.TryRemovePoints(Cost))
                return;

            alreadyUsed = true;

            //LINQ! - Just a less verbose way of writing things
            foreach (Transform zombieSP in UnlockableZombieSpawnPoints.Where(zombieSP => !zombieSP.gameObject.activeInHierarchy))
            {
                zombieSP.gameObject.SetActive(true);
                GameManager.Instance.ZombieSpawnPoints.Add(zombieSP);
            }

            AudioManager.Instance.BuySound.Play();
            gameObject.SetActive(false);
        }
    }
}