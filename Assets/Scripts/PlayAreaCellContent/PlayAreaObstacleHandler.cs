using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent
{

    public class PlayAreaObstacleHandler : MonoBehaviour, IPlayAreaObstacleHandler
    {

        [SerializeField] private Image _obstacleImage;

        private PlayAreaObstacle _obstacle;

        public void RemoveObstacle()
        {
            _obstacle = null;
            _obstacleImage.color = new Color(_obstacleImage.color.r, _obstacleImage.color.g, _obstacleImage.color.b, Statics.ALPHA_OFF);
            _obstacleImage.sprite = null;
        }
        public void SetObstacle(PlayAreaObstacle obstacle)
        {
            _obstacle = obstacle;
            _obstacleImage.color = new Color(_obstacleImage.color.r, _obstacleImage.color.g, _obstacleImage.color.b, Statics.ALPHA_ON);
            _obstacleImage.sprite = obstacle.Sprite;
        }

        public PlayAreaObstacle GetObstacle()
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