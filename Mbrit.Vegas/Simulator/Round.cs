using System.Collections;
using System.Text;

namespace Mbrit.Vegas.Simulator
{
    public class Round<T> : IEnumerable<T>
    {
        public int Index { get; }
        private List<T> Vectors { get; }

        internal Round(int index, IEnumerable<T> vectors)
        {
            this.Index = index;
            this.Vectors = new List<T>(vectors);
        }

        public int Count => this.Vectors.Count;

        public IEnumerator<T> GetEnumerator() => this.Vectors.GetEnumerator();

        internal T GetResult(int index) => this.Vectors[index];

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public string GetKey()
        {
            var wlds = new List<WinLoseDrawType>();

            var count = this.Count;
            for (var index = 0; index < count; index++)
            {
                var result = this.Vectors[index];

                if (result is WinLoseDrawType)
                {
                    var wld = (WinLoseDrawType)(object)result;
                    wlds.Add(wld);
                }
                else
                    throw new NotSupportedException($"Cannot handle '{result}'.");
            }

            return wlds.GetKey();
        }
    }
}