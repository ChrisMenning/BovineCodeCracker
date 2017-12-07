using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BovineCodeCracker
{
    public class PermutationMaker
    {
        public PermutationMaker()
        { }


        /// <summary>
        /// Generates all permutations for a given code length and depth.
        /// NOTE: This works with integers, not special unicode symbols. Output from this
        /// method must be later converted.
        /// </summary>
        /// <param name="size">How long is the code?</param>
        /// <param name="depth">How many unique symbols in the code?</param>
        /// <returns>A collection of strings.</returns>
        public static IEnumerable<string> Permutations(int size, int depth)
        {
            IntToSymbol its = new IntToSymbol();
            if (size > 0)
            {
                foreach (string s in Permutations(size - 1, depth))
                {
                    string allChars = string.Empty;

                    for (int i = 0; i < depth; i++)
                    {
                        allChars += its.convert(i);
                    }

                    foreach (char c in allChars)
                    {
                        if (!s.Contains(c))
                        {
                            yield return s + c;
                        }
                    }
                }
            }
            else
            {
                yield return string.Empty;
            }
        }
    }
}
