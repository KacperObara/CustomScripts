using FistVR;
using UnityEngine;

namespace CustomScripts
{
    public class ZombieBodyPart : MonoBehaviour, IFVRDamageable
    {
        public int PartDamageMultiplier = 1;
        public ZombieController Controller;

        public void Damage(Damage dam)
        {
            int damage = 0;

            if (dam.Dam_TotalKinetic < 1000)
                damage = 1;
            else if (dam.Dam_TotalKinetic < 2000)
                damage = 2;
            else if (dam.Dam_TotalKinetic >= 2000)
                damage = 3;

            damage *= PartDamageMultiplier;

            Controller.OnHit(damage);
        }
    }
}