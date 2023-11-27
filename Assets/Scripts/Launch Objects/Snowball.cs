using Helpers;
using Models;
using Player;
using PVE;
using UnityEngine;
using VFX;

namespace Launch_Objects
{
    public class Snowball : LaunchObjectBase
    {
        private void Start()
        {
            StartByDefaults();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (RandomChance.Chance(effectChance) && other.gameObject.tag is Tags.Player or Tags.Enemy)
            {
                var side = other.gameObject.tag switch
                {
                    Tags.Enemy => Side.Enemy,
                    Tags.Player => Side.Player,
                    _ => Side.Enemy
                };

                PveGameManager.Instance.Hit(new HitData
                {
                    damage = (int)(impactDamage * Random.Range(0.25f,1)),
                    toSide = side,
                    impactPos = transform.position,
                    impactType = ImpactType.None,
                    effectData = new DamageEffectData
                    {
                        type = DamageEffectType.Freeze,
                        time = effectRemainTime
                    },
                    Collider2D = other
                });
            
                DestroyObject(OneSecInMS);
                return;
            }
            
            HitWithoutEffect(other);
        }
    }
}
