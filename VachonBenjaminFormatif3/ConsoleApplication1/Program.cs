using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] tableau = new int[100];
            int moyenne = 0;
            Random rng = new Random();

            for (int i = 0; i <= 99; i++)
            {
                tableau[i] = rng.Next(-1, 100);
                moyenne += tableau[i];
            }

            moyenne = moyenne / 100;

            for (int i = 0; i <= 99; i++)
            {
                if (tableau[i] > moyenne)
                {
                    Console.WriteLine(tableau[i] + "\n");
                }
            }
            Console.WriteLine("La moyenne est de " + moyenne);
            Console.ReadLine();
        }
    }
}
