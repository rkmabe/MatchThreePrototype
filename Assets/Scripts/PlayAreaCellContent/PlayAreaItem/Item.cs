using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem
{
    public class Item : MonoBehaviour
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

    public enum ItemTypes
    {
        None = 0,
        WhitePin = 1,
        GreenPin = 2,
        BluePin = 3,
        RedPin = 4,
        PurplePin = 5,
        BlackBall = 6,
        PinkPin = 7,
    }

}
