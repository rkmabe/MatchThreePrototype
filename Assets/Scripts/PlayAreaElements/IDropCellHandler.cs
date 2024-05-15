namespace MatchThreePrototype.PlayAreaElements
{

    public interface IDropCellHandler
    {

        public void Setup(PlayArea playArea, PlayAreaColumn column, IRowInfoProvider rowInfoProvider);

        public DropCell FindDropCell(PlayAreaCell cell);
    }

}
