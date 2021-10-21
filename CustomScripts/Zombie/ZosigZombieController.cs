using CustomScripts.Managers;
using UnityEngine;
using UnityEngine.AI;
using Damage = FistVR.Damage;
using Sosig = FistVR.Sosig;
using SosigLink = FistVR.SosigLink;

namespace CustomScripts.Zombie
{
    public class ZosigZombieController : ZombieController
    {
        // private float agentUpdateTimer;
        // private const float agentUpdateInterval = 1f;

        //private NavMeshAgent agent;
        private Sosig sosig;

        private int hitsGivingMoney = 6;

        public override void Initialize()
        {
            Debug.Log("Initializing Zosig");
            sosig = GetComponent<Sosig>();
            //agent = sosig.Agent;


        }



        // private void Update()
        // {
        //     if (sosig == null)
        //         return;
        //
        //     agentUpdateTimer += Time.deltaTime;
        //     if (agentUpdateTimer >= agentUpdateInterval)
        //     {
        //         agentUpdateTimer -= agentUpdateInterval;
        //         agent.SetDestination(GameReferences.Instance.Player.position);
        //     }
        // }

        public void OnKill()
        {
            if (!ZombieManager.Instance.ExistingZombies.Contains(this))
                return;

            Debug.Log("KILL!");

            GameManager.Instance.AddPoints(100);

            ZombieManager.Instance.OnZombieDied(this);
        }

        public void OnGetHit(Damage damage)
        {
            if (damage.Dam_TotalKinetic < 20)
                return;

            if (hitsGivingMoney <= 0)
                return;

            hitsGivingMoney--;

            Debug.Log("HIT!: " + damage.Dam_TotalKinetic);

            GameManager.Instance.AddPoints(10);
        }

        public override void OnHit(float damage)
        {

        }

        public override void OnHitPlayer()
        {
        }
    }
}