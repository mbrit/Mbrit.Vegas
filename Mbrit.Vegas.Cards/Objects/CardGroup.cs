using Mbrit.Vegas.Lens.Gdi;

namespace Mbrit.Vegas.Cards
{
    internal class CardGroup
    {
        internal XRectangleF Rectangle { get; }
        internal int StartHand { get; }
        internal int EndHand { get; }

        internal CardGroup(XRectangleF rect, int startHand, int endHand)
        {
            this.Rectangle = rect;
            this.StartHand = startHand;
            this.EndHand = endHand;
        }

        internal int NumRows => (this.EndHand - this.StartHand) + 1;
    }
}