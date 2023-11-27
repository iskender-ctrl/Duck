using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coffee.UIExtensions;
using Models;
using PVE;
using UnityEngine;
using UnityEngine.UI;
using VFX;
using Random = UnityEngine.Random;

namespace Launch_Objects
{
    public abstract class LaunchObjectBase : MonoBehaviour
    {
        public LaunchObject launchObjectType;
        public TrailType trail;
        public ImpactType impactType;
        public int impactDamage;
        public int effectChance;
        public int effectRemainTime;
        public int extraDamageAmount;
        public float defaultAnimDuration;
        public float specialAnimDuration;
        public Image objectImage;
        public Transform trailPlace;
        public UIParticle uiParticle;
        private LaunchObject _type;
        private LaunchObjectAsset _assets;
        private bool _isDestroyed = false;

        public const int OneSecInMS = 1000;
        public const int TenSecInMS = 10000;
        
        public void StartByDefaults(bool defaultDestroy = true)
        {
            _type = launchObjectType;

            _assets = GameData.Instance.GameAssets.launchObjectAssets.Find(x => x.type == _type);
            
            var trailAsset = GameData.Instance.GameAssets.smokeTrailAssets.Find(x => x.type == trail);
            Instantiate(trailAsset.prefab, trailPlace);
            uiParticle.RefreshParticles();
            PlayDefaultAnim();
            
            if(defaultDestroy)
                DestroyObject(TenSecInMS);
        }

        public void HitWithoutEffect(Collider2D other)
        {
            Side side;
            switch (other.gameObject.tag)
            {
                case Tags.Enemy:
                {
                    side = Side.Enemy;
                    PveGameManager.Instance.Hit(new HitData
                    {
                        damage = (int)(impactDamage * Random.Range(0.25f,1)),
                        toSide = side,
                        impactPos = transform.position,
                        impactType = impactType,
                        Collider2D = other
                    });

                    objectImage.gameObject.SetActive(false);
                    DestroyObject(OneSecInMS);
                    break;
                }
                case Tags.Player:
                {
                    side = Side.Player;
                    PveGameManager.Instance.Hit(new HitData
                    {
                        damage = (int)(impactDamage * Random.Range(0.25f,1)),
                        toSide = side,
                        impactPos = transform.position,
                        impactType = impactType,
                        Collider2D = other
                    });

                    objectImage.gameObject.SetActive(false);
                    DestroyObject(OneSecInMS);
                    break;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            HitWithoutEffect(other);
        }

        public async void DestroyObject(int delayPerMS)
        {
            await Task.Delay(delayPerMS);
            if (_isDestroyed) return;
            _isDestroyed = true;
            Destroy(gameObject);
        }

        private void PlayDefaultAnim()
        {
            StartCoroutine(AnimationRoutine(_assets.primaryAnim, defaultAnimDuration, objectImage));
        }

        public void PlaySecondaryAnim()
        {
            if (_assets.secondaryAnim != null)
                StartCoroutine(AnimationRoutine(_assets.secondaryAnim, specialAnimDuration, objectImage));
        }

        private IEnumerator AnimationRoutine(List<Sprite> sprites, float animDuration, Image imageReference,
            float delayTime = 0, Action oncomplete = null)
        {
            var delayBetweenFrames = animDuration / sprites.Count;
            yield return new WaitForSeconds(delayTime);

            //destroy animation
            foreach (var img in sprites)
            {
                imageReference.sprite = img;
                yield return new WaitForSeconds(delayBetweenFrames);
            }
            
            oncomplete?.Invoke();
        }
    }
}