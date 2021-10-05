using FistVR;
using UnityEngine;
using UnityEngine.UI;

namespace CustomScripts
{
    /// <summary>
    /// Script for checking the damage of weapons/bullets
    /// </summary>
    public class DebugDamagableText : MonoBehaviour, IFVRDamageable
    {
        public Text Text1;
        public Text Text2;

        public void Damage(Damage dam)
        {
            Text1.text = "Kinetic: " + dam.Dam_TotalKinetic;
            Text2.text = "Energy: " + dam.Dam_TotalEnergetic;
        }
    }
}