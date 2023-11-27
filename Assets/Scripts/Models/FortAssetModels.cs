using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class FortAsset
    {
        public FortType type;
        public FortIdleSprites idle;
        public FortSpriteList animations;
    }

    [Serializable]
    public class FortIdleSprites
    {
        public Sprite fabric;
        public Sprite tower;
        public Sprite fort;
        public Sprite tools;
    }

    [Serializable]
    public class FortSpriteList
    {
        public List<Sprite> fabric;
        public List<Sprite> tower;
        public List<Sprite> fort;
        public List<Sprite> tools;
    }
}