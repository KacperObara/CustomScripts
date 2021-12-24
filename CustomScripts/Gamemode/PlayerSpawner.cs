using System;
using System.Collections;
using CustomScripts.Player;
using FistVR;
using UnityEngine;

namespace CustomScripts.Gamemode
{
    public class PlayerSpawner : MonoBehaviour
    {
        public Transform EndGameSpawnerPos;

        private void Awake()
        {
            GM.CurrentSceneSettings.PlayerDeathEvent += OnPlayerDeath;
        }

        private void OnPlayerDeath(bool killedself)
        {
            if (PlayerData.Instance.QuickRevivePerkActivated)
            {
                PlayerData.Instance.QuickRevivePerkActivated = false;

                StartCoroutine(DelayedInvincibility());
            }
        }

        private IEnumerator DelayedInvincibility()
        {
            yield return new WaitForSeconds(3f);

            GM.CurrentPlayerBody.ActivatePower(PowerupType.Invincibility, PowerUpIntensity.High, PowerUpDuration.Short,
                       false, false);

            yield return new WaitForSeconds(2f);

            transform.position = EndGameSpawnerPos.position;
        }

        private void OnDestroy()
        {
            GM.CurrentSceneSettings.PlayerDeathEvent -= OnPlayerDeath;
        }
    }
}