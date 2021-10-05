using UnityEngine;

namespace CustomScripts
{
    public interface IModifier
    {
        void ApplyModifier();
    }
    
    public interface IPerkBottle : IModifier
    {
    }

    public interface IPowerUp : IModifier
    {
        void Spawn(Vector3 pos);
    }
}