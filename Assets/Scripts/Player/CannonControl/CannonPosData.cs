using System;
using Models;
using UnityEngine;

namespace Player.CannonControl
{
    [Serializable]
    public class CannonPosData
    {
        public FortType type;
        public Vector2 spawnStartPos;
        public Vector2 spawnIncreaseRate;
        public Vector2 smokeStartPos;
        public Vector2 smokeSpawnIncreaseRate;
    }
}
