using System.Collections.Generic;
using UnityEngine;


namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock
{

    public class Block : MonoBehaviour
    {

        public BlockTypes BlockType { get => _type; }
        [SerializeField] private BlockTypes _type;


        // we start with current block level.
        // if there are any _lowerBlockLevels, we move to the next lowest when the current is eliminated
        // if there are no lower block levels, the entire block is eliminated
        // when the last level is removed, the item underneath should be allowed to match
        //public List<BlockLevel> LowerBlockLevels { get => _lowerBlockLevels; }
        [SerializeField] private List<BlockLevel> _lowerBlockLevels;
        [SerializeField] private BlockLevel _currBlockLevel;

        // cache the abovev data on awake.
        // so that teh block can be restored to default state before returning to object pool.
        private List<BlockLevel> _lowerBlockLevelsCache = new List<BlockLevel>();
        private BlockLevel _currBlockLevelCache;


        internal Sprite CurrentSprite { get => _currBlockLevel.Sprite; }


        internal void RemoveLevel(out bool allLevelsRemoved)
        {
            if (_lowerBlockLevels.Count > 0)
            {
                _currBlockLevel = _lowerBlockLevels[0];
                _lowerBlockLevels.RemoveAt(0);
                allLevelsRemoved = false;
            }
            else
            {
                allLevelsRemoved = true;
            }
        }

        internal void RestoreFromCache()
        {
            _lowerBlockLevels.AddRange(_lowerBlockLevelsCache);
            _currBlockLevel = _currBlockLevelCache;
        }

        private void Awake()
        {
            _lowerBlockLevelsCache.AddRange(_lowerBlockLevels);
            _currBlockLevelCache = _currBlockLevel;
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
        public struct BlockLevel
        {
            public int Num;
            public Sprite Sprite;
        }

    }

    public enum BlockTypes
    {
        None = 0,
        PlateLight = 1,
        PlateMedium = 2,
        PlateHeavy = 3,
    }

}