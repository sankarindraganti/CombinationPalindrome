#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

/*
Five Dwarves ( Gimli Fili Ilif Ilmig and Mark) met at the Prancing Pony and played a word game to determine which combinations of their names resulted in a palindrome. 
Write a program in that prints out all of those combinations.

Solution: 
    We use nCr = n!/(r!)*(n-r)! to find the different combinations for the given set.
    For each combination obtained from the above step, we will find the n! permutations. 

    For set { "Gimli", "Fili", "Ilif", "Ilmig", "Mark" } , below will be the 3 element sets
    Applying the Formula nCr we will have 10 sets.

                        Gimli Fili Ilif
                        Gimli Fili Ilmig
                        Gimli Fili Mark
                        Gimli Ilif Ilmig
                        Gimli Ilif Mark
                        Gimli Ilmig Mark
                        Fili  Ilif Ilmig
                        Fili  Ilif Mark
                        Fili  Ilmig Mark
                        Ilif Ilmig Mark

    We will find permutations for each of the above combinations. For Combination Gimli Fili Ilif, there will be 6 (3!) permutations.
    
                        Gimli Fili Ilif
                        Gimli Ilif Fili
                        Fili Gimli Ilif
                        Fili Ilif Gimli
                        Ilif Gmili Fili
                        Ilif Fili Gimli
     We join the each permuted set using string.join and neutralize the casing of the characters and will verify if the permuted string is palindrome or not.
*/
namespace CombinationPalindrome
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var inputString = new List<string>() { "Gimli", "Fili", "Ilif", "Ilmig", "Mark" };
                //var inputString = new string[] { "abb", "bba" , "ccc" };
                
                List<string> results = new List<string>();

                var tasks = new List<Task>();

                Console.WriteLine("Started Processing...");

                var parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 10 };

                //Generate combinations for the given input set.
                // We start from length of the subset 2 as starting from 1 means finding permutations of an individuals name.
                for (int i = 2; i <=inputString.Count(); i++)
                {
                    var combinationsAndPalindrome = new CombinationsAndPalindrome();
                    results.AddRange(combinationsAndPalindrome.GenerateCombinations(inputString.ToList(), 0, i));
                }

                //Generate permutation for each of the above generated set and verify if the resulted string is a palindrome or not.
                Parallel.ForEach(results, parallelOptions, (result) => tasks.Add(Task.Factory.StartNew(() =>
                                                      {
                                                          foreach (var permutedString in result.Split(' ').AsEnumerable<string>().GetPermutations())
                                                          {
                                                              if (string.Join("", permutedString).ToLower().IsPalindrome())
                                                              {
                                                                  Console.WriteLine(string.Format("Elements : {0} \t and the palindrome string is {1}",
                                                                                                                                      string.Join(" ", permutedString),
                                                                                                                                      string.Join("", permutedString).ToLower()));
                                                              }
                                                          }
                                                      })));

                Task.WaitAll(tasks.ToArray());

                Console.WriteLine("Completed Processing..");
            }
            catch(AggregateException aggregateException)
            {
                aggregateException.Handle(exception => {
                        if(exception is ArgumentException)
                        {
                            Console.WriteLine("Something wrong with one of the inputs \n"+ exception.Message);
                            return true;
                        }
                        else if(exception is OutOfMemoryException)
                        {
                            Console.WriteLine("No more memory left to compute permutations and combinations. Try reducing the input size.");
                            return true;
                        }
                            return false;
                        });
            }
           catch(Exception exception)
            {
                Console.WriteLine("Something went wrong. Here ae the details " + exception.Message);
            }
            Console.Read();
        }
    }
}
