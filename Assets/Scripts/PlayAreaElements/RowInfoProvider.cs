using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public class RowInfoProvider : MonoBehaviour, IRowInfoProvider
    {

        private List<PlayAreaRowInfo> _rowInfo = new List<PlayAreaRowInfo>();

        public PlayAreaRowInfo GetRowInfo(int rowNum)
        {
            for (int i = 0; i < _rowInfo.Count; i++)
            {
                if (_rowInfo[i].RowNum == rowNum)
                {
                    return _rowInfo[i];
                }
            }

            Debug.LogWarning("RowInfo not found for " + rowNum);
            return default(PlayAreaRowInfo);
        }

        public void SetupRowInfo(List<PlayAreaCell> cells)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                RectTransform rect = cells[i].RectTransform;
                PlayAreaRowInfo row = new PlayAreaRowInfo();
                row.RowNum = cells[i].Number;
                row.MinY = cells[i].RectTransform.anchorMin.y;
                row.MaxY = cells[i].RectTransform.anchorMax.y;

                _rowInfo.Add(row);
            }
        }
    }

    public struct PlayAreaRowInfo
    {
        public int RowNum;
        public float MinY;
        public float MaxY;
    }
}