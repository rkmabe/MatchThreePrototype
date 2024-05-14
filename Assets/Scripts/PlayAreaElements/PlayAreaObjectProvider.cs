using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public class PlayAreaObjectProvider : MonoBehaviour, IPlayAreaObjectProvider
    {

        public PlayAreaCell GetPlayAreaCell(PlayAreaColumn column, int cellNum)
        {
            for (int i = 0; i < column.Cells.Count; i++)
            {
                if (column.Cells[i].Number == cellNum)
                {
                    return column.Cells[i];
                }
            }

            return null;
        }

        public PlayAreaColumn GetPlayAreaColumn(List<PlayAreaColumn> columns, int columnNum)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].Number == columnNum)
                {
                    return columns[i];
                }
            }

            return null;
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
