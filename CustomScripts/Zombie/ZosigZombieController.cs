using System.Collections;
using CustomScripts.Managers;
using CustomScripts.Player;
using UnityEngine;
using UnityEngine.AI;
using Damage = FistVR.Damage;
using Sosig = FistVR.Sosig;
using SosigLink = FistVR.SosigLink;

namespace CustomScripts.Zombie
{
    public class ZosigZombieController : ZombieController
    {
        private float agentUpdateTimer;
        private const float agentUpdateInterval = 1f;

        private Sosig sosig;

        private int hitsGivingMoney = 6;

        private bool isDead = false;
        private bool isAttackingWindow = false;

        private float cachedSpeed;

        public override void Initialize()
        {
            sosig = GetComponent<Sosig>();

            sosig.CoreRB.gameObject.AddComponent<ZosigTrigger>().Initialize(this);

            sosig.Speed_Run = 2f;
            if (RoundManager.Instance.IsFastWalking)
            {
                sosig.Speed_Run = 4f;
            }

            if (RoundManager.Instance.IsRunning)
            {
                sosig.Speed_Run = 7f;
            }

            if (GameSettings.FasterEnemies)
            {
                sosig.Speed_Run += 1.5f;
            }


            sosig.Mustard = 100 + (20 * RoundManager.Instance.RoundNumber);

            foreach (var link in sosig.Links)
            {
                link.SetIntegrity(100 + (15 * RoundManager.Instance.RoundNumber));
            }

            if (GameSettings.WeakerEnemies)
            {
                sosig.Mustard = 70 + (15 * RoundManager.Instance.RoundNumber);

                foreach (var link in sosig.Links)
                {
                    link.SetIntegrity(80 + (10 * RoundManager.Instance.RoundNumber));
                }
            }

            sosig.DamMult_Melee = .2f;
            //sosig.GetHeldMeleeWeapon().O.IsPickUpLocked = true;
            CheckPerks();
        }

        public void CheckPerks()
        {
            if (PlayerData.Instance.DeadShotPerkActivated)
            {
                sosig.DamMult_Projectile = 1.25f;
            }

            if (PlayerData.Instance.DoubleTapPerkActivated)
            {
                sosig.DamMult_Projectile = 1.25f;
            }

            if (PlayerData.Instance.DoubleTapPerkActivated && PlayerData.Instance.DeadShotPerkActivated)
            {
                sosig.DamMult_Projectile = 1.5f;
            }
        }

        private void Update()
        {
            if (sosig == null)
                return;

            agentUpdateTimer += Time.deltaTime;
            if (agentUpdateTimer >= agentUpdateInterval)
            {
                agentUpdateTimer -= agentUpdateInterval;
                sosig.SetCurrentOrder(Sosig.SosigOrder.Assault);
                sosig.UpdateAssaultPoint(GameReferences.Instance.Player.position);
            }
        }

        public void OnKill()
        {
            if (!ZombieManager.Instance.ExistingZombies.Contains(this))
                return;

            isDead = true;

            GameManager.Instance.AddPoints(100);

            ZombieManager.Instance.OnZombieDied(this);

            StartCoroutine(DelayedDespawn());
        }

        public void OnGetHit(Damage damage)
        {
            if (damage.Dam_TotalKinetic < 20)
                return;

            if (PlayerData.Instance.InstaKill)
            {
                sosig.KillSosig();
            }

            if (hitsGivingMoney <= 0)
                return;

            hitsGivingMoney--;

            GameManager.Instance.AddPoints(10);
        }

        public override void OnHit(float damage)
        {
            //nuke
            sosig.KillSosig();
        }

        public override void OnHitPlayer()
        {
        }

        public void OnTriggerEntered(Collider other)
        {
            if (isDead)
                return;

            if (isAttackingWindow)
                return;

            if (other.GetComponent<WindowTrigger>())
            {
                Window window = other.GetComponent<WindowTrigger>().Window;
                if (window.IsOpen)
                    return;

                isAttackingWindow = true;

                cachedSpeed = sosig.Speed_Run;
                sosig.Speed_Run = 0;

                LastInteractedWindow = window;
                OnTouchingWindow();
            }
        }

        public void OnTouchingWindow() // refactor this
        {
            StartCoroutine(TearPlankDelayed());
        }

        public void OnHitWindow()
        {
            LastInteractedWindow.OnPlankRipped();
        }

        private IEnumerator TearPlankDelayed()
        {
            while (!LastInteractedWindow.IsOpen)
            {
                yield return new WaitForSeconds(1.5f);

                if (!isDead)
                    OnHitWindow();
            }

            isAttackingWindow = false;
            sosig.Speed_Run = cachedSpeed;
        }

        private IEnumerator DelayedDespawn()
        {
            yield return new WaitForSeconds(5);
            sosig.DeSpawnSosig();
        }
    }
}