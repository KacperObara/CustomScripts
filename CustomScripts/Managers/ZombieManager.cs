using System;
using System.Collections;
using System.Collections.Generic;
using CustomScripts.Gamemode.GMDebug;
using CustomScripts.Zombie;
using FistVR;
using UnityEngine;
using AIMeleeWeapon = On.FistVR.AIMeleeWeapon;
using Random = UnityEngine.Random;
using SosigSpawner = WurstMod.MappingComponents.Generic.SosigSpawner;

namespace CustomScripts.Managers
{
    public class ZombieManager : MonoBehaviourSingleton<ZombieManager>
    {
        public List<ZombieController> AllZombies;
        [HideInInspector] public List<ZombieController> ExistingZombies;

        public List<Transform> ZombieSpawnPoints;
        public List<CustomSosigSpawner> ZosigsSpawnPoints;

        public Transform ZombieTarget;

        public override void Awake()
        {
            base.Awake();

            GM.CurrentSceneSettings.SosigKillEvent += OnSosigDied;
            On.FistVR.Sosig.ProcessDamage_Damage_SosigLink += OnGetHit;
        }

        public void SpawnZombie(float delay)
        {
            StartCoroutine(DelayedZombieSpawn(delay));
        }

        public void OnZombieSpawned(ZombieController controller)
        {
            Transform spawnPoint =
                ZombieSpawnPoints[Random.Range(0, ZombieSpawnPoints.Count)];

            controller.transform.position = spawnPoint.position;

            Window targetWindow = spawnPoint.GetComponent<ZombieSpawner>().WindowWaypoint;
            if (targetWindow != null)
                ZombieTarget = targetWindow.ZombieWaypoint;

            controller.Initialize(ZombieTarget);
            ExistingZombies.Add(controller);
        }

        public void OnZosigSpawned(Sosig zosig)
        {
            ZombieController controller = zosig.gameObject.AddComponent<ZosigZombieController>();

            controller.Initialize(ZombieTarget);
            ExistingZombies.Add(controller);
        }

        public void SpawnZosig()
        {
            CustomSosigSpawner spawner =
                ZosigsSpawnPoints[Random.Range(0, ZosigsSpawnPoints.Count)];

            Window targetWindow = spawner.GetComponent<ZombieSpawner>().WindowWaypoint;
            if (targetWindow != null)
                ZombieTarget = targetWindow.ZombieWaypoint;

            spawner.SpawnCount = 1;
            spawner.SetActive(true);
        }

        public void OnZombieDied(ZombieController controller)
        {
            if (GameSettings.UseCustomEnemies)
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

            ZombieTarget = GameReferences.Instance.Player;

            if (!GameSettings.UseCustomEnemies)
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


        // private void OnSosigDied(On.FistVR.Sosig.orig_SosigDies orig, Sosig self, Damage.DamageClass damclass,
        //     Sosig.SosigDeathType deathtype)
        // {
        //     orig.Invoke(self, damclass, deathtype);
        //     //self.GetComponent<ZosigZombieController>().OnKill();
        // }

        private void OnSosigDied(Sosig sosig)
        {
            sosig.GetComponent<ZosigZombieController>().OnKill();
        }

        private void OnGetHit(On.FistVR.Sosig.orig_ProcessDamage_Damage_SosigLink orig, FistVR.Sosig self, Damage d,
            SosigLink link)
        {
            // Disable friendly fire
            if (d.Class == Damage.DamageClass.Melee &&
                d.Source_IFF != GM.CurrentPlayerBody.GetPlayerIFF()) //self.E.IFFCode
            {
                d.Dam_Blinding = 0;
                d.Dam_TotalKinetic = 0;
                d.Dam_TotalEnergetic = 0;
                d.Dam_Blunt = 0;
                d.Dam_Chilling = 0;
                d.Dam_Cutting = 0;
                d.Dam_Thermal = 0;
                d.Dam_EMP = 0;
                d.Dam_Piercing = 0;
                d.Dam_Stunning = 0;
            }

            orig.Invoke(self, d, link);
            self.GetComponent<ZosigZombieController>().OnGetHit(d);
        }

        private void OnDestroy()
        {
            GM.CurrentSceneSettings.SosigKillEvent -= OnSosigDied;
            On.FistVR.Sosig.ProcessDamage_Damage_SosigLink -= OnGetHit;
            //On.FistVR.Sosig.SosigDies -= OnSosigDied;
        }
    }
}