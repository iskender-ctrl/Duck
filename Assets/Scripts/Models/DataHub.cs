using System;
using System.Collections.Generic;
using Player.CannonControl;
using UnityEngine;

namespace Models
{
    [CreateAssetMenu(fileName = "DataHub", menuName = "ScriptableObjects/DataHub", order = 0)]
    public class DataHub : ScriptableObject
    {
        public List<Duck> allDucks;
        public List<CannonPosData> canonPositionsData;

        public PveGameData pveData;

        

        public Duck GetDuck(DuckType type, Rarity rarity)
        {
            var duck = allDucks.Find(x => x.type == type && x.rarity == rarity);
            if (duck != null)
                return duck;
            
            Debug.LogError($"{rarity.ToString()} {type.ToString()} is null!");
            return null;
        }
    }
    
    [Serializable]
    public class PveGameData
    {
        public int playerHealth = 100;
        public int enemyHealth = 100;
        

    }
}