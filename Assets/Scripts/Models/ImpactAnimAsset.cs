using System;
using System.Collections.Generic;
using UnityEngine;
using VFX;

namespace Models
{
    [Serializable]
    public class ImpactAnimAsset
    {
        public ImpactType type;
        public List<Sprite> anim;
    }
}