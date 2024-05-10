using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle
{

    public class ObstaclePool : MonoBehaviour
    {

        [Header("Must be at least max PlayAreaSize")]
        [SerializeField] private int _maxPerType = 81;

        [SerializeField] private List<ObstaclePrefabConfig> _obstaclePrefabConfig = new List<ObstaclePrefabConfig>();

        private List<Obstacle> _availableObstalces = new List<Obstacle>();

        public bool IsInitialized { get => _isInitialized; }
        private bool _isInitialized = false;

        internal Obstacle GetNextAvailable(ObstacleTypes type)
        {
            if (_availableObstalces.Count > 0)
            {
                for (int i = 0; i < _availableObstalces.Count; i++)
                {
                    if (_availableObstalces[i].ObstacleType == type)
                    {
                        Obstacle returnObstacle = _availableObstalces[i];
                        _availableObstalces.RemoveAt(i);
                        return returnObstacle;
                    }
                }
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("No more " + type.ToString() + " OBSTACLES in pool");
            return null;
        }

        internal Obstacle GetNextAvailable()
        {
            if (_availableObstalces.Count > 0)
            {
                Obstacle returnObstacle = _availableObstalces[0];
                _availableObstalces.RemoveAt(0);
                return returnObstacle;
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("NO more OBSTACLES in pool");
            return null;
        }

        internal void OnPooledObstacleReturn(Obstacle obstacle)
        {
            PutInPool(obstacle);
        }

        private void InitializePool()
        {
            for (int i = 0; i < _obstaclePrefabConfig.Count; i++)
            {
                for (int j = 0; j < _maxPerType; j++)
                {
                    PutInPool(Instantiate(_obstaclePrefabConfig[i].Prefab) as Obstacle);
                }
            }

            _isInitialized = true;
        }
        private void PutInPool(Obstacle obstacle)
        {
            //block.transform.parent = transform;
            obstacle.transform.SetParent(transform);
            obstacle.transform.localPosition = Statics.Vector3Zero();
            obstacle.gameObject.SetActive(false);

            _availableObstalces.Add(obstacle);
        }


        private void OnDestroy()
        {
            ObstacleHandler.OnPooledObstacleReturn -= OnPooledObstacleReturn;
        }

        private void Awake()
        {
            InitializePool();

            ObstacleHandler.OnPooledObstacleReturn += OnPooledObstacleReturn;
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
            public Obstacle Prefab;
            //public List<AudioClip> AudioClip;
        }
    }

}
