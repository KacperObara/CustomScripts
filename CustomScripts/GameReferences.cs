using UnityEngine;
using WurstMod.MappingComponents.Generic;

namespace CustomScripts
{
    public class GameReferences : MonoBehaviour
    {
        public static GameReferences Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            Player = DebugPlayer;
        }

        [HideInInspector] public Transform Player;
        [SerializeField] private Transform DebugPlayer;

        public CustomScene CustomScene;
        public Transform Respawn;

        private void Start()
        {
#if !UNITY_EDITOR
            Player = FistVR.GM.CurrentPlayerBody.transform;
#endif
        }

        public bool IsPlayerClose(Transform pos, float dist)
        {
            return Vector3.Distance(pos.position, Player.position) <= dist;
        }
    }
}