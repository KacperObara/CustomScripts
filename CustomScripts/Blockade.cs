using System.Collections.Generic;
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

            for (int i = 0; i < UnlockableZombieSpawnPoints.Count; i++)
            {
                if (!UnlockableZombieSpawnPoints[i].gameObject.activeInHierarchy)
                {
                    UnlockableZombieSpawnPoints[i].gameObject.SetActive(true);
                    GameManager.Instance.ZombieSpawnPoints.Add(UnlockableZombieSpawnPoints[i]);
                }
            }

            AudioManager.Instance.BuySound.Play();
            gameObject.SetActive(false);
        }
    }
}