using UnityEngine;

namespace CustomScripts
{
    public interface IPerkBottle
    {
        void ApplyModifier();
    }

    public interface IPowerUp
    {
        void ApplyModifier();
        void Spawn(Vector3 pos);
    }
}