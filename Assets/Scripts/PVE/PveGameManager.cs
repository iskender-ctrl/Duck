using System;
using Helpers;
using Player;
using UnityEngine;
using UnityEngine.UI;
using VFX;

namespace PVE
{
    public class PveGameManager : MonoBehaviourSingleton<PveGameManager>
    {
        public PvePlayerData player;
        public PvePlayerData enemy;

        public Transform mainCanvas; 
        
        //for skills
        public float recoveringEnergyValue;
        public float playerEnergy;
        
        //for objects
        public float overloadPoint; //-1 point of overload is subtracted every 3 seconds.
        public float overloadMaxValue;

        public CursorController cursorController; //TODO this one is not well implemented
        
        
        [SerializeField] private Slider playerHpSlider;
        [SerializeField] private Slider enemyHpSlider;

        [SerializeField] private TimerView timerView;
        
        private int _maxHp = 100;
        private DamageTextManager _damageTextManager;


        private void Awake()
        {
            _damageTextManager = GetComponent<DamageTextManager>();
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            player.Health = _maxHp;
            enemy.Health = _maxHp;

            playerHpSlider.minValue = 0;
            playerHpSlider.maxValue = _maxHp;
            UpdateHpUI(Side.Player);
            
            enemyHpSlider.minValue = 0;
            enemyHpSlider.maxValue = _maxHp;
            UpdateHpUI(Side.Enemy);


            player.OnHealthChanged += RepaintPlayerHP;
            enemy.OnHealthChanged += RepaintEnemyHP;
            player.OnHealthChanged += CheckIfSomeoneCouldWin;
            enemy.OnHealthChanged += CheckIfSomeoneCouldWin;
            
            timerView.Elapsed += DecideWhoWinByHP;
        }

        public void Hit(HitData hitData)
        {
            switch (hitData.toSide)
            {
                case Side.Player:
                {
                    player.Health -= hitData.damage ;
                    //UpdateHpUI(Side.Player);
                    player.fortification.GetDamage(hitData.impactPos, hitData.impactType, hitData.effectData);
                    break;
                }
                case Side.Enemy:
                {
                    enemy.Health -= hitData.damage;
                    //UpdateHpUI(Side.Enemy);
                    enemy.fortification.GetDamage(hitData.impactPos, hitData.impactType, hitData.effectData);

                    break;
                }
            }
            _damageTextManager.SpawnText(hitData.Collider2D.bounds,hitData.damage);
            
        }

        private void RepaintPlayerHP() => UpdateHpUI(Side.Player);
        private void RepaintEnemyHP() => UpdateHpUI(Side.Enemy);
        
        private void UpdateHpUI(Side side)
        {
            if (side == Side.Player)
                playerHpSlider.value = player.Health;
            else if (side == Side.Enemy)
                enemyHpSlider.value = enemy.Health;
        }

        #region Win/Lose logic

        private void CheckIfSomeoneCouldWin()
        {
            if(player.Health > 0 && enemy.Health >0) return;
            DecideWhoWinByHP();
        }
        private void DecideWhoWinByHP()
        {
            int winScores = player.Health - enemy.Health;
            OnGameWin(winScores);
        }

        private void OnGameWin(int winScores)
        {
            //simple logic: by scoreA- scoreB we decide if someone win or no one
            Debug.LogError($"{(winScores > 0 ? "Local" : winScores ==0? "No One":"Enemy")} player win!");
            Debug.LogError("Implement Win or loseScreen here");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
#endif
        }
        
        #endregion

    }

    [Serializable]
    public class PvePlayerData
    {
        public event Action OnHealthChanged;
        public int Health
        {
            get => _health;
            set
            {
                OnHealthChanged?.Invoke();
                _health = value;
            }
        }
        [SerializeField] private int _health = 100;
        
        
        public FortificationHolder fortification;
    }

    [Serializable]
    public class HitData
    {
        public int damage;
        public Side toSide;
        public Vector2 impactPos;
        public ImpactType impactType;
        public DamageEffectData effectData;
        public Collider2D Collider2D;
    }

    [Serializable]
    public enum Side
    {
        Player,
        Enemy
    }
}
