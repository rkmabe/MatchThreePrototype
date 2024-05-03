using UnityEngine;

namespace MatchThreePrototype
{

    public class PlayAreaObstacle : MonoBehaviour
    {

        public bool CanDrop { get => _canDrop; }
        [SerializeField] private bool _canDrop = false;

        public ObstacleTypes ObstacleType { get => _type; }
        [SerializeField] private ObstacleTypes _type;

        public Sprite Sprite { get => _sprite; }
        [SerializeField] private Sprite _sprite;

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