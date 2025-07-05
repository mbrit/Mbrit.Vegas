

using System.Collections;

namespace Mbrit.Vegas.Utility
{
    internal class Round<T> : IEnumerable<T>
    {
        private List<T> Vectors;

        public Round(List<T> vectors)
        {
            this.Vectors = vectors;
        }

        public IEnumerator<T> GetEnumerator() => this.Vectors.GetEnumerator();

        internal T GetResult(int index) => this.Vectors[index];

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}