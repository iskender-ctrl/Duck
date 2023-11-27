using Helpers;
using UnityEngine;

namespace Models
{
    public class GameData : MonoBehaviourSingleton<GameData>
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private DataHub _dataHub;

        public DataHub DataHub
        {
            get
            {
                if (_dataHub == null)
                {
                    _dataHub = Resources.Load<DataHub>("DataHub");
                }

                return _dataHub;
            }
        }

        private GameAssets _gameAssets;
        
        public GameAssets GameAssets
        {
            get
            {
                if (_gameAssets == null)
                {
                    _gameAssets = Resources.Load<GameAssets>("GameAssets");
                }

                return _gameAssets;
            }
        }
    }
}