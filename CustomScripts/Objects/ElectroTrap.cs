using System;
using System.Collections;
using CustomScripts.Zombie;
using FistVR;
using UnityEngine;

namespace CustomScripts.Objects
{
    public class ElectroTrap : MonoBehaviour
    {
        public int Cost = 1000;
        public float EnabledTime = 10f;
        public float PlayerTouchDamage = 4000;

        public ParticleSystem ElectricityPS;
        public AudioSource ElectricityAudio;
        public Collider DamageTrigger;
        public GameObject PlayerBlocker;

        private bool activated = false;
        private bool damageThrottled = false;


        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ZombieController>())
            {
                other.GetComponent<ZombieController>().OnHit(99999, true);
            }
            else if (other.GetComponent<PlayerCollider>())
            {
                if (damageThrottled)
                    return;

                AudioManager.Instance.PlayerHitSound.Play();
                GM.CurrentPlayerBody.Health -= PlayerTouchDamage;

                if (GM.CurrentPlayerBody.Health <= 0)
                    GameManager.Instance.KillPlayer();

                StartCoroutine(ThrottleDamage());
            }
        }

        public void OnLeverPull()
        {
            if (activated)
                return;

            if (GameManager.Instance.TryRemovePoints(Cost))
            {
                ActivateTrap();
            }
        }

        private void ActivateTrap()
        {
            activated = true;
            ElectricityPS.Play(true);
            ElectricityAudio.Play();
            PlayerBlocker.SetActive(true);
            DamageTrigger.enabled = true;

            StartCoroutine(DelayedDeactivateTrap());
        }

        private IEnumerator DelayedDeactivateTrap()
        {
            yield return new WaitForSeconds(EnabledTime);

            activated = false;
            ElectricityPS.Stop(true);
            ElectricityAudio.Stop();
            PlayerBlocker.SetActive(false);
            DamageTrigger.enabled = false;
        }

        private IEnumerator ThrottleDamage()
        {
            damageThrottled = true;
            yield return new WaitForSeconds(1.5f);
            damageThrottled = false;
        }
    }
}