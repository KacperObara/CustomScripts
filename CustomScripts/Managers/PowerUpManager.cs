using System;
using System.Collections;
using CustomScripts.Powerups;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CustomScripts
{
    public class PowerUpManager : MonoBehaviourSingleton<PowerUpManager>
    {
        public override void Awake()
        {
            base.Awake();

            RoundManager.OnZombieKilled -= RollForPowerUp;
            RoundManager.OnZombieKilled += RollForPowerUp;
        }

        public PowerUpDoublePoints PowerUpDoublePoints;
        public PowerUpNuke PowerUpNuke;
        public PowerUpCarpenter PowerUpCarpenter;
        public PowerUpInstaKill PowerUpInstaKill;
        public PowerUpDeathMachine PowerUpDeathMachine;
        public PowerUpMaxAmmo PowerUpMaxAmmo;

        public float ChanceForNukePowerUp = 1f;
        public float ChanceForX2PowerUp = 4f;
        public float ChanceForInstaKill = 2f;
        public float ChanceForCarpenter = 4f;
        public float ChanceForDeathMachine = 1f;
        public float ChanceForAmmo = 5f;

        public bool IsPowerUpCooldown = false;
        public bool IsMaxAmmoCooldown = false;

        public void RollForPowerUp(GameObject spawnPos) // TODO Temporary algorithm until more power ups are added
        {
            int chance = Random.Range(0, 200);
            if (GameSettings.LimitedAmmo && !IsMaxAmmoCooldown)
            {
                if (chance < ChanceForAmmo)
                {
                    StartCoroutine(PowerUpMaxAmmoCooldown());
                    SpawnPowerUp(PowerUpMaxAmmo, spawnPos.transform.position);
                    return;
                }
            }

            if (IsPowerUpCooldown) //30 sec cooldown between power ups
                return;

            chance = Random.Range(0, 200);
            if (chance < ChanceForNukePowerUp)
            {
                SpawnPowerUp(PowerUpNuke, spawnPos.transform.position);
                return;
            }

            chance = Random.Range(0, 200);
            if (chance < ChanceForX2PowerUp)
            {
                SpawnPowerUp(PowerUpDoublePoints, spawnPos.transform.position);
                return;
            }

            chance = Random.Range(0, 200);
            if (chance < ChanceForCarpenter)
            {
                SpawnPowerUp(PowerUpCarpenter, spawnPos.transform.position);
                return;
            }

            chance = Random.Range(0, 200);
            if (chance < ChanceForInstaKill)
            {
                SpawnPowerUp(PowerUpInstaKill, spawnPos.transform.position);
                return;
            }

            // chance = Random.Range(0, 200);
            // if (chance < ChanceForDeathMachine)
            // {
            //     SpawnPowerUp(PowerUpDeathMachine, spawnPos.transform.position);
            //     return;
            // }
        }

        public void SpawnPowerUp(IPowerUp powerUp, Vector3 pos)
        {
            if (powerUp == null)
            {
                Debug.LogWarning("PowerUp spawn failed! IpowerUp == null Tell Kodeman");
                return;
            }

            StartCoroutine(PowerUpCooldown());
            powerUp.Spawn(pos + Vector3.up);
        }

        private IEnumerator PowerUpCooldown()
        {
            IsPowerUpCooldown = true;
            yield return new WaitForSeconds(30f);
            IsPowerUpCooldown = false;
        }

        private IEnumerator PowerUpMaxAmmoCooldown()
        {
            IsMaxAmmoCooldown = true;
            yield return new WaitForSeconds(30f);
            IsMaxAmmoCooldown = false;
        }

        private void OnDestroy()
        {
            RoundManager.OnZombieKilled -= RollForPowerUp;
        }
    }
}