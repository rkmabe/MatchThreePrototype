using MatchThreePrototype.MatchReaction.MatchTypes;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using UnityEngine;

namespace MatchThreePrototype.MatchReaction
{
    public class MatchTypeFactory : MonoBehaviour
    {
        public Match GetNewMatchBase(ItemTypes type)
        {
            switch (type)
            {
                case ItemTypes.WhitePin: return new MatchWhitePin();
                case ItemTypes.RedPin: return new MatchRedPin();
                case ItemTypes.GreenPin : return new MatchGreenPin();
                case ItemTypes.BluePin: return new MatchBluePin();
                case ItemTypes.PurplePin: return new MatchPurplePin();
                case ItemTypes.PinkPin: return new MatchPinkPin();
                case ItemTypes.BlackBall: return new MatchBlackBall();
                default: return null;
            }
        }

        private void Awake()
        {

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
