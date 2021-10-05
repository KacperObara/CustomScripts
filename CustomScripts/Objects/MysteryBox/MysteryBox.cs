using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using WurstMod.MappingComponents.Generic;
using Random = UnityEngine.Random;

namespace CustomScripts
{
    public class MysteryBox : MonoBehaviour
    {
        public int Cost = 950;

        public List<string> WeaponsObjectID;
        public List<string> AmmoObjectID;

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
                int random = Random.Range(0, WeaponsObjectID.Count);

                WeaponSpawner.ObjectId = WeaponsObjectID[random];
                AmmoSpawner.ObjectId = AmmoObjectID[random];

                WeaponSpawner.Spawn();
                AmmoSpawner.Spawn();

                InUse = false;

                mysteryBoxMover.CurrentRoll++;
            }
        }
    }
}