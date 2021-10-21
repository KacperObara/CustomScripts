using UnityEngine;

namespace CustomScripts.Zombie
{
    public abstract class ZombieController : MonoBehaviour
    {
        [HideInInspector] public Window LastInteractedWindow;

        public abstract void Initialize();
        public abstract void OnHit(float damage);
        public abstract void OnHitPlayer();
    }
}