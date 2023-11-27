using System;
using System.Collections.Generic;
using Launch_Objects;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class LaunchObjectTrailAsset
    {
        public TrailType type;
        public List<Sprite> sprites;
    }
}