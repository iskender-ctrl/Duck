using System;
using Launch_Objects;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class SmokeTrailAsset
    {
        public TrailType type;
        public GameObject prefab;
    }
    
    [Serializable]
    public enum TrailType
    {
        Normal,
        RocketSmoke,
        Freezing,
    }
}