using System.Collections;

namespace Mbrit.Vegas.Simulator
{
    public struct Round<T> : IEnumerable<T>
    {
        private List<T> Vectors { get; }

        internal Round(IEnumerable<T> vectors)
        {
            this.Vectors = new List<T>(vectors);
        }

        public IEnumerator<T> GetEnumerator() => this.Vectors.GetEnumerator();

        internal T GetResult(int index) => this.Vectors[index];

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}