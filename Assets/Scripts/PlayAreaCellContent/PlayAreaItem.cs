using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellContent
{
    public class PlayAreaItem : MonoBehaviour
    {
        // Start is called before the first frame update

        public ItemTypes ItemType { get => _type; }
        [SerializeField] private ItemTypes _type;

        public Sprite Sprite { get => _sprite; }
        [SerializeField] private Sprite _sprite;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
