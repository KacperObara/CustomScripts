using System.Collections;
using FistVR;
using UnityEngine;
using UnityEngine.AI;
using WurstMod.MappingComponents.Generic;

namespace CustomScripts
{
    public enum State
    {
        Chase,
        Dead
    }

    public class ZombieController : MonoBehaviour
    {
        public int PointsOnHit = 10;
        public int PointsOnKill = 100;

        public float WalkSpeed = 0.8f;
        public float FastWalkSpeed = 1.25f;
        public float RunSpeed = 2.5f;

        [HideInInspector] public State State;
        [HideInInspector] public float Health;

        public GameObject HeadObject;

        private float agentUpdateTimer;
        private const float agentUpdateInterval = 1f;

        private Animator animator;
        private NavMeshAgent agent;
        private RandomZombieSound soundPlayer;
        private Transform player;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            soundPlayer = GetComponent<RandomZombieSound>();

            player = GameReferences.Instance.Player;

            //GetComponent<Trigger>().enterEvent.AddListener(OnPlayerTouch);
            //GetComponent<Trigger>().enterEvent.AddListener(OnPlayerStopTouch);
        }

        public void Initialize()
        {
            agentUpdateTimer = agentUpdateInterval;
            animator.SetBool("Dead", false);

            agent.speed = WalkSpeed;
            if (RoundManager.Instance.IsFastWalking)
            {
                agent.speed = FastWalkSpeed;
                animator.SetBool("FastWalk", true);
            }

            if (RoundManager.Instance.IsRunning)
            {
                agent.speed = RunSpeed;
                animator.SetBool("Run", true);
            }

            Health = RoundManager.Instance.ZombieHP;

            State = State.Chase;
            agent.enabled = true;

            soundPlayer.Initialize();
        }

        private void Update()
        {
            if (State == State.Dead)
                return;

            agentUpdateTimer += Time.deltaTime;
            if (agentUpdateTimer >= agentUpdateInterval)
            {
                agentUpdateTimer -= agentUpdateInterval;
                agent.SetDestination(GameReferences.Instance.Player.position);
            }
        }

        public void OnHit(int Damage)
        {
            if (Health <= 0)
                return;

            float newDamage = Damage * Player.Instance.DamageModifier;
            Damage = (int) newDamage;

            AudioManager.Instance.ZombieHitSound.Play();
            GameManager.Instance.AddPoints(PointsOnHit);
            Health -= Damage;

            if (Health <= 0)
            {
                State = State.Dead;
                animator.SetBool("Dead", true);

                agent.enabled = false;
                GameManager.Instance.AddPoints(PointsOnKill);

                AudioManager.Instance.ZombieDeathSound.Play();

                GameManager.Instance.OnZombieDied(this);
            }
        }

        public void OnHitPlayer()
        {
            if (player == null)
                return;

            if (State == State.Dead)
                return;


            AudioManager.Instance.PlayerHitSound.Play();
            GM.CurrentPlayerBody.Health -= 2500;
            StartCoroutine(CheckStillColliding());

            if (GM.CurrentPlayerBody.Health <= 0)
                GameManager.Instance.KillPlayer();
        }


        private int playertouches = 0;

        public void OnPlayerTouch()
        {
            playertouches++;
            OnHitPlayer();
        }

        public void OnPlayerStopTouch()
        {
            playertouches--;
        }

        private IEnumerator CheckStillColliding()
        {
            yield return new WaitForSeconds(2f);
            if (playertouches != 0)
            {
                OnHitPlayer();
            }
        }
    }
}