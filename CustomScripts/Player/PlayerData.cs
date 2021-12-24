using System;
using System.Collections;
using System.Collections.Generic;
using CustomScripts.Gamemode;
using FistVR;
using UnityEngine;
using Valve.VR;

namespace CustomScripts.Player
{
    public class PlayerData : MonoBehaviourSingleton<PlayerData>
    {
        public PowerUpIndicator DoublePointsPowerUpIndicator;
        public PowerUpIndicator InstaKillPowerUpIndicator;
        public PowerUpIndicator DeathMachinePowerUpIndicator;

        public float DamageModifier = 1f;
        public float MoneyModifier = 1f;

        public float LargeItemSpeedMult;
        public float MassiveItemSpeedMult;
        private float currentSpeedMult = 1f;

        public bool InstaKill = false;

        public bool DeadShotPerkActivated = false;
        public bool DoubleTapPerkActivated = false;
        public bool SpeedColaPerkActivated = false;
        public bool QuickRevivePerkActivated = false;
        public bool StaminUpPerkActivated = false;

        public override void Awake()
        {
            base.Awake();

            RoundManager.OnRoundChanged -= OnRoundAdvance;
            RoundManager.OnRoundChanged += OnRoundAdvance;

            //GM.CurrentPlayerBody.SetPlayerIFF(-20);

            On.FistVR.FVRPhysicalObject.BeginInteraction += OnPhysicalObjectStartInteraction;
            On.FistVR.FVRPhysicalObject.EndInteraction += OnPhysicalObjectEndInteraction;
            On.FistVR.FVRPhysicalObject.EndInteractionIntoInventorySlot += OnPhysicalObjectEndInteractionIntoInventorySlot;

            DeadShotPerkActivated = false;
            DoubleTapPerkActivated = false;
            SpeedColaPerkActivated = false;
            QuickRevivePerkActivated = false;
            StaminUpPerkActivated = false;
        }


        private void OnPhysicalObjectStartInteraction(On.FistVR.FVRPhysicalObject.orig_BeginInteraction orig,
            FVRPhysicalObject self, FVRViveHand hand)
        {
            orig.Invoke(self, hand);

            OnItemHeldChange();
            if (self as FVRFireArm)
            {
                WeaponWrapper wrapper = self.GetComponent<WeaponWrapper>();
                if (wrapper == null)
                {
                    wrapper = self.gameObject.AddComponent<WeaponWrapper>();
                    wrapper.Initialize((FVRFireArm) self);
                }

                wrapper.OnWeaponGrabbed();
            }
        }

        private void OnPhysicalObjectEndInteraction(On.FistVR.FVRPhysicalObject.orig_EndInteraction orig,
            FVRPhysicalObject self, FVRViveHand hand)
        {
            orig.Invoke(self, hand);
            StartCoroutine(DelayedItemChange());
        }

        private void OnPhysicalObjectEndInteractionIntoInventorySlot(On.FistVR.FVRPhysicalObject.orig_EndInteractionIntoInventorySlot orig, FVRPhysicalObject self, FVRViveHand hand, FVRQuickBeltSlot slot)
        {
            orig.Invoke(self, hand, slot);
            StartCoroutine(DelayedItemChange());
        }

        private IEnumerator DelayedItemChange()
        {
            yield return new WaitForSeconds(.1f);

            OnItemHeldChange();
        }


        private void OnRoundAdvance()
        {
            GM.CurrentPlayerBody.HealPercent(1f);
        }


        public FVRViveHand LeftHand => GM.CurrentMovementManager.Hands[0];
        public FVRViveHand RightHand => GM.CurrentMovementManager.Hands[1];


        // Called on FVRFireArm grabbed or released
        private void OnItemHeldChange()
        {
            currentSpeedMult = 1f;

            if (!StaminUpPerkActivated)
            {
                FVRPhysicalObject.FVRPhysicalObjectSize heaviestItem = FVRPhysicalObject.FVRPhysicalObjectSize.Small;

                if (LeftHand.CurrentInteractable != null && LeftHand.CurrentInteractable as FVRPhysicalObject != null)
                {
                    if (((FVRPhysicalObject) LeftHand.CurrentInteractable).Size > heaviestItem)
                        heaviestItem = ((FVRPhysicalObject) LeftHand.CurrentInteractable).Size;
                }

                if (RightHand.CurrentInteractable != null && RightHand.CurrentInteractable as FVRPhysicalObject != null)
                {
                    if (((FVRPhysicalObject) RightHand.CurrentInteractable).Size > heaviestItem)
                        heaviestItem = ((FVRPhysicalObject) RightHand.CurrentInteractable).Size;
                }

                switch (heaviestItem)
                {
                    case FVRPhysicalObject.FVRPhysicalObjectSize.Large:
                        currentSpeedMult = LargeItemSpeedMult;
                        break;
                    case FVRPhysicalObject.FVRPhysicalObjectSize.Massive:
                        currentSpeedMult = MassiveItemSpeedMult;
                        break;
                    default:
                        currentSpeedMult = 1f;
                        break;
                }
            }
            else
            {
                currentSpeedMult = 1.1f;
            }


            for (int i = 0; i < GM.Options.MovementOptions.ArmSwingerBaseSpeeMagnitudes.Length; i++)
            {
                GM.Options.MovementOptions.ArmSwingerBaseSpeeMagnitudes[i] = .75f * currentSpeedMult;
            }


            GM.CurrentSceneSettings.UsesMaxSpeedClamp = true;
            GM.CurrentSceneSettings.MaxSpeedClamp = 5.5f * currentSpeedMult;
        }

        private void OnDestroy()
        {
            RoundManager.OnRoundChanged -= OnRoundAdvance;

            On.FistVR.FVRPhysicalObject.BeginInteraction -= OnPhysicalObjectStartInteraction;
            On.FistVR.FVRPhysicalObject.EndInteraction -= OnPhysicalObjectEndInteraction;
            On.FistVR.FVRPhysicalObject.EndInteractionIntoInventorySlot -= OnPhysicalObjectEndInteractionIntoInventorySlot;

            GM.Options.MovementOptions.ArmSwingerBaseSpeeMagnitudes = new float[6]
            {
                0.0f,
                0.15f,
                0.25f,
                0.5f,
                0.8f,
                1.2f
            };
        }
    }
}