using System;
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
        AttackWindow,
        Dead
    }

    public class ZombieController : MonoBehaviour
    {
        public int PointsOnHit = 10;
        public int PointsOnKill = 100;

        //public ZombieSpawnPoint LastSpawnPoint;
        public Window LastInteractedWindow;

        // !!!animator speed = 0.1
        // private float WalkSpeed = .8f;
        // private float FastWalkSpeed = 1.25f;
        // private float RunSpeed = 2.5f;

        private float WalkAnimationSpeed = 2f;
        private float FastWalkAnimationSpeed = .8f;
        private float RunAnimationSpeed = .9f;

        [HideInInspector] public State State;
        [HideInInspector] public float Health;

        public GameObject HeadObject;

        private float agentUpdateTimer;
        private const float agentUpdateInterval = 1f;

        private Animator animator;
        private NavMeshAgent agent;

        private RandomZombieSound soundPlayer;
        //private Transform player;

        //private Transform Target;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            soundPlayer = GetComponent<RandomZombieSound>();

            //player = GameReferences.Instance.Player;
        }

        public void Initialize()
        {
            //Target = LastSpawnPoint.Window.ZombieWaypoint;

            agentUpdateTimer = agentUpdateInterval;
            animator.SetBool("Dead", false);
            animator.SetBool("Walk", true);

            agent.speed = 0.1f;
            //agent.speed = WalkSpeed;
            if (GameSettings.FasterEnemies)
                animator.SetFloat("WalkSpeed", 1.2f);

            if (RoundManager.Instance.IsFastWalking)
            {
                //agent.speed = FastWalkSpeed;
                animator.SetBool("FastWalk", true);
                animator.SetBool("Walk", false);
                if (GameSettings.FasterEnemies)
                    animator.SetFloat("FastWalkSpeed", 1.2f);
            }

            if (RoundManager.Instance.IsRunning)
            {
                //agent.speed = RunSpeed;
                animator.SetBool("Run", true);
                animator.SetBool("FastWalk", false);
                if (GameSettings.FasterEnemies)
                    animator.SetFloat("RunSpeed", 1.25f);
            }

            if (GameSettings.WeakerEnemies)
                Health = RoundManager.Instance.WeakerZombieHP;
            else
                Health = RoundManager.Instance.ZombieHP;

            State = State.Chase;
            agent.enabled = true;

            soundPlayer.Initialize();
        }

        private void Update()
        {
            if (State == State.Dead)
                return;

            if (State == State.AttackWindow)
                return;

            agentUpdateTimer += Time.deltaTime;
            if (agentUpdateTimer >= agentUpdateInterval)
            {
                agentUpdateTimer -= agentUpdateInterval;
                agent.SetDestination(GameReferences.Instance.Player.position);
                //agent.SetDestination(GameReferences.Instance.Player.position);
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
                animator.SetBool("Walk", false);

                agent.enabled = false;
                GameManager.Instance.AddPoints(PointsOnKill);

                AudioManager.Instance.ZombieDeathSound.Play();

                GameManager.Instance.OnZombieDied(this);
            }
        }

        public void OnHitPlayer()
        {
            // if (player == null)
            //     return;

            if (State == State.Dead)
                return;

            AudioManager.Instance.PlayerHitSound.Play();
            GM.CurrentPlayerBody.Health -= 2500;
            StartCoroutine(CheckStillColliding());

            if (GM.CurrentPlayerBody.Health <= 0)
                GameManager.Instance.KillPlayer();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (State == State.Dead)
                return;

            if (other.GetComponent<WindowTrigger>())
            {
                Window window = other.GetComponent<WindowTrigger>().Window;
                if (window.IsOpen || State == State.AttackWindow)
                    return;

                LastInteractedWindow = window;
                OnTouchingWindow();
            }
        }

        public void OnTouchingWindow()
        {
            agent.speed = 0;
            animator.applyRootMotion = false;
            //.SetTrigger("PunchTrigger");
            animator.SetBool("PunchBool", true);
            State = State.AttackWindow;
        }

        public void OnHitWindow()
        {
            LastInteractedWindow.OnPlankRipped();
            //LastSpawnPoint.Window.OnPlankRipped();
        }

        public void OnHitWindowEnd()
        {
            if (LastInteractedWindow.IsOpen)
            {
                animator.SetBool("PunchBool", false);
                State = State.Chase;
                agent.speed = 0.1f;
                animator.applyRootMotion = true;
            }
            else
            {
                //animator.SetTrigger("PunchTrigger");
            }
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

        private void OnDestroy()
        {
        }
    }
}