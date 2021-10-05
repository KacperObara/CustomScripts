using System;
using FistVR;
using UnityEngine;

namespace CustomScripts
{
    public class PlayerCollider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ZombieController>())
            {
                other.GetComponent<ZombieController>().OnPlayerTouch();
            }

            if (other.GetComponent<IPowerUp>() != null)
            {
                other.GetComponent<IPowerUp>().ApplyModifier();
            }

            if (other.GetComponent<IPerkBottle>() != null)
            {
                other.GetComponent<IPerkBottle>().ApplyModifier();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<ZombieController>())
            {
                other.GetComponent<ZombieController>().OnPlayerStopTouch();
            }
        }

        private void Update()
        {
            transform.position = GM.CurrentPlayerBody.Head.position;
        }
    }
}