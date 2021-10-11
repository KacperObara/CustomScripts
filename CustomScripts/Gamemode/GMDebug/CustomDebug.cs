using CustomScripts.Powerups;
using UnityEngine;

namespace CustomScripts.Gamemode.GMDebug
{
    public class CustomDebug : MonoBehaviour
    {
        public Transform Point;

        public PowerUpCarpenter Carpenter;
        public PowerUpInstaKill InstaKill;
        public PowerUpDeathMachine DeathMachine;
        public PowerUpMaxAmmo MaxAmmo;


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
    }
}