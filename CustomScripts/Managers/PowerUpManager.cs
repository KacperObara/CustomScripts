using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CustomScripts
{
    public class PowerUpManager : MonoBehaviour
    {
        public static PowerUpManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            RoundManager.OnZombieKilled -= RollForPowerUp;
            RoundManager.OnZombieKilled += RollForPowerUp;
        }

        public PowerUpX2 PowerUpDoublePoints;
        public bool DoublePointsUsed = false;

        public PowerUpNuke PowerUpNuke;
        public bool NukeUsed = false;

        public float ChanceForNukePowerUp = 1f;
        public float ChanceForX2PowerUp = 4f;
        public int LastPowerUpRound = 0;

        public void RollForPowerUp(GameObject spawnPos) // TODO Temporary algorithm until more power ups are added
        {
            // no 2 power ups in one round until better balance
            if (LastPowerUpRound == RoundManager.Instance.RoundNumber)
                return;

            int chance = Random.Range(0, 100);
            if (chance < ChanceForNukePowerUp)
            {
                LastPowerUpRound = RoundManager.Instance.RoundNumber;
                SpawnPowerUp(PowerUpNuke, spawnPos.transform.position);
                return;
            }

            chance = Random.Range(0, 100);
            if (chance < ChanceForX2PowerUp)
            {
                LastPowerUpRound = RoundManager.Instance.RoundNumber;
                SpawnPowerUp(PowerUpDoublePoints, spawnPos.transform.position);
                return;
            }
        }

        public void SpawnPowerUp(IPowerUp powerUp, Vector3 pos)
        {
            if (powerUp == null)
            {
                Debug.LogWarning("PowerUp spawn failed! IpowerUp == null Tell Kodeman");
                return;
            }

            powerUp.Spawn(pos + Vector3.up);
        }

        private void OnDestroy()
        {
            RoundManager.OnZombieKilled -= RollForPowerUp;
        }
    }
}