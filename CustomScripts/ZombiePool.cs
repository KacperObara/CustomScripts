using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CustomScripts
{
    public class ZombiePool : MonoBehaviour
    {
        public static ZombiePool Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public Transform DespawnedWaypoint;

        public List<ZombieController> AvailableZombies;

        public void Spawn()
        {
            //TODO no check if zombie is available to spawn, since there is a temporary limit to 20 zombies.
            GameManager.Instance.SpawnZombie(AvailableZombies[0]);
            AvailableZombies.Remove(AvailableZombies[0]);
        }

        public void Despawn(ZombieController zombie)
        {
            AvailableZombies.Add(zombie);
            zombie.transform.position = DespawnedWaypoint.position;
        }
    }
}