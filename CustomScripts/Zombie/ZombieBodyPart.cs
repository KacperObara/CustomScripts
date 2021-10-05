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
            if (dam.Dam_TotalKinetic < 20)
                return;

            // TODO may need to rethink explosives, for example grenades hit many times many body parts (I think)
            if (dam.Class == FistVR.Damage.DamageClass.Explosive)
            {
                Controller.OnHit(1);
                return;
            }

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