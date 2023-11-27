using System;
using Helpers;
using Models;
using Player;
using PVE;
using UnityEngine;
using VFX;

namespace Launch_Objects
{
    public class IceMissile : LaunchObjectBase
    {
        private float _angle;
        [SerializeField] private Rigidbody2D rb;
        
        private void Start()
        {
            StartByDefaults();
        }

        private void Update()
        {
            LookDirection();
        }
        
        public void LookDirection()
        {
            _angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
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
                    damage = impactDamage,
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