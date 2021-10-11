using System;
using System.Collections;
using System.Collections.Generic;
using CustomScripts.Objects.Weapons;
using UnityEngine;
using UnityEngine.Serialization;
using WurstMod.MappingComponents.Generic;
using Random = UnityEngine.Random;

namespace CustomScripts
{
    public class MysteryBox : MonoBehaviour
    {
        public int Cost = 950;

        public List<WeaponData> LootId;
        public List<WeaponData> LimitedAmmoLootId;

        public ItemSpawner WeaponSpawner;
        public ItemSpawner AmmoSpawner;

        public AudioSource SpawnAudio;

        [HideInInspector] public bool InUse = false;

        private MysteryBoxMover mysteryBoxMover;

        private void Awake()
        {
            mysteryBoxMover = GetComponent<MysteryBoxMover>();
        }

        public void SpawnWeapon()
        {
            if (InUse)
                return;

            if (!GameManager.Instance.TryRemovePoints(950))
                return;

            InUse = true;
            SpawnAudio.Play();

            StartCoroutine(DelayedSpawn());
        }

        private IEnumerator DelayedSpawn()
        {
            yield return new WaitForSeconds(5.5f);

            if (mysteryBoxMover.TryTeleport())
            {
                mysteryBoxMover.StartTeleportAnim();
                GameManager.Instance.AddPoints(950);
            }
            else
            {
                int random = Random.Range(0, LootId.Count);

                if (GameSettings.LimitedAmmo)
                {
                    WeaponSpawner.ObjectId = LootId[random].DefaultSpawners[0];
                    WeaponSpawner.Spawn();

                    AmmoSpawner.ObjectId = LootId[random].DefaultSpawners[1];
                    for (int i = 0; i < LootId[random].LimitedAmmoMagazineCount; i++)
                    {
                        AmmoSpawner.Spawn();
                    }
                }
                else
                {
                    WeaponSpawner.ObjectId = LootId[random].DefaultSpawners[0];
                    AmmoSpawner.ObjectId = LootId[random].DefaultSpawners[1];

                    WeaponSpawner.Spawn();
                    AmmoSpawner.Spawn();
                }

                InUse = false;

                mysteryBoxMover.CurrentRoll++;
            }
        }
    }
}