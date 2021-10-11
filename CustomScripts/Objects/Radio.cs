using System;
using System.Collections;
using FistVR;
using UnityEngine;

namespace CustomScripts.Objects
{
    public class Radio : MonoBehaviour, IFVRDamageable
    {
        private bool isThrottled = false;

        private AudioSource audio;

        private void Awake()
        {
            audio = GetComponent<AudioSource>();
        }

        public void Damage(Damage dam)
        {
            if (dam.Class == FistVR.Damage.DamageClass.Explosive)
                return;

            if (isThrottled)
                return;

            if (!audio)
                return;

            if (audio.isPlaying)
                audio.Stop();
            else
                audio.Play();

            StartCoroutine(Throttle());
        }

        private IEnumerator Throttle()
        {
            isThrottled = true;
            yield return new WaitForSeconds(.5f);
            isThrottled = false;
        }
    }
}