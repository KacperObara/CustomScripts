using System;
using CustomScripts.Zombie;
using FistVR;
using UnityEngine;

namespace CustomScripts
{
    public class PlayerCollider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<CustomZombieController>()?.OnPlayerTouch();
        }

        private void OnTriggerExit(Collider other)
        {
            other.GetComponent<CustomZombieController>()?.OnPlayerStopTouch();
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.CompareTag("Zombie"))
        //     {
        //         other.GetComponent<ZombieController>()?.OnPlayerTouch();
        //     }
        //
        //     if (other.CompareTag("PlayerInteractable"))
        //     {
        //         other.GetComponent<IModifier>()?.ApplyModifier();
        //     }
        // }
        //
        // private void OnTriggerExit(Collider other)
        // {
        //     if (other.CompareTag("Zombie"))
        //     {
        //         other.GetComponent<ZombieController>()?.OnPlayerStopTouch();
        //     }
        // }

        private void Update()
        {
            transform.position = GM.CurrentPlayerBody.Head.position;
        }
    }
}