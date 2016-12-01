using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {

            int[] tableau = new int[100];
            int moyenne = 0;
            int plusgrand = 0;
            Random rng = new Random();

            for (int i = 0; i <= 99; i++)
            {
                tableau[i] = rng.Next(-1, 100);
                moyenne += tableau[i];
            }

            moyenne = moyenne / 100;
            Console.WriteLine("Voici votre moyenne " + moyenne);
            for (int i = 0; i <= 99; i++)
            {
                if (tableau[i] > moyenne)
                {
                    plusgrand++;   
                }
            }
            Console.WriteLine("Voici la liste des nombres plus grand que la moyenne ");
            for (int i = 0; i <= 99; i++)
            {               
                if (tableau[i] > moyenne)
                {
                    Console.WriteLine(" la position " + i + " est la position de votre nombre, soit " + tableau[i]);                   
                }
            }
            Console.WriteLine("Il y a " + plusgrand + " nombre plus grand que la moyenne");

            Console.ReadLine();
        }
    }
}
