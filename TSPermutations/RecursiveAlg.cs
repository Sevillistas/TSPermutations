using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TSPermutations
{
    class RecursiveAlg
    {
        private static string[] superpermutations = {"1", "121", "123121321", "", "", "", "", "", "", ""};

        private static PNode pTree;

        public static string RunRecursiveAlg(int n)
        {
            Console.WriteLine("Creating permutations tree...");
            pTree = new PNode(n);
            Console.WriteLine("Permutations tree created.");

            Stopwatch timer = new Stopwatch();
            timer.Start();
            string result = FindSuperpermutation(n);
            timer.Stop();
            Console.WriteLine("Superpermutation found. Elapsed time = {0}", timer.Elapsed);
            Console.WriteLine("{0}: " + result, result.Length);

            return result;
        }

        private static string FindSuperpermutation(int n)
        {
            if (superpermutations[n - 1].Equals(""))
            {
                string prevPerm = FindSuperpermutation(n - 1);
                superpermutations[n - 1] = BuildPermFromPrev(prevPerm, n - 1);
            }

            return superpermutations[n - 1];
        }

        private static string BuildPermFromPrev(string prevPerm, int prevN)
        {
            int iterations = prevPerm.Length - prevN + 1;
            //Extract all permutations from (n-1)-supepermutation saving its order.
            List<string> perms = new List<string>();
            for (int i = 0; i < iterations; i++)
            {
                string prefix = prevPerm.Substring(i, prevN);
                if (pTree.FindByPermutation(prefix).Level == prevN)
                {
                    perms.Add(prefix);
                }
            }
            //Console.WriteLine("Permutations found: {0} from {1}.", perms.Count, Program.Factorials[prevN]);

            //make ROL
            StringBuilder expPerms = new StringBuilder();
            foreach (string perm in perms)
            {
                //Computed analitically
                string newShiftedClipedPerm = perm + (prevN + 1) + perm;
                expPerms.Append(newShiftedClipedPerm);
            }

            string result;
            if (prevN > 4)
            {
                string expPermsStr = expPerms.ToString();
                int expPermsStrLen = expPermsStr.Length;
                List<string> parts = new List<string>();
                for (int i = 0; i < Program.PARALLELISM; i++)
                    parts.Add(expPermsStr.Substring(i * expPermsStrLen / Program.PARALLELISM, expPermsStrLen / Program.PARALLELISM));
                var partsRes = parts.AsParallel().Select(x => EliminateOverlaps(x, prevN));
                string strRes = "";
                foreach (string part in partsRes)
                {
                    strRes += part;
                }
                result = EliminateOverlaps(strRes, prevN);
            }
            else
            {
                result = EliminateOverlaps(expPerms.ToString(), prevN);
            }

            return result;
        }

        private static string EliminateOverlaps(string source, int frameLength)
        {
            string trimmed = source;
            for (int i = frameLength; i < trimmed.Length; i++)
            {
                for (int j = 1; j <= frameLength; j++)
                {
                    if (trimmed[i].Equals(trimmed[i - j]))
                    {
                        if (trimmed.Substring(i - j, j).Equals(trimmed.Substring(i, j)))
                        {
                            trimmed = trimmed.Remove(i - j, j);
                            break;
                        }
                    }
                }
            }
            return trimmed;
        }
    }
}