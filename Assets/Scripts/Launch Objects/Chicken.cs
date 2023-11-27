using System.Collections;
using Helpers;
using Models;
using PVE;
using UnityEngine;
using VFX;

namespace Launch_Objects
{
    public class Chicken : LaunchObjectBase
    {
        private Side _attackingSide;
        
        private void Start()
        {
            StartByDefaults(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (RandomChance.Chance(effectChance) && other.gameObject.tag is Tags.Player or Tags.Enemy)
            {
                gameObject.transform.SetParent(PveGameManager.Instance.mainCanvas);
                _attackingSide = other.gameObject.tag switch
                {
                    Tags.Enemy => Side.Enemy,
                    Tags.Player => Side.Player,
                    _ => Side.Enemy
                };
                
                
                PveGameManager.Instance.Hit(new HitData
                {
                    damage = impactDamage,
                    toSide = _attackingSide,
                    impactPos = transform.position,
                    impactType = ImpactType.None,
                    Collider2D = other
                });

                var rb = GetComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.mass = 15;
                
                StartCoroutine(RampageMode());
                return;
            }
            
            HitWithoutEffect(other);
        }

        private IEnumerator RampageMode()
        {
            yield return new WaitForSeconds(1f);
            PlaySecondaryAnim();
            PveGameManager.Instance.Hit(new HitData
            {
                damage = (int)(extraDamageAmount * Random.Range(0.25f,1)),
                toSide = _attackingSide,
                impactPos = transform.position,
                impactType = ImpactType.None,
            });
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}
