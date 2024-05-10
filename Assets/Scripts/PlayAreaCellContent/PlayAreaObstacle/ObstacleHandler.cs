using System;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle
{

    public class ObstacleHandler : MonoBehaviour, IObstacleHandler
    {

        [SerializeField] private Image _obstacleImage;

        private Obstacle _obstacle;

        public ObstacleStateMachine StateMachine { get => _stateMachine; }
        private ObstacleStateMachine _stateMachine;

        public bool IsProcessingRemoval { get => _isProcessingRemoval; }
        private bool _isProcessingRemoval;

        public static Action<Obstacle> OnPooledObstacleReturn;

        public void RemoveObstacle()
        {
            _obstacle = null;
            _obstacleImage.color = new Color(_obstacleImage.color.r, _obstacleImage.color.g, _obstacleImage.color.b, Statics.ALPHA_OFF);
            _obstacleImage.sprite = null;
        }
        public void SetObstacle(Obstacle obstacle)
        {
            _obstacle = obstacle;
            _obstacleImage.color = new Color(_obstacleImage.color.r, _obstacleImage.color.g, _obstacleImage.color.b, Statics.ALPHA_ON);
            _obstacleImage.sprite = obstacle.Sprite;

            //_stateMachine.Initialize(_stateMachine.IdleState);
        }

        public Obstacle GetObstacle()
        {
            return _obstacle;
        }

        public Image GetImage()
        {
            return _obstacleImage;
        }

        public bool CanDrop()
        {
            return _obstacle.CanDrop;
        }

        public void StartRemoval()
        {
            _isProcessingRemoval = true;
        }
        public bool GetIsProcessingRemoval()
        {
            return _isProcessingRemoval;
        }
        public void FinishRemoval()
        {
            _isProcessingRemoval = false;

            OnPooledObstacleReturn?.Invoke(_obstacle);

            RemoveObstacle();

        }

        public void UpdateStateMachine()
        {
            _stateMachine.Update();
        }

        private void Awake()
        {
            _stateMachine = new ObstacleStateMachine(this);
            _stateMachine.Initialize(_stateMachine.IdleState);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}