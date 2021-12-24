using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomScripts.Gamemode;
using CustomScripts.Objects.Weapons;
using FistVR;
using UnityEngine;
using ItemSpawner = WurstMod.MappingComponents.Generic.ItemSpawner;
using Random = UnityEngine.Random;
using SosigSpawner = WurstMod.MappingComponents.Generic.SosigSpawner;

namespace CustomScripts.Powerups
{
    public class PackAPunch : MonoBehaviour
    {
        public int Cost;

        public List<WeaponData> WeaponsData;
        public List<ItemSpawner> Spawners;

        [HideInInspector] public bool InUse = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<FVRPhysicalObject>()) // This is not actually expensive, since it's rarely called
            {
                FVRPhysicalObject fvrPhysicalObject = other.GetComponent<FVRPhysicalObject>();

                if (fvrPhysicalObject.ObjectWrapper != null)
                {
                    TryBuying(fvrPhysicalObject);
                }
            }
        }

        public void TryBuying(FVRPhysicalObject fvrPhysicalObject)
        {
            WeaponWrapper weaponWrapper = fvrPhysicalObject.GetComponent<WeaponWrapper>();

            if (fvrPhysicalObject as FVRFireArm == null)
                return;

            // Disabling minigun since it could break the DeathMachine
            if (fvrPhysicalObject.ObjectWrapper.ItemID == "M134Minigun")
                return;

            if (weaponWrapper == null)
                return;
            if (weaponWrapper.PackAPunchDeactivated)
                return;

            WeaponData weapon = WeaponsData.FirstOrDefault(x => x.Id == fvrPhysicalObject.ObjectWrapper.ItemID);


            if (weapon) // Normal behavior with gun changes
            {
                if (GameManager.Instance.TryRemovePoints(Cost))
                {
                    if (InUse)
                        return;
                    InUse = true;

                    fvrPhysicalObject.ForceBreakInteraction();
                    fvrPhysicalObject.IsPickUpLocked = true;
                    Destroy(fvrPhysicalObject.gameObject);

                    StartCoroutine(DelayedSpawn(weapon));

                    AudioManager.Instance.BuySound.Play();
                    AudioManager.Instance.PackAPunchUpgradeSound.Play();

                    weaponWrapper.BlockPackAPunchUpgrade();
                }
            }
            else // Alternative behavior for unforeseen guns and subsequent Re pack a punching
            {
                if (GameManager.Instance.TryRemovePoints(Cost))
                {
                    if (InUse)
                        return;
                    InUse = true;

                    fvrPhysicalObject.ForceBreakInteraction();

                    StartCoroutine(DelayedReturn(fvrPhysicalObject));


                    AudioManager.Instance.BuySound.Play();
                    AudioManager.Instance.PackAPunchUpgradeSound.Play();

                    weaponWrapper.IncreaseFireRate(1.4f);

                    weaponWrapper.BlockPackAPunchUpgrade();

                    if (fvrPhysicalObject.GetComponent<Rigidbody>())
                        fvrPhysicalObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                    weaponWrapper.gameObject.SetActive(false);
                }
            }
        }

        private IEnumerator DelayedSpawn(WeaponData weapon)
        {
            yield return new WaitForSeconds(5f);

            for (int i = 0; i < weapon.UpgradedWeapon.DefaultSpawners.Count; i++)
            {
                Spawners[i].ObjectId = weapon.UpgradedWeapon.DefaultSpawners[i];
                Spawners[i].Spawn();
            }

            if (GameSettings.LimitedAmmo)
            {
                for (int i = 0; i < weapon.UpgradedWeapon.LimitedAmmoMagazineCount - 1; i++)
                {
                    Spawners[1].ObjectId = weapon.UpgradedWeapon.DefaultSpawners[1];
                    Spawners[1].Spawn();
                }
            }

            InUse = false;
        }

        private IEnumerator DelayedReturn(FVRPhysicalObject weapon)
        {
            yield return new WaitForSeconds(5f);

            weapon.transform.position = Spawners[0].transform.position;
            weapon.transform.rotation = Quaternion.identity;
            weapon.gameObject.SetActive(true);

            FVRFireArm fireArm = weapon as FVRFireArm;
            fireArm.GetComponent<WeaponWrapper>().OnPackAPunched();

            List<FVRObject> compatibleRounds = IM.OD[fireArm.ObjectWrapper.ItemID].CompatibleSingleRounds;
            List<FVRObject> compatibleMags = IM.OD[fireArm.ObjectWrapper.ItemID].CompatibleMagazines;
            List<FVRObject> compatibleClips = IM.OD[fireArm.ObjectWrapper.ItemID].CompatibleClips;
            List<FVRObject> compatibleSpeedLoaders = IM.OD[fireArm.ObjectWrapper.ItemID].CompatibleSpeedLoaders;

            // Randomizing ammo
            FVRFireArmRound randomRound = null;
            if (compatibleRounds.Count > 0)
            {
                int random = Random.Range(0, compatibleRounds.Count);
                randomRound = compatibleRounds[random].GetGameObject().GetComponent<FVRFireArmRound>();
            }

            int ammoContainersToSpawn = 1;
            if (compatibleMags.Count > 0)
            {
                // Spawning new magazine
                FVRObject newMagazine = compatibleMags
                    .OrderByDescending(x => x.MagazineCapacity)
                    .FirstOrDefault();

                if (GameSettings.LimitedAmmo)
                    ammoContainersToSpawn = 2;

                for (int i = 0; i < ammoContainersToSpawn; i++)
                {
                    GameObject magObject = Instantiate(newMagazine.GetGameObject(), Spawners[1].transform.position,
                        Quaternion.identity);

                    magObject.AddComponent<MagazineWrapper>().RoundClass = randomRound.RoundClass;

                    FVRFireArmMagazine magazine = magObject.GetComponent<FVRFireArmMagazine>();

                    magazine.ReloadMagWithType(randomRound.RoundClass);
                }
            }
            else if (compatibleClips.Count > 0)
            {
                // not sure if clips use magazine capacity, or if it does matter,
                // are there even more than 1 available clips per gun anyway?
                FVRObject newClip = compatibleClips
                    .OrderByDescending(x => x.MagazineCapacity)
                    .FirstOrDefault();

                if (GameSettings.LimitedAmmo)
                    ammoContainersToSpawn = 4;

                for (int i = 0; i < ammoContainersToSpawn; i++)
                {
                    GameObject clipObject = Instantiate(newClip.GetGameObject(), Spawners[1].transform.position,
                        Quaternion.identity);
                    clipObject.AddComponent<MagazineWrapper>().RoundClass = randomRound.RoundClass;

                    FVRFireArmClip clip = clipObject.GetComponent<FVRFireArmClip>();

                    clip.ReloadClipWithType(randomRound.RoundClass);
                }
            }
            else if (compatibleSpeedLoaders.Count > 0)
            {
                FVRObject newSpeedLoader = compatibleSpeedLoaders
                    .OrderByDescending(x => x.MagazineCapacity)
                    .FirstOrDefault();

                if (GameSettings.LimitedAmmo)
                    ammoContainersToSpawn = 4;

                for (int i = 0; i < ammoContainersToSpawn; i++)
                {
                    GameObject speedLoaderObject = Instantiate(newSpeedLoader.GetGameObject(),
                        Spawners[1].transform.position,
                        Quaternion.identity);

                    speedLoaderObject.AddComponent<MagazineWrapper>().RoundClass = randomRound.RoundClass;

                    Speedloader speedLoader = speedLoaderObject.GetComponent<Speedloader>();

                    speedLoader.ReloadClipWithType(randomRound.RoundClass);
                }
            }

            // reloading existing magazine
            FVRFireArmMagazine loadedMag = fireArm.Magazine;
            if (loadedMag && randomRound != null)
            {
                if (!loadedMag.GetComponent<MagazineWrapper>())
                    loadedMag.gameObject.AddComponent<MagazineWrapper>().RoundClass = randomRound.RoundClass;

                loadedMag.ReloadMagWithType(randomRound.RoundClass);
            }

            FVRFireArmClip loadedClip = fireArm.Clip;
            if (loadedClip && randomRound != null)
            {
                if (!loadedClip.GetComponent<MagazineWrapper>())
                    loadedClip.gameObject.AddComponent<MagazineWrapper>().RoundClass = randomRound.RoundClass;

                loadedClip.ReloadClipWithType(randomRound.RoundClass);
            }


            InUse = false;
        }

        // private IEnumerator OverrideInUse()
        // {
        //     yield return new WaitForSeconds(7f);
        //
        //     if (InUse)
        //     {
        //         Debug.Log("WTF Why in use?");
        //         InUse = false;
        //     }
        // }

        // Making bigger mags (breaks when duplicated from quickbelt slot)
        // component.ObjectWrapper.MagazineCapacity += 10 * fireArm.GetComponent<WeaponWrapper>().TimesPackAPunched;
        // component.m_capacity += 10 * fireArm.GetComponent<WeaponWrapper>().TimesPackAPunched;
        // component.m_timeSinceRoundInserted = 0.0f;
        // Array.Resize(ref component.LoadedRounds, component.m_capacity);
    }
}