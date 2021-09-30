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

        //public FVRLever lever;
        //public Shop shop;
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

            //
            // if (lever.curState == FVRLever.LeverState.On)
            // {
            //     shop.TryBuying();
            // }
            // if (lever.curState == FVRLever.LeverState.Off)
            // {
            //     GameManager.Instance.AddPoints(1000);
            // }

            // zombie.GetComponent<CustomPlayerTrigger>().Enter.AddListener(UpdateEnter);
            // zombie.GetComponent<CustomPlayerTrigger>().Enter.AddListener(UpdateEnter);
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