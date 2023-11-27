using Models;
using Player;
using PVE;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Launch_Objects
{
    public class WaterBalloon : LaunchObjectBase
    {
        [SerializeField] private GameObject waterSplashEffect;
        
        private void Start()
        {
            StartByDefaults();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag is Tags.Player or Tags.Enemy)
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
                    impactType = impactType,
                    effectData = new DamageEffectData
                    {
                        type = DamageEffectType.FrozenExtraDamage,
                        ExtraDamageAmount = extraDamageAmount
                    },
                    Collider2D = other
                });
            
                objectImage.gameObject.SetActive(false);
                Instantiate(waterSplashEffect, transform.position,quaternion.identity);
                DestroyObject(OneSecInMS);
            }
        }
    }
}