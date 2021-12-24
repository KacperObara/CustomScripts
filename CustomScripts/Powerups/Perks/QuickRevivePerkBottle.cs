using CustomScripts.Player;
using FistVR;
using UnityEngine;

namespace CustomScripts.Powerups.Perks
{
    public class QuickRevivePerkBottle : MonoBehaviour, IModifier
    {
        public Transform Respawn;
        public Transform RespawnWaypoint;

        public void ApplyModifier()
        {
            Respawn.position = RespawnWaypoint.position;
            PlayerData.Instance.QuickRevivePerkActivated = true;
        }
    }
}