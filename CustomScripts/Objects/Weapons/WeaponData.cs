using System.Collections.Generic;
using UnityEngine;

namespace CustomScripts.Objects.Weapons
{
    [CreateAssetMenu(fileName = "WeaponData")]
    public class WeaponData : ScriptableObject
    {
        public string Id => DefaultSpawners[0];

        public List<string> DefaultSpawners;
        public List<string> UpgradedSpawners;
        public string UpgradedAmmo;
    }
}