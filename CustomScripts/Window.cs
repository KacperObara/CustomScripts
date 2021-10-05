using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FistVR;
using UnityEngine;

namespace CustomScripts
{
    public class WindowPlank : MonoBehaviour
    {
        public Plank Plank;
    }

    public class Window : MonoBehaviour
    {
        public List<WindowPlank> PlankSlots;

        public Transform ZombieWaypoint;

        public int PlanksRemain { get; set; } // Overcomplicated a little
        public bool IsOpen => PlanksRemain == 0;

        public List<Plank> AllPlanks;

        private AudioSource TearPlankAudio;

        private void Start()
        {
            TearPlankAudio = GetComponent<AudioSource>();
            RepairAll();
        }

        public void RepairAll()
        {
            for (int i = 0; i < PlankSlots.Count; i++)
            {
                PlankSlots[i].Plank = AllPlanks[i];
                AllPlanks[i].transform.position = PlankSlots[i].transform.position;
                AllPlanks[i].transform.rotation = PlankSlots[i].transform.rotation;
                AllPlanks[i].PhysicalObject.ForceBreakInteraction();
                AllPlanks[i].PhysicalObject.IsPickUpLocked = true;

                PlanksRemain++;
            }
        }

        public void OnPlankTouch(Plank plank)
        {
            WindowPlank windowPlank = PlankSlots.FirstOrDefault(x => x.Plank == null);
            if (!windowPlank)
                return;

            windowPlank.Plank = plank;

            plank.PhysicalObject.ForceBreakInteraction();
            plank.PhysicalObject.IsPickUpLocked = true;

            plank.OnRepairDrop(windowPlank.transform);
            //plank.transform.position = windowPlank.transform.position;
            //plank.transform.rotation = windowPlank.transform.rotation;
            PlanksRemain++;
        }

        public void OnPlankRipped()
        {
            WindowPlank windowPlank = PlankSlots.LastOrDefault(x => x.Plank != null);
            if (!windowPlank)
                return;


            Plank plank = windowPlank.Plank;
            plank.ReturnToRest();

            FVRPhysicalObject physicalObject = plank.GetComponent<FVRPhysicalObject>();
            physicalObject.IsPickUpLocked = false;

            windowPlank.Plank = null;

            TearPlankAudio.Play();
            PlanksRemain--;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Plank>())
            {
                OnPlankTouch(other.GetComponent<Plank>());
            }
        }
    }
}