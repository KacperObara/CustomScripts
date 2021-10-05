using System;
using FistVR;
using UnityEngine;

namespace CustomScripts
{
    public class TestPlayerCol : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ZombieController>())
            {
                other.GetComponent<ZombieController>().OnPlayerTouch();
            }

            if (other.GetComponent<PowerUpNuke>())
            {
                other.GetComponent<PowerUpNuke>().ApplyModifier();
            }

            if (other.GetComponent<PowerUpX2>())
            {
                other.GetComponent<PowerUpX2>().ApplyModifier();
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