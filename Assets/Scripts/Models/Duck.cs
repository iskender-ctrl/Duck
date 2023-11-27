using System;
using Player;

namespace Models
{
    [Serializable]
    public class Duck
    {
        public int id;
        public DuckType type;
        public Rarity rarity;
        public DuckAnimController prefab;
    }
}