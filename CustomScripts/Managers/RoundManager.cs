using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CustomScripts
{
    public class RoundManager : MonoBehaviour
    {
        public static RoundManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public static Action OnRoundChanged;
        public static Action OnZombiesLeftChanged;
        public static Action<GameObject> OnZombieKilled;

        public GameObject StartButton;

        [HideInInspector] public int RoundNumber = 0;
        [HideInInspector] public int ZombiesLeft;

        public int ZombieStartHp = 2;

        [Tooltip("How much zombie HP is added per round")]
        public int ZombieRoundHpIncrement = 1;

        public int ZombieStartCount = 2;

        [Tooltip("How much zombies are added per round")]
        private int ZombieRoundCountIncrement; // Make public and expand

        public int ZombieFastWalkRound = 4;
        public int ZombieRunRound = 6;

        public int ZombieHP => ZombieStartHp + (RoundNumber * ZombieRoundHpIncrement); // 3 4 5 6 7...
        public int WeakerZombieHP => ZombieStartHp + (RoundNumber / 2); // 2 3 3 4 4 5 5...
        public bool IsFastWalking => RoundNumber >= ZombieFastWalkRound;
        public bool IsRunning => RoundNumber >= ZombieRunRound;


        private GameManager gameManager;
        private int zombieLimit = 20;

        private void Start()
        {
            gameManager = GameManager.Instance;
        }

        public void StartGame()
        {
            StartButton.SetActive(false);
            GameReferences.Instance.Respawn.position = gameManager.RespawnWaypoint.position;

            RoundNumber = 0;

            if (Random.Range(0, 20) == 0)
                Debug.Log("Are you sure your front doors are locked?");


            AdvanceRound();
        }

        public void AdvanceRound()
        {
            if (GameManager.Instance.GameEnded)
                return;

            RoundNumber++;

            if (GameSettings.MoreEnemies)
                ZombieRoundCountIncrement = 2;
            else
                ZombieRoundCountIncrement = 1;

            int zombiesToSpawn = ZombieStartCount + (RoundNumber * ZombieRoundCountIncrement);
            if (zombiesToSpawn > zombieLimit)
                zombiesToSpawn = zombieLimit;

            for (int i = 0; i < zombiesToSpawn; i++)
            {
                StartCoroutine(DelayedZombieSpawn(2f + i));
            }

            ZombiesLeft = zombiesToSpawn;

            AudioManager.Instance.RoundStartSound.PlayDelayed(1);

            OnZombiesLeftChanged?.Invoke();
            OnRoundChanged?.Invoke();
        }

        public void EndRound()
        {
            AudioManager.Instance.RoundEndSound.PlayDelayed(1);
            StartCoroutine(DelayedAdvanceRound());
        }

        private IEnumerator DelayedZombieSpawn(float delay)
        {
            yield return new WaitForSeconds(delay);
            ZombiePool.Instance.Spawn();
        }

        private IEnumerator DelayedAdvanceRound()
        {
            yield return new WaitForSeconds(5f);
            AdvanceRound();
        }
    }
}