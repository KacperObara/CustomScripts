using UnityEngine;
using UnityEngine.AI;

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

            if (Health <= 0 || State == State.Dead) // second condition should be enough
                return;

            // For some reason, PlayerTrigger is often called absolutely randomly, and I don't know why,
            // My quick solution is to check the distance from the player when it's called
            if (GameReferences.Instance.IsPlayerClose(transform, 3f))
            {
                GameManager.Instance.KillPlayer();
            }
        }
    }
}