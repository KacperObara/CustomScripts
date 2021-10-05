using System;
using System.Collections;
using System.Collections.Generic;
using FistVR;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CustomScripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public static Action OnPointsChanged;
        [HideInInspector] public int Points;
        [HideInInspector] public int TotalPoints; // for highscore

        public bool GameStarted = false;
        public bool GameEnded = false;

        [Tooltip("Where the player should respawn on death")]
        public Transform RespawnWaypoint;

        public EndPanel EndPanel;

        public List<ZombieController> AllZombies;
        [HideInInspector] public List<ZombieController> ExistingZombies;
        public List<ZombieSpawnPoint> ZombieSpawnPoints;

        public void AddPoints(int amount)
        {
            float newAmount = amount * Player.Instance.MoneyModifier;
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

            //zombie.LastSpawnPoint = ZombieSpawnPoints[random];
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