using FistVR;
using UnityEngine;
using UnityEngine.UI;

namespace CustomScripts
{
    public class DebugDamagableText : MonoBehaviour, IFVRDamageable
    {
        public Text text1;
        public Text text2;

        public void Damage(Damage dam)
        {
            text1.text = "Kinetic: " + dam.Dam_TotalKinetic;
            text2.text = "Energy: " + dam.Dam_TotalEnergetic;
        }
    }
}