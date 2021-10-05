using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CustomScripts
{
    public class MysteryBoxMover : MonoBehaviour
    {
        public List<Transform> SpawnPoints;
        public Transform CurrentPos;

        private Animator animator;

        public AudioSource ByeByeSound;
        public int SafeRollsProvided = 3;
        [Range(0, 100)] public float TeleportChance = 20f;

        [HideInInspector] public int CurrentRoll = 0;

        private MysteryBox mysteryBox;

        private Transform parent;
        private Vector3 startingPos;

        private void Awake()
        {
            parent = transform.parent;
            animator = parent.GetComponent<Animator>();
            mysteryBox = GetComponent<MysteryBox>();
        }

        private void Start()
        {
            startingPos = transform.localPosition;
            Teleport(true);
        }

        public void Teleport(bool firstTime = false)
        {
            Transform newPos = SpawnPoints[Random.Range(0, SpawnPoints.Count)];

            if (!firstTime)
            {
                SpawnPoints.Add(CurrentPos);
            }

            CurrentPos = newPos;
            SpawnPoints.Remove(newPos); // Exclude current transform from randomization

            parent.transform.position = newPos.position;
            parent.transform.rotation = newPos.rotation;

            CurrentRoll = 0;
            mysteryBox.InUse = false;
        }

        public bool TryTeleport()
        {
            if (CurrentRoll <= SafeRollsProvided)
                return false;

            return (Random.Range(0, 100) <= TeleportChance);
        }

        public void StartTeleportAnim()
        {
            ByeByeSound.Play();

            animator.Play("Teleport");
            StartCoroutine(DelayedTeleport());
        }

        private IEnumerator DelayedTeleport()
        {
            yield return new WaitForSeconds(4.2f);
            Teleport();
        }
    }
}