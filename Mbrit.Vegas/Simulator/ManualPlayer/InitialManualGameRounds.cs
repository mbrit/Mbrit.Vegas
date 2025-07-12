//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Mbrit.Vegas.Simulator
//{
//    public class InitialManualGameRounds : IWinLoseDrawRoundsBucket
//    {
//        public decimal HouseEdge { get; }

//        public InitialManualGameRounds(decimal houseEdge)
//        {
//            this.HouseEdge = houseEdge;
//        }   

//        public IWinLoseDrawRound this[int index] => new ShimRound();

//        public int Count => 1;

//        private class ShimRound : IWinLoseDrawRound
//        {
//            public int Index => 0;

//            public string GetKey() => string.Empty;

//            public WinLoseDrawType GetResult(int hand) => throw new InvalidOperationException("The initial game round does not have any results.");
//        }
//    }
//}
