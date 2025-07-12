namespace Mbrit.Vegas.Simulator
{
    public class PlayerPlayMove : PlayerMove
    {
        public int Units { get; }

        internal PlayerPlayMove(int units)
        {
            this.Units = units;
        }
    }
}