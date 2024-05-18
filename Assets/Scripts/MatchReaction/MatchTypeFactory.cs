using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.UI;
using UnityEngine;

namespace MatchThreePrototype.MatchReaction
{
    public class MatchTypeFactory : MonoBehaviour
    {

        ScoreInfoBlock scoreInfo;

        public Match GetNewMatchBase(ItemTypes type)
        {
            switch (type)
            {
                case ItemTypes.WhitePin: return new MatchWhitePin(scoreInfo);
                case ItemTypes.RedPin: return new MatchRedPin(scoreInfo);
                case ItemTypes.GreenPin : return new MatchGreenPin(scoreInfo);
                case ItemTypes.BluePin: return new MatchBluePin(scoreInfo);
                case ItemTypes.PurplePin: return new MatchPurplePin(scoreInfo);
                case ItemTypes.PinkPin: return new MatchPinkPin(scoreInfo);
                case ItemTypes.BlackBall: return new MatchBlackBall(scoreInfo);
                default: return null;
            }
        }

        private void Awake()
        {
            scoreInfo = FindFirstObjectByType<ScoreInfoBlock>();
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
