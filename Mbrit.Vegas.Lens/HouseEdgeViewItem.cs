namespace Mbrit.Vegas.Lens
{
    internal class HouseEdgeViewItem
    {
        internal float HouseEdge { get; }

        internal HouseEdgeViewItem(float houseEdge)
        {
            this.HouseEdge = houseEdge;
        }

        public override string ToString() => (this.HouseEdge * 100) + "%";
    }
}