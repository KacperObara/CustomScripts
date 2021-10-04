using System.Collections;
using FistVR;
using UnityEngine;
using UnityEngine.Events;
using WurstMod.MappingComponents;

namespace CustomScripts
{
    [RequireComponent(typeof(Collider))]
    public class CustomPlayerTrigger : ComponentProxy // not working
    {
        // public UnityEvent Enter;
        // public UnityEvent Exit;
        //
        // private int _inTrigger;
        //
        // public bool CheckForConstantCollision;
        // public float ConstantCollisionInterval = 2f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<FVRPlayerBody>())
            {
                Debug.Log("GRACZ TYKA!");
                // if (_inTrigger != 0) return;
                // Enter.Invoke();
                // _inTrigger++;
                //
                // if (CheckForConstantCollision)
                //     StartCoroutine(CheckStillColliding());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<FVRPlayerBody>())
            {
                Debug.Log("gracz juz nie tyka!");
            }

            // if (other.GetComponent<FVRPlayerBody>())
            // {
            //     _inTrigger--;
            //     if (_inTrigger != 0) return;
            //     Exit.Invoke();
            // }
        }

        // private IEnumerator CheckStillColliding()
        // {
        //     yield return new WaitForSeconds(ConstantCollisionInterval);
        //
        //     if (_inTrigger != 0)
        //         Enter.Invoke();
        // }
    }
}