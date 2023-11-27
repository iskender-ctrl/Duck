using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class LaunchObjectAsset
    {
        public LaunchObject type;
        public List<Sprite> primaryAnim;
        [CanBeNull] public List<Sprite> secondaryAnim;
    }
}