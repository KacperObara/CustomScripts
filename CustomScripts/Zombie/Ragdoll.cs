using System;
using System.Collections;
using UnityEngine;

namespace CustomScripts.Zombie
{
    public class Ragdoll : MonoBehaviour
    {
        private Rigidbody[] rbs;
        private Animator animator;
        private CustomZombieController controller;

        private void Awake()
        {
            controller = GetComponent<CustomZombieController>();
            rbs = GetComponentsInChildren<Rigidbody>();
            animator = GetComponent<Animator>();

            controller.OnZombieDied += ActivateRagdoll;
            controller.OnZombieInitialize += DeactivateRagdoll;
        }

        private void DeactivateRagdoll()
        {
            animator.enabled = true;
            foreach (var rb in rbs)
            {
                rb.isKinematic = true;
            }
        }

        private void ActivateRagdoll()
        {
            animator.enabled = false;
            foreach (var rb in rbs)
            {
                rb.isKinematic = false;
            }

            StartCoroutine(DampenFall());
        }

        private IEnumerator DampenFall()
        {
            yield return new WaitForSeconds(.1f);
            foreach (var rb in rbs)
            {
                rb.velocity = Vector3.zero;
            }
        }

        private void OnDestroy()
        {
            controller.OnZombieDied -= ActivateRagdoll;
            controller.OnZombieInitialize -= DeactivateRagdoll;
        }
    }
}