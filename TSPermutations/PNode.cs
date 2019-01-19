using System;
using System.Collections.Generic;
using System.Linq;

namespace TSPermutations
{
    class PNode
    {
        public string Permutation { get; set; }

        public int Level { get; set; }

        public PNode[] Descendants { get; set; }

        public PNode(int n)
        {
            Level = 0;
            Permutation = "";
            Descendants = new PNode[n];

            List<int> symbols = Enumerable.Range(1, n).ToList();
            for (int i = 0; i < n; i++)
            {
                Descendants[i] = new PNode(n - 1, this, symbols[i], symbols);
            }
        }

        private PNode(int n, PNode ancestor, int symbol, List<int> symbols)
        {
            Level = ancestor.Level + 1;
            Permutation = ancestor.Permutation + symbol;
            
            List<int> inSymbols = new List<int>(symbols);
            inSymbols.Remove(symbol);

            if (symbols.Count > 0)
            {
                Descendants = new PNode[n];
                for (int i = 0; i < n; i++)
                {
                    Descendants[i] = new PNode(n - 1, this, inSymbols[i], inSymbols);
                }
            }
        }

        public PNode FindByPermutation(string permutation)
        {
            if (Permutation.Equals(permutation))
            {
                return this;
            }
            string prefix = permutation.Substring(0, Level + 1);
            foreach (PNode descendant in Descendants)
            {
                if (descendant.Permutation.Substring(0, Level + 1).Equals(prefix))
                {
                    return descendant.FindByPermutation(permutation);
                }
            }

            return new PNode(0);
        }
    }
}
