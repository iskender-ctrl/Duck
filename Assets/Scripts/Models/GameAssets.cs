using System.Collections.Generic;
using Player.CannonControl;
using UnityEngine;

namespace Models
{
    [CreateAssetMenu(fileName = "GameAssets", menuName = "ScriptableObjects/GameAssets", order = 0)]
    public class GameAssets : ScriptableObject
    {
        public List<FortAsset> fortifications;
        public List<CannonAnimAsset> cannon;
        public List<Sprite> cannonMuzzleSmokeEffect;
        public List<LaunchObjectAsset> launchObjectAssets;
        public List<SmokeTrailAsset> smokeTrailAssets;
        public List<ImpactAnimAsset> explosionAnim;
    }
}