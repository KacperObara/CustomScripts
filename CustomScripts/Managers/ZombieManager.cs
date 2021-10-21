using System;
using System.Collections;
using System.Collections.Generic;
using CustomScripts.Gamemode.GMDebug;
using CustomScripts.Zombie;
using FistVR;
using UnityEngine;
using Random = UnityEngine.Random;
using SosigSpawner = WurstMod.MappingComponents.Generic.SosigSpawner;

namespace CustomScripts.Managers
{
    public class ZombieManager : MonoBehaviourSingleton<ZombieManager>
    {
        public List<ZombieController> AllZombies;
        [HideInInspector] public List<ZombieController> ExistingZombies;
        public List<ZombieSpawnPoint> ZombieSpawnPoints;
        public List<CustomSosigSpawner> ZosigsSpawnPoints;

        public override void Awake()
        {
            base.Awake();

            On.FistVR.Sosig.ProcessDamage_Damage_SosigLink += OnGetHit;
            On.FistVR.Sosig.SosigDies += OnSosigDied;
        }

        public void SpawnZombie(float delay)
        {
            StartCoroutine(DelayedZombieSpawn(delay));
        }

        public void OnZombieSpawned(ZombieController controller)
        {
            ZombieSpawnPoint spawnPoint =
                ZombieSpawnPoints[Random.Range(0, ZombieSpawnPoints.Count)];

            controller.transform.position = spawnPoint.transform.position;

            controller.Initialize();
            ExistingZombies.Add(controller);
        }

        public void OnZosigSpawned(Sosig zosig)
        {
            ZombieController controller = zosig.gameObject.AddComponent<ZosigZombieController>();

            controller.Initialize();
            ExistingZombies.Add(controller);

            Debug.Log("Added Zosig!");
        }

        public void SpawnZosig()
        {
            CustomSosigSpawner spawner =
                ZosigsSpawnPoints[Random.Range(0, ZosigsSpawnPoints.Count)];

            spawner.SpawnCount = 1;
            spawner.SetActive(true);
        }

        public void OnZombieDied(ZombieController controller)
        {
            if (!GameSettings.UseZosigs)
                StartCoroutine(DelayedZombieDespawn(controller.GetComponent<CustomZombieController>()));

            ExistingZombies.Remove(controller);

            RoundManager.Instance.ZombiesLeft--;

            RoundManager.OnZombiesLeftChanged?.Invoke();
            RoundManager.OnZombieKilled?.Invoke(controller.gameObject);


            if (RoundManager.Instance.ZombiesLeft <= 0) //if (ExistingZombies.Count <= 0)
            {
                RoundManager.Instance.EndRound();

                CleanZombies();
            }
        }


        // Special method to deal with weird bug
        public void CleanZombies()
        {
            for (int i = ExistingZombies.Count - 1; i >= 0; i--)
            {
                ExistingZombies[i].OnHit(9999);
                Debug.LogWarning("There are still zombies existing when they shouldn't!");
            }
        }

        private IEnumerator DelayedZombieSpawn(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (GameSettings.UseZosigs)
            {
                SpawnZosig();
            }
            else
            {
                ZombiePool.Instance.Spawn();
            }
        }

        private IEnumerator DelayedZombieDespawn(CustomZombieController controller)
        {
            yield return new WaitForSeconds(5f);
            ZombiePool.Instance.Despawn(controller);
        }


        // Zosig stuff
        private void OnSosigDied(On.FistVR.Sosig.orig_SosigDies orig, Sosig self, Damage.DamageClass damclass,
            Sosig.SosigDeathType deathtype)
        {
            self.GetComponent<ZosigZombieController>().OnKill();
        }

        private void OnGetHit(On.FistVR.Sosig.orig_ProcessDamage_Damage_SosigLink orig, FistVR.Sosig self, Damage d,
            SosigLink link)
        {
            self.GetComponent<ZosigZombieController>().OnGetHit(d);
        }

        private void OnDestroy()
        {
            On.FistVR.Sosig.ProcessDamage_Damage_SosigLink -= OnGetHit;
            On.FistVR.Sosig.SosigDies -= OnSosigDied;
        }
    }
}