using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CustomScripts
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public List<AudioClip> FarZombieSounds;
        public List<AudioClip> CloseZombieSounds;

        public AudioSource BuySound;

        public AudioSource ZombieDeathSound;

        public AudioSource RoundStartSound;
        public AudioSource RoundEndSound;

        public AudioSource EndMusic;
    }
}