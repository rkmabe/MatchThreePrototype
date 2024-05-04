using System.Collections.Generic;
using UnityEngine;
using MatchThreePrototype.PlayAreaCellContent;

namespace MatchThreePrototype
{

    public class ObstaclePool : MonoBehaviour
    {

        [Header("Must be at least max PlayAreaSize")]
        [SerializeField] private int _maxPerType = 81;

        [SerializeField] private List<ObstaclePrefabConfig> _obstaclePrefabConfig = new List<ObstaclePrefabConfig>();

        private List<PlayAreaObstacle> _availableObstalces = new List<PlayAreaObstacle>();

        public bool IsInitialized { get => _isInitialized; }
        private bool _isInitialized = false;

        internal PlayAreaObstacle GetNextAvailable(ObstacleTypes type)
        {
            if (_availableObstalces.Count > 0)
            {
                for (int i = 0; i < _availableObstalces.Count; i++)
                {
                    if (_availableObstalces[i].ObstacleType == type)
                    {
                        PlayAreaObstacle returnObstacle = _availableObstalces[i];
                        _availableObstalces.RemoveAt(i);
                        return returnObstacle;
                    }
                }
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("No more " + type.ToString() + " OBSTACLES in pool");
            return null;
        }

        internal PlayAreaObstacle GetNextAvailable()
        {
            if (_availableObstalces.Count > 0)
            {
                PlayAreaObstacle returnObstacle = _availableObstalces[0];
                _availableObstalces.RemoveAt(0);
                return returnObstacle;
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("NO more OBSTACLES in pool");
            return null;
        }

        internal void Return(PlayAreaObstacle obstacle)
        {
            PutInPool(obstacle);
        }

        private void InitializePool()
        {
            for (int i = 0; i < _obstaclePrefabConfig.Count; i++)
            {
                for (int j = 0; j < _maxPerType; j++)
                {
                    PutInPool(Instantiate(_obstaclePrefabConfig[i].Prefab) as PlayAreaObstacle);
                }
            }

            _isInitialized = true;
        }
        private void PutInPool(PlayAreaObstacle obstacle)
        {
            //block.transform.parent = transform;
            obstacle.transform.SetParent(transform);
            obstacle.transform.localPosition = Statics.Vector3Zero();
            obstacle.gameObject.SetActive(false);

            _availableObstalces.Add(obstacle);
        }


        private void Awake()
        {
            InitializePool();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        [System.Serializable]
        public struct ObstaclePrefabConfig
        {
            public ObstacleTypes Type;
            public PlayAreaObstacle Prefab;
            //public List<AudioClip> AudioClip;
        }
    }
    public enum ObstacleTypes
    {
        None = 0,
        RedWall = 1,
        GreyWall = 2,
    }
}
