using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas
{
    public class PrefixTree
    {
        private class Node
        {
            public Node[] Children = new Node[2]; // 0 = 'w', 1 = 'l'
            public List<int>? Ids;
        }

        private readonly Node _root = new();

        public void Add(string key, int id)
        {
            var current = _root;
            foreach (char c in key)
            {
                int index = c == 'w' ? 0 : 1;
                current.Children[index] ??= new Node();
                current = current.Children[index];
            }

            current.Ids ??= new List<int>();
            current.Ids.Add(id);
        }

        public IEnumerable<int> GetAllDownstreamIds(string prefix)
        {
            var current = _root;
            foreach (char c in prefix)
            {
                int index = c == 'w' ? 0 : 1;
                current = current.Children[index];
                if (current == null)
                    yield break;
            }

            foreach (var id in Traverse(current))
                yield return id;
        }

        private IEnumerable<int> Traverse(Node node)
        {
            if (node.Ids != null)
            {
                foreach (var id in node.Ids)
                    yield return id;
            }

            for (int i = 0; i < 2; i++)
            {
                if (node.Children[i] != null)
                {
                    foreach (var id in Traverse(node.Children[i]))
                        yield return id;
                }
            }
        }
    }
}
