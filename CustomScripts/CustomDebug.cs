using System;
using System.Collections;
using System.Collections.Generic;
using CustomScripts;
using FistVR;
using UnityEngine;
using UnityEngine.UI;
using WurstMod.MappingComponents.Generic;
using Random = UnityEngine.Random;

namespace CustomScripts
{
    public class CustomDebug : MonoBehaviour
    {
        public static CustomDebug Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public TrapLever lever;
        public Shop shop;

        public MysteryBox MysteryBox;

        public PowerUpX2 PowerUp;
        public PowerUpNuke PowerUpNuke;
        public Transform DebugPos;

        private bool WriteLogs;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                RoundManager.Instance.StartGame();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                int random = Random.Range(0, GameManager.Instance.ExistingZombies.Count);

                GameManager.Instance.ExistingZombies[random].OnHit(10);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                GameManager.Instance.AddPoints(1000);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                GameManager.Instance.AddPoints(1000);
                MysteryBox.GetComponent<MysteryBoxMover>().CurrentRoll = 5;
                MysteryBox.GetComponent<MysteryBoxMover>().TeleportChance = 100;
                MysteryBox.SpawnWeapon();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                SpawnPowerUp();
                //PowerUpView.Activate(10f);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                PowerUp.ApplyModifier();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                SpawnPowerUpNuke();
                //PowerUpView.Activate(10f);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                PowerUpNuke.ApplyModifier();
            }


            if (Input.GetKeyDown(KeyCode.M))
            {
                WriteLogs = !WriteLogs;
                //lever.transform.parent.localRotation = Quaternion.Euler(0, 315, 0);
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                //lever.transform.parent.localRotation = Quaternion.Euler(0, 45, 0);
            }


            if (Input.GetKey(KeyCode.Space) || WriteLogs)
            {
                Debug.Log(lever.ValvePos);
            }


            //Debug.Log(lever.transform.parent.rotation.y + " " + lever.transform.parent.localRotation.y);
            //Debug.Log( transform.eulerAngles(lever.transform.parent.localRotation));

            // if (lever.transform.parent.localRotation.y < -0.3f)
            // {
            //     shop.TryBuying();
            //     Debug.LogWarning("ENABLED");
            // }

            // if (lever.curState == FVRLever.LeverState.Off)
            // {
            //     GameManager.Instance.AddPoints(10);
            // }

            // zombie.GetComponent<CustomPlayerTrigger>().Enter.AddListener(UpdateEnter);
            // zombie.GetComponent<CustomPlayerTrigger>().Enter.AddListener(UpdateEnter);
        }

        public void SpawnPowerUp()
        {
            PowerUp.Spawn(DebugPos.position);
        }

        public void SpawnPowerUpNuke()
        {
            PowerUpNuke.Spawn(DebugPos.position);
        }

        public void IncreasePowerUpSpawn()
        {
            PowerUpMgr.Instance.ChanceForX2PowerUp = 50f;
            //PowerUpMgr.Instance.ChanceForNukePowerUp = 25f;
            //GM.CurrentPlayerBody.SetHealthThreshold(100000);
        }


        // private float enter = 0;
        //
        // public void UpdateEnter()
        // {
        //     enter++;
        //     DebugText1.text = enter.ToString();
        // }
        //
        // private float exit = 0;
        // public void UpdateExit()
        // {
        //     exit++;
        //     DebugText2.text = enter.ToString();
        // }
        //
        // public ZombieController zombie;
        // public Text DebugText1;
        // public Text DebugText2;
    }
}