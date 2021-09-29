using System;
using System.Collections;
using System.Collections.Generic;
using CustomScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CustomScripts
{
    public class CustomDebug : MonoBehaviour
    {
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
        }
    }
}