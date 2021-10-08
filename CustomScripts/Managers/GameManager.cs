using System;
using System.Collections;
using System.Collections.Generic;
using CustomScripts.Player;
using FistVR;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CustomScripts
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public static Action OnPointsChanged;

        public List<ZombieController> AllZombies;
        public List<ZombieSpawnPoint> ZombieSpawnPoints;

        [Tooltip("Where the player should respawn on death")]
        public Transform RespawnWaypoint;

        public EndPanel EndPanel;

        [HideInInspector] public List<ZombieController> ExistingZombies;

        [HideInInspector] public int Points;
        [HideInInspector] public int TotalPoints; // for highscore

        [HideInInspector] public bool GameStarted = false;
        [HideInInspector] public bool GameEnded = false;

        public void AddPoints(int amount)
        {
            float newAmount = amount * PlayerData.Instance.MoneyModifier;

            PlayerData.Instance.MoneyModifier.ToString();

            amount = (int) newAmount;

            Points += amount;
            TotalPoints += amount;

            OnPointsChanged?.Invoke();
        }

        public bool TryRemovePoints(int amount)
        {
            if (Points >= amount)
            {
                Points -= amount;
                OnPointsChanged?.Invoke();
                return true;
            }

            return false;
        }

        public void SpawnZombie(ZombieController zombie)
        {
            int random = Random.Range(0, ZombieSpawnPoints.Count);

            zombie.transform.position = ZombieSpawnPoints[random].transform.position;

            zombie.Initialize();

            ExistingZombies.Add(zombie);
        }

        public void OnZombieDied(ZombieController controller)
        {
            StartCoroutine(DelayedZombieDespawn(controller));

            ExistingZombies.Remove(controller);

            RoundManager.Instance.ZombiesLeft--;

            RoundManager.OnZombiesLeftChanged?.Invoke();
            RoundManager.OnZombieKilled?.Invoke(controller.gameObject);


            if (ExistingZombies.Count == 0)
                RoundManager.Instance.EndRound();
        }

        private IEnumerator DelayedZombieDespawn(ZombieController controller)
        {
            yield return new WaitForSeconds(5f);
            ZombiePool.Instance.Despawn(controller);
        }

        public void StartGame()
        {
            ZombieController.playertouches = 0;
            ZombieController.isBeingHit = false;

            GM.CurrentMovementManager.TeleportToPoint(GameStart.Instance.transform.position, false);

            if (GameStarted)
                return;
            GameStarted = true;

            RoundManager.Instance.StartGame();
        }

        public void KillPlayer()
        {
            if (GameEnded)
                return;

            GameReferences.Instance.CustomScene.KillPlayer();
        }

        public void EndGame()
        {
            if (GameEnded)
                return;

            GameEnded = true;

            AudioManager.Instance.EndMusic.PlayDelayed(1f);

            EndPanel.UpdatePanel();

            if (PlayerPrefs.GetInt("BestScore") < TotalPoints)
                PlayerPrefs.SetInt("BestScore", TotalPoints);
        }
    }
}