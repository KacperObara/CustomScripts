using System;
using CustomScripts.Managers;
using CustomScripts.Objects;
using CustomScripts.Powerups;
using CustomScripts.Zombie;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CustomScripts.Gamemode.GMDebug
{
    public class CustomDebug : MonoBehaviour
    {
        public Transform Point;

        public AnimationCurve TestCurve;

        public PowerUpCarpenter Carpenter;
        public PowerUpInstaKill InstaKill;
        public PowerUpDeathMachine DeathMachine;
        public PowerUpMaxAmmo MaxAmmo;
        public PowerUpDoublePoints DoublePoints;
        public PowerUpNuke Nuke;

        public ElectroTrap Trap;

        public void SpawnCarpenter()
        {
            Carpenter.Spawn(Point.position);
        }

        public void SpawnInstaKill()
        {
            InstaKill.Spawn(Point.position);
        }

        public void SpawnDeathMachine()
        {
            DeathMachine.Spawn(Point.position);
        }

        public void SpawnMaxAmmo()
        {
            MaxAmmo.Spawn(Point.position);
        }

        public void SpawnDoublePoints()
        {
            DoublePoints.Spawn(Point.position);
        }

        public void SpawnNuke()
        {
            Nuke.Spawn(Point.position);
        }

        public void KillRandom()
        {
            if (ZombieManager.Instance.ExistingZombies.Count > 0)
                ZombieManager.Instance.ExistingZombies[0].OnHit(99999);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                GameSettings.Instance.ToggleUseZosigs();
                RoundManager.Instance.StartGame();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                if (ZombieManager.Instance.ExistingZombies.Count > 0)
                    ZombieManager.Instance.ExistingZombies[0].OnHit(99999);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (ZombieManager.Instance.ExistingZombies.Count > 0)
                    ZombieManager.Instance.ExistingZombies[0].OnHit(1, true);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                if (ZombieManager.Instance.ExistingZombies.Count > 0)
                    ZombieManager.Instance.ExistingZombies[0].OnHit(2);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Trap.OnLeverPull();
            }
        }
    }
}