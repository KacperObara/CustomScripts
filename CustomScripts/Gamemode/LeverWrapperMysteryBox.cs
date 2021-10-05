using System.Collections;
using FistVR;
using UnityEngine;
using UnityEngine.Events;

namespace CustomScripts
{
    // DELETE THIS
    public class LeverWrapperMysteryBox : MonoBehaviour
    {
        public UnityEvent LeverOnEvent;

        private TrapLever lever;

        private bool wasUsed = false;
        private bool isDelayDone = false;

        private bool startedHoldingThisFrame;
        private bool stoppedHoldingThisFrame;

        private bool isDown = false;

        private void Start()
        {
            lever = GetComponent<TrapLever>();
        }

        private void Update()
        {
            if (!lever.IsHeld)
            {
                startedHoldingThisFrame = false;
                isDelayDone = false;

                if (stoppedHoldingThisFrame)
                {
                    StartCoroutine(DelayedCheck2());
                }

                return;
            }

            if (!startedHoldingThisFrame)
            {
                startedHoldingThisFrame = true;
                stoppedHoldingThisFrame = false;
                StartCoroutine(DelayedCheck());
            }

            if (isDelayDone)
            {
                // if (Input.GetKey(KeyCode.Z))
                //     Debug.Log((object) ("Is down: " + isDown + " " + lever.ValvePos));

                if (!isDown && lever.ValvePos < .1f)
                {
                    isDown = true;
                    lever.ForceBreakInteraction();
                    LeverOnEvent.Invoke();
                }

                else if (isDown && lever.ValvePos > .9f)
                {
                    isDown = false;
                    lever.ForceBreakInteraction();
                    LeverOnEvent.Invoke();
                }

                stoppedHoldingThisFrame = true;
            }
        }

        // Lever up position is 0, lever down position is 1,
        // while you're grabbing, current position is 1, and end position is 0,
        // but the moment you grab, it's still 0 for some time.
        // That's why this abomination is here
        private IEnumerator DelayedCheck()
        {
            yield return new WaitForSeconds(.1f);
            isDelayDone = true;
        }

        private IEnumerator DelayedCheck2()
        {
            yield return new WaitForSeconds(.1f);

            stoppedHoldingThisFrame = false;

            if (isDown && lever.ValvePos < .1f)
            {
                isDown = false;
                lever.ForceBreakInteraction();
                LeverOnEvent.Invoke();
            }
            else if (!isDown && lever.ValvePos > .9f)
            {
                isDown = true;
                lever.ForceBreakInteraction();
                LeverOnEvent.Invoke();
            }
        }
    }
}