using System.Collections.Generic;
using UnityEngine;
using WurstMod.MappingComponents.Generic;

namespace CustomScripts
{
    public class GameReferences : MonoBehaviourSingleton<GameReferences>
    {
        public override void Awake()
        {
            base.Awake();

            Player = DebugPlayer;
        }

        public Material CanBuyMat;
        public Material CannotBuyMat;

        public Color CanBuyColor;
        public Color CannotBuyColor;

        public List<Window> Windows;

        [HideInInspector] public Transform Player;
        [SerializeField] private Transform DebugPlayer;

        public CustomScene CustomScene;
        public Transform Respawn;

        private void Start()
        {
#if !UNITY_EDITOR // TODO define directives don't work for me for some reason

            if (FistVR.GM.CurrentPlayerBody != null)
                Player = FistVR.GM.CurrentPlayerBody.transform;
#endif
        }

        public bool IsPlayerClose(Transform pos, float dist)
        {
            return Vector3.Distance(pos.position, Player.position) <= dist;
        }
    }
}