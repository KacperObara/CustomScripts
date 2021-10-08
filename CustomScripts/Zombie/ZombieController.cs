using System.Collections;
using CustomScripts.Player;
using FistVR;
using UnityEngine;
using UnityEngine.AI;

namespace CustomScripts
{
    public enum State
    {
        Chase,
        AttackWindow,
        Dead
    }

    // TODO string based property lookup is inefficient
    public class ZombieController : MonoBehaviour
    {
        public int PointsOnHit = 10;
        public int PointsOnKill = 100;

        public GameObject HeadObject;

        [HideInInspector] public Window LastInteractedWindow;
        [HideInInspector] public State State;
        [HideInInspector] public float Health;

        private Animator animator;
        private NavMeshAgent agent;
        private RandomZombieSound soundPlayer;

        private float agentUpdateTimer;
        private const float agentUpdateInterval = 1f;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            soundPlayer = GetComponent<RandomZombieSound>();
        }

        public void Initialize()
        {
            agentUpdateTimer = agentUpdateInterval;
            animator.SetBool("Dead", false);
            animator.SetBool("Walk", true);

            agent.speed = 0.1f;
            if (GameSettings.FasterEnemies)
                animator.SetFloat("WalkSpeed", 1.2f);

            if (RoundManager.Instance.IsFastWalking)
            {
                animator.SetBool("FastWalk", true);
                animator.SetBool("Walk", false);
                if (GameSettings.FasterEnemies)
                    animator.SetFloat("FastWalkSpeed", 1.2f);
            }

            if (RoundManager.Instance.IsRunning)
            {
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
            }
        }

        public void OnHit(int Damage)
        {
            if (Health <= 0)
                return;

            float newDamage = Damage * PlayerData.Instance.DamageModifier;

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
            if (State == State.Dead)
                return;

            isBeingHit = true;
            playertouches++;

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
            animator.SetBool("PunchBool", true);
            State = State.AttackWindow;
        }

        public void OnHitWindow()
        {
            LastInteractedWindow.OnPlankRipped();
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
        }

        public static int playertouches = 0;
        public static bool isBeingHit = false;

        public void OnPlayerTouch()
        {
            if (playertouches != 0)
                return;

            if (isBeingHit)
                return;

            OnHitPlayer();
        }

        public void OnPlayerStopTouch()
        {
            if (playertouches == 0)
                return;

            playertouches--;
        }

        private IEnumerator CheckStillColliding()
        {
            yield return new WaitForSeconds(1.5f);

            if (playertouches != 0 && !GameManager.Instance.GameEnded)
            {
                OnHitPlayer();
            }

            isBeingHit = false;
        }
    }
}