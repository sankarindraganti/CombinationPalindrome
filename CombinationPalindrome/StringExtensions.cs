#region using
using System.Collections.Generic;
using System.Linq;
#endregion

namespace CombinationPalindrome
{
    public static class StringExtensions
    {
        /// <summary>
        /// IsPalinndrome verifies if a string is palindrom or not.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Returns true if the given string is palindrome else returns false</returns>
        public static bool IsPalindrome(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            int low = 0;
            int high = input.Length - 1;
            while (high > low)
            {
                if (input[low++] != input[high--])
                {
                    return false;
                }
            }
            return true;
        }

        /*
            GetPermutations computes the permutations for a given set of elements.

             How does the implementation work ?

            Let us consider an example {A B C D}  
         
             *               0  ABCD       6 BACD      12 CABD      18 DABC
             *               1  ABDC       7 BADC      13 CADB      19 DACB
             *               2  ACBD       8 BCAD      14 CBAD      20 DBAC
             *               3  ACDB       9 BCDA      15 CBDA      21 DBCA
             *               4  ADBC      10 BDAC      16 CDAB      22 DCAB
             *               5  ADCB      11 BDCA      17 CDBA      23 DCBA

            Note in the above example, the first letter changes after every 6 iterations which is 3!
            and the second letter changes after every 2 iterations which is 2!
            and the third letter changes after every 1 iteration which is 1!

             Based on the above observation, we will create a Factorial array to store the factorial value associated with each position.
             For the above example (a set of 4), our Factorial array will be {0,1,2,6,24}
             We go from 0 to n! (24 in this case) iterations to find the positional changes for each iteration.
             We will create a sequence array that contains n-1 elements as we need to make n-1 moves to generate a new permutation.
             For iteration 19, the value of sequence array will be computed in the below way.

             iterationValue = 19;
             for(int j=0;j<sequence.Length(3);j++)
             {
                int factorialValue = factorial[sequence.Length-j]; // Our factorial array is {0,1,2,6,24}
                //For j=0, factorialValue = 6
                //For j=1, factorialVlaue = 2
                //For j=2, factorialVlaue = 1

                sequence[j] = iterationValue / factorialValue; 
                //For j=0, sequence[0] = 3
                //For j=1, sequence[1] = 0
                //For j=2, sequence[2] = 1

                iterationValue = iterationValue / factorialValue;
                //iterationValue = 1, when j=0
                //iterationValue = 1 , when j=1
                //iterationVlaue = 1, when j=1
             }

            By the end of the above loop, our Sequence array will be {3,0,1}
            Based on the above sequence we move the elements by the above mentioned positions

            A B C D
            Moving the first element by 3 positions will result in {D B C A}
            Moving the second element by 0 positions will result in {D B C A}
            Moving the third element by 1 position will result in {D B A C}

           For a set { A, B, C, D} the 19th iteration will result in {D,B,A,C}    

            Reference:
            https://en.wikipedia.org/wiki/Factorial_number_system
            http://www.keithschwarz.com/interesting/code/?dir=factoradic-permutation
            http://stackoverflow.com/questions/756055/listing-all-permutations-of-a-string-integer          
        */

        /// <summary>
        /// GetPermutations computes and returns a list of permutations of a given input.
        /// </summary>
        /// <param name="inputList">Input set</param>
        /// <returns>Returns a list of permutations for the given input set.</returns>
        public static IEnumerable<IEnumerable<string>> GetPermutations(this IEnumerable<string> inputList)
        {
            var inputArray = inputList.ToArray();
            var factorials = Enumerable.Range(0, inputArray.Length +1)
                                       .Select(Factorial)
                                       .ToArray();

            for (var i = 0L; i < factorials[inputArray.Length]; i++)
            {
                var sequence = GenerateSequence(i, inputArray.Length - 1, factorials);
                yield return GeneratePermutation(inputArray, sequence);
            }
        }

        /// <summary>
        /// GenerateSequence generates an array of integers based on which the elements will be moved.
        /// </summary>
        /// <param name="positionValue">Iteration number</param>
        /// <param name="size">size of the sequence array</param>
        /// <param name="factorials">Array with factorial values</param>
        /// <returns>Returns an array  sequenc</returns>
        private static int[] GenerateSequence(long positionValue, int size, IReadOnlyList<long> factorials)
        {
            var sequence = new int[size];

            for (var j = 0; j < sequence.Length; j++)
            {
                var factorial = factorials[sequence.Length - j];

                sequence[j] = (int)(positionValue / factorial);

                positionValue = (int)(positionValue % factorial);
            }

            return sequence;
        }

        /// <summary>
        /// GeneratePermuatation generates a permutation based on the sequence array
        /// </summary>
        /// <param name="inputArray">Input array</param>
        /// <param name="sequence">Sequence array with index switches for eache element</param>
        /// <returns>Returns a perumted input array</returns>
        private static IEnumerable<string> GeneratePermutation(string[] inputArray, IReadOnlyList<int> sequence)
        {
            var clone = (string[])inputArray.Clone();

            for (int i = 0; i < clone.Length - 1; i++)
            {
                string temp = clone[i];
                clone[i] = clone[i + sequence[i]];
                clone[i + sequence[i]]  = temp;
            }

            return clone;
        }

      /// <summary>
      /// Factorial function computes the factorial value for a given integer
      /// </summary>
      /// <param name="n">An integer value</param>
      /// <returns>Returns the factorial of the given number</returns>
      private static long Factorial(int n)
        {
            long result = 1;

            for (int i = 1; i <= n; i++)
            {
                result = result * i;
            }
            return result;
        }
    }
}
