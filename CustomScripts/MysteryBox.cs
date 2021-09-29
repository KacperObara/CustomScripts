using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WurstMod.MappingComponents.Generic;

namespace CustomScripts
{
    public class MysteryBox : MonoBehaviour
    {
        private bool inUse = false;
        public int Cost = 950;

        public List<string> WeaponsObjectID;
        public List<string> AmmoObjectID;

        public ItemSpawner WeaponSpawner;
        public ItemSpawner AmmoSpawner;

        public AudioSource SpawnAudio;

        public void SpawnWeapon()
        {
            if (inUse)
                return;

            if (!GameManager.Instance.TryRemovePoints(950))
                return;

            inUse = true;
            SpawnAudio.Play();

            StartCoroutine(DelayedSpawn());
        }

        private IEnumerator DelayedSpawn()
        {
            yield return new WaitForSeconds(5.5f);
            int random = Random.Range(0, WeaponsObjectID.Count);

            WeaponSpawner.ObjectId = WeaponsObjectID[random];
            AmmoSpawner.ObjectId = AmmoObjectID[random];

            WeaponSpawner.Spawn();
            AmmoSpawner.Spawn();

            inUse = false;
        }
    }
}