using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using Player.CannonControl;
using PVE;
using UnityEngine;
using UnityEngine.UI;
using VFX;
using Random = UnityEngine.Random;

namespace Player
{
    public class FortificationHolder : MonoBehaviour
    {
        [SerializeField] private Image bigFabricImg;
        [SerializeField] private Image smallFabricImg;
        [SerializeField] private Image towerImg;
        [SerializeField] private Image fortImg;
        [SerializeField] private Image toolsImg;
        [SerializeField] private Transform duckHolder;
        [SerializeField] private float animDuration;
        [SerializeField] private Cannon cannon;
        [SerializeField] private ImpactEffect impactPrefab;
        [SerializeField] private FreezingEffect freezingEffect;
        
        
        private FortType _type;
        private FortAsset _fortAsset;
        private DuckAnimController _duckAnimController;

        private void Start()
        {//TODO: remove this (testing scripts)
            Set(FortType.Haunted, GameData.Instance.DataHub.allDucks[1]);
        }

        private void Update()
        { //TODO: remove this (testing scripts)
            if (Input.GetKeyDown(KeyCode.S))
            {
                Set(FortType.Stone, GameData.Instance.DataHub.allDucks[2]);
                if(duckHolder.GetChild(0) != null)
                    Destroy(duckHolder.GetChild(0).gameObject);
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                Set(FortType.Haunted, GameData.Instance.DataHub.allDucks[1]);
                if(duckHolder.GetChild(0) != null)
                    Destroy(duckHolder.GetChild(0).gameObject);
            }
        
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartFireAnimation();
            }
        
        }

        public void PlayDuckAnimation(DuckAnim duckAnim)
        {
            _duckAnimController.PlayAnim(duckAnim);
        }

        public void Set(FortType type, Duck duck)
        {
            StopAllCoroutines();
            
            cannon.Set(type);
            _type = type;
            _fortAsset = GameData.Instance.GameAssets.fortifications.Find(x => x.type == type);
            
            bigFabricImg.gameObject.SetActive(false);
            smallFabricImg.gameObject.SetActive(false);
            switch (type)
            {
                case FortType.Haunted:
                    bigFabricImg.gameObject.SetActive(true);
                    break;
                case FortType.Stone:
                    smallFabricImg.gameObject.SetActive(true);
                    break;
            }
            
            SetIdleSprites();
            var duckPrefab = GameData.Instance.DataHub.allDucks.Find(x => x.rarity == duck.rarity && x.type == duck.type).prefab;
            _duckAnimController = Instantiate(duckPrefab, duckHolder);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void SetIdleSprites()
        {
            bigFabricImg.sprite = _fortAsset.idle.fabric;
            smallFabricImg.sprite = _fortAsset.idle.fabric;
            towerImg.sprite = _fortAsset.idle.tower;
            fortImg.sprite = _fortAsset.idle.fort;
            toolsImg.sprite = _fortAsset.idle.tools;
        }

        public void StartFireAnimation()
        {
            StartCoroutine(bigFabricImg.gameObject.activeInHierarchy
                ? LoopAnimate(_fortAsset.animations.fabric, bigFabricImg)
                : LoopAnimate(_fortAsset.animations.fabric, smallFabricImg));

            StartCoroutine(LoopAnimate(_fortAsset.animations.tower, towerImg));
            //TODO: StartCoroutine(LoopAnimate(_fortAsset.animations.fort, fortImg));
            StartCoroutine(LoopAnimate(_fortAsset.animations.tools, toolsImg));
        }
        
        private IEnumerator LoopAnimate(List<Sprite> sprites, Image imgReference)
        {
            if(sprites == null || sprites.Count == 0)
                yield break;
            
            var delay = animDuration / sprites.Count;
            
            //destroy animation
            foreach (var img in sprites)
            {
                imgReference.sprite = img;
                yield return new WaitForSeconds(delay);
            }
            
            //firing idle animation loop
            while (true)
            {
                for (var i = 90; i < sprites.Count-1; i++)
                {
                    imgReference.sprite = sprites[i];
                    yield return new WaitForSeconds(delay);
                }
            }
        }

        public void GetDamage(Vector2 impactPos, ImpactType impactType, DamageEffectData effectData = null)
        {
            _duckAnimController.PlayAnim(DuckAnim.Angry);
            var impactEffect = Instantiate(impactPrefab, transform);
            impactEffect.transform.position = impactPos;
            impactEffect.Play(impactType);

            if (effectData == null) return;
            switch (effectData.type)
            {
                default:
                case DamageEffectType.None:
                    break;
                case DamageEffectType.Freeze:
                {
                    if (cannon.isFrozen) return; // if already frozen
                    var vfx = Instantiate(freezingEffect, transform);
                    vfx.transform.position = gameObject.transform.position;
                    vfx.Play(effectData.time);
                    
                    StartCoroutine(FreezeRoutine(effectData.time));
                    break;
                }
                case DamageEffectType.FrozenExtraDamage:
                {
                    if (cannon.isFrozen)
                    {
                        var side = gameObject.tag is not Tags.Enemy ? Side.Player : Side.Enemy;
                        PveGameManager.Instance.Hit(new HitData
                        {
                            damage = (int)(effectData.ExtraDamageAmount.Value * Random.Range(0.25f,1)),
                            toSide = side,
                        });
                    }
                    break;
                }
            }
        }

        private IEnumerator FreezeRoutine(float time)
        {
            cannon.isFrozen = true;
            _duckAnimController.isFrozen = true;
            yield return new WaitForSeconds(time);
            cannon.isFrozen = false;
            _duckAnimController.isFrozen = false;
        }
    }

    [Serializable]
    public class DamageEffectData
    {
        public DamageEffectType type;
        public float time;
        public int? ExtraDamageAmount;
    }

    [Serializable]
    public enum DamageEffectType
    {
        None = 0,
        Freeze = 1,
        FrozenExtraDamage = 2,
        
        
    }
}
