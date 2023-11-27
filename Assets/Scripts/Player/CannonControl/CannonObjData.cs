using System;
using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Player.CannonControl
{
    [Serializable]
    public class CannonObjData
    {
        public List<GameObject> gameObjects;
        public FortType type;
    }

    [Serializable]
    public class CannonAnimAsset
    {
        public FortType type;
        public List<Sprite> heads;
        public List<Sprite> stands;
    }
}