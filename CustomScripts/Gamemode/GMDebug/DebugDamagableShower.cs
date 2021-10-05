using FistVR;
using UnityEngine;
using UnityEngine.UI;

namespace CustomScripts.Gamemode.GMDebug
{
    /// <summary>
    /// Script for checking the damage of weapons/bullets
    /// </summary>
    public class DebugDamagableShower : MonoBehaviour, IFVRDamageable
    {
        public Text Text;

        public void Damage(Damage dam)
        {
            Text.text = "Kinetic: " + dam.Dam_TotalKinetic;
        }
    }
}