using System;
using FistVR;
using UnityEngine;

namespace CustomScripts
{
    public class TestAmmo : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            FVRFireArmMagazine magazine = other.GetComponent<FVRFireArmMagazine>();
            if (!magazine)
                return;

            magazine.ReloadMagWithType(FireArmRoundClass.Tracer);
        }
    }
}