using System;
using System.Collections;
using FistVR;
using UnityEngine;
using UnityEngine.Serialization;

namespace CustomScripts
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        
        
        public PowerUpView PowerUpView;

        public float DamageModifier = 1f;
        public float MoneyModifier = 1f;
    }
}