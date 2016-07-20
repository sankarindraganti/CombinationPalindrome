#region using
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace CombinationPalindrome
{
    public class CombinationsAndPalindrome
    {
        private List<string> Combinations = new List<string>();

        /// <summary>
        /// GenerateCombinations generates combinations of specified length from a given input set.
        /// </summary>
        /// <param name="input">input list</param>
        /// <param name="startPos">start position</param>
        /// <param name="length">Length of the subset</param>
        /// <returns>Returns a list of subsets of specified length</returns>
        public List<string> GenerateCombinations(List<string> input, int startPos, int length)
        {
            Combinations.Clear();

            if(input==null)
            {
                throw new ArgumentNullException("Input cannot be null");
            }

            if(startPos < 0 || length < 0)
            {
                throw new ArgumentException("Please provide a valid startPos and length");
            }

            if(input.Count() ==0)
            {
                return null;
            }


            this.GenerateCombiations(input.ToArray(), startPos, length, new string[input.Count]);

            return this.Combinations;
        }

        /// <summary>
        /// GenerateCombinations recursively generates unique combination set of specified length
        /// </summary>
        /// <param name="input">input list</param>
        /// <param name="startPos">Start position</param>
        /// <param name="length">Length of the subset</param>
        /// <param name="result">Temporary array to store the subset</param>
        private void GenerateCombiations(string[] input, int startPos, int length, string[] result)
        {
            if (length == 0)
            {
                this.Combinations.Add(string.Join(" ", result).TrimStart(' '));
            }
            else
            {
                for (int i = startPos; i < input.Length; i++)
                {
                    result[input.Length - length] = input[i];
                    GenerateCombiations(input, i + 1, length - 1, result);
                }
            }
        }
    }
}
