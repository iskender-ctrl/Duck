using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using PVE;
using UnityEngine;
using UnityEngine.UI;

namespace Player.CannonControl
{
    public class Cannon : MonoBehaviour
    {
        public Transform smokeSpawn, shotPoint;

        public LimitationsClickDetector cannonLimitations;
        
        public Image cannonHead;
        public Image cannonStand;
        public List<Sprite> headSprites;
        public List<Sprite> standSprites;

        public bool isFrozen = false;
        
        private int _lastIndex = 0;
        private int _changeSpriteSpeed;

        [SerializeField] private float force;
        [SerializeField] private FortificationHolder fortInstance;
        
        private float _angleDeg;
        private float _limitedAngleDeg;
        private float _angleRad;
        private float _distance;

        private bool _moving = false;

        // [SerializeField] private Vector2 objectSpawnStartPoint;
        // [SerializeField] private Vector2 objectSpawnRateOfIncrease;
        // [SerializeField] private Vector2 smokeSpawnStartPoint;
        // [SerializeField] private Vector2 smokeSpawnRateOfIncrease;
        private CannonPosData _posData;
        private Vector3 _touchScreenPosition;
        private Vector3 _screenPosition;
        private Vector3 _lookAt;
        private Vector3 _velocity;

        [SerializeField] private TrajectoryPredictor trajectoryPredictor;
        [SerializeField] private Rigidbody2D bullet;

        private FortType _fortType;
        [SerializeField] private List<CannonObjData> cannonOjbData;
        [SerializeField] private CannonSmokeEffect smokeEffect;
        [SerializeField] private float maximumAngleDeg = 17;


        private bool shootRequested;
        private void OnEnable()
        {
            cannonLimitations.gameObject.SetActive(true);
            cannonLimitations.onClicked += SetShootRequested;
        }

        private void SetShootRequested() => shootRequested = true;

        private void OnDisable()
        {
            cannonLimitations.gameObject.SetActive(false);
            cannonLimitations.onClicked -= SetShootRequested;
        }

        public void Set(FortType type)
        {
            _fortType = type;
            cannonOjbData.ForEach(cannonData =>
            {
                cannonData.gameObjects.ForEach(x=>x.gameObject.SetActive(false));
            });

            var cannonData = cannonOjbData.Find(x => x.type == type);
            cannonData.gameObjects.ForEach(x=>x.gameObject.SetActive(true));
            var animAsset = GameData.Instance.GameAssets.cannon.Find(x => x.type == type);
            headSprites = animAsset.heads;
            cannonHead.sprite = animAsset.heads[0];
            standSprites = animAsset.stands;
            cannonStand.sprite = animAsset.stands[0];
            _posData = GameData.Instance.DataHub.canonPositionsData.Find(x => x.type == type);
        }

        public void Shoot()
        {
            //Spawning launch object
            print("mouse 0 clicked!");
            fortInstance.PlayDuckAnimation(DuckAnim.Angry);
            var instantiatedBullet = Instantiate(bullet, transform);
            var bulletTransform = instantiatedBullet.transform;
            bulletTransform.position = shotPoint.position;
            bulletTransform.rotation = shotPoint.rotation;
            instantiatedBullet.velocity = _velocity;
            
            //playing vfx
            var smoke = Instantiate(smokeEffect, smokeSpawn);
            smoke.transform.SetParent(transform);   
        }

        private void Update()
        {
            if(isFrozen)
                return;
            
            GetScreenPosition();
            GetAngleDeg();

            if (!_moving)
                StartCoroutine(ChangeSprite(_lastIndex, (int) Mathf.Round(Mathf.Clamp(_limitedAngleDeg, 0, maximumAngleDeg))));
            
            if (_angleDeg <= maximumAngleDeg && _angleRad >= 0)
            {
                SetSpawnPoints();
                
                PveGameManager.Instance.cursorController.SetTrue();
                
                if (shootRequested)
                {
                    shootRequested = false;
                    Shoot();
                }
            }
            else
            {
                PveGameManager.Instance.cursorController.SetWrong();
            }

        }

        private void FixedUpdate()
        {
            if(isFrozen)
                return;

            var shotPos = shotPoint.position;
            _distance = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                shotPos);
            var timeBetweenObjects = _distance /  force;

            _velocity = CalculateVelocity(Camera.main.ScreenToWorldPoint(Input.mousePosition), shotPos,
                timeBetweenObjects);
            trajectoryPredictor.debugLineDuration = Time.unscaledDeltaTime;
            trajectoryPredictor.Predict2D(shotPos, new Vector2(_velocity.x, _velocity.y), Physics2D.gravity / 2);
        }

        public void SetSpawnPoints()
        {
            smokeSpawn.transform.localPosition =
                new Vector3( _posData.smokeStartPos.x + _posData.smokeSpawnIncreaseRate.x * Mathf.Round(_limitedAngleDeg),
                    _posData.smokeStartPos.y + _posData.smokeSpawnIncreaseRate.y * Mathf.Round(_limitedAngleDeg), 0);
            smokeSpawn.transform.localRotation = Quaternion.Euler(0, 0, 0.694f * Mathf.Round(_limitedAngleDeg));

            shotPoint.transform.localPosition = new Vector3(
                _posData.spawnStartPos.x - _posData.spawnIncreaseRate.x * Mathf.Round(_limitedAngleDeg),
                _posData.spawnStartPos.y + _posData.spawnIncreaseRate.y * Mathf.Round(_limitedAngleDeg), 0);
        }

        public void GetScreenPosition()
        {
            // if (trajectoryPredictor.predictionPoints.Count == 0 || trajectoryPredictor.debugLine == null) return;
            _screenPosition = trajectoryPredictor.predictionPoints[trajectoryPredictor.predictionPoints.Count / 2];
        }

        public void GetAngleDeg()
        {
            _lookAt = _screenPosition;
            _angleRad = Mathf.Atan2(_lookAt.y - transform.position.y, _lookAt.x - transform.position.x);
            _angleDeg = (180 / Mathf.PI) * _angleRad;

            if (_angleDeg > 90)
            {
                _angleDeg = 180 - _angleDeg;
            }

            _limitedAngleDeg = Mathf.Clamp(_angleDeg, 0, 80f);
        }
        
        IEnumerator ChangeSprite(int last, int angleDeg)
        {
            _moving = true;
            _changeSpriteSpeed = 2;

            if (Mathf.Abs(last - angleDeg) <= 10 && Mathf.Abs(last - angleDeg) >= 2)
            {
                _changeSpriteSpeed = 5;
            }
            else if (Mathf.Abs(last - angleDeg) <= 20)
            {
                _changeSpriteSpeed = 4;
            }
            else if (Mathf.Abs(last - angleDeg) <= 28)
            {
                _changeSpriteSpeed = 3;
            }

            if (last < angleDeg)
            {
                for (int i = last; i <= angleDeg; i++)
                {
                    yield return new WaitForSeconds(.0025f * _changeSpriteSpeed);
                    if (headSprites.Count > i)
                    {
                        cannonHead.sprite = headSprites[i];
                        if (cannonStand != null)
                        {
                            if(_fortType != FortType.Haunted)
                                cannonStand.sprite = standSprites[i];
                        }

                        _lastIndex = i;
                    }
                }
            }
            else
            {
                for (int i = last; i >= angleDeg; i--)
                {
                    yield return new WaitForSeconds(.0025f * _changeSpriteSpeed);
                    if (headSprites.Count > i)
                    {
                        cannonHead.sprite = headSprites[i];
                        if (cannonStand != null)
                        {
                            if(_fortType != FortType.Haunted)
                                cannonStand.sprite = standSprites[i];
                        }

                        _lastIndex = i;
                    }
                }
            }

            _moving = false;
        }

        private Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
        {
            //define the distance x and y first
            Vector3 distance = target - origin;
            Vector3 distance_x_z = distance;
            distance_x_z.Normalize();
            distance_x_z.y = 0;

            //creating a float that represents our distance 
            float sy = distance.y;
            float sxz = distance.magnitude;

            //calculating initial x velocity
            //Vx = x / t
            float Vxz = sxz / time;

            ////calculating initial y velocity
            //Vy0 = y/t + 1/2 * g * t
            float Vy = sy / time + 0.5f * Mathf.Abs(Physics.gravity.y / 2) * time;

            Vector3 result = distance_x_z * Vxz;
            result.y = Vy;

            return result;
        }
    }
}