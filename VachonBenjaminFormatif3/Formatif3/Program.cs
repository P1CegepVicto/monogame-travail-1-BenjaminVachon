using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formatif3
{
    class Program
    {
        static void Main(string[] args)
        {
            bool[] Tableau = new bool[100];
            Random rng = new Random();
            string app = "w";
            string touche;
            int Perd = 0;
            int position = 0;
            int essaie = 0;

            for (int i = 0; i <= 99; i++)
            {
                if (rng.Next(0, 2) == 1)
                {
                    Tableau[i] = true;
                }
                if (rng.Next(0, 2) == 2)
                {
                    Tableau[i] = false;
                }
            }
            Tableau[0] = true;
            Tableau[99] = true;

            while (app.ToLower() != "q" || app.ToLower() != "p")
            {
                if (app == "w")
                {
                    Console.WriteLine("pour quitter appuyer sur Q");
                    app = "e";
                }
                Console.WriteLine("Pour vous déplacez appuyer sur: A, S, D, G, H, Y, P");
                touche = Console.ReadLine();
                if(touche.ToLower() == "y")
                {
                    for (int i = position; i <= position + 10; i++)
                    {
                        if (Tableau[i] == false)
                        {
                            Console.Write("[ ]");
                        }
                        if (Tableau[i] == true)
                        {
                            Console.Write("[( )]");
                        }
                    }
                }
                if (touche.ToLower() == "p")
                {
                    
                }
                if (touche.ToLower() == "a")
                {
                    position -= 3;
                }
                if (touche.ToLower() == "s")
                {
                    position -= 2;
                }
                if (touche.ToLower() == "d")
                {
                    position -= 1;
                }
                if (touche.ToLower() == "g")
                {
                    position += 2;
                }
                if (touche.ToLower() == "h")
                {
                    position += 4;
                }
                if (touche.ToLower() == "q")
                {
                    app = "q";
                }
                Console.WriteLine("Vous vous trouvez dans la case " + position);

                if (Tableau[position-1] == false)
                {
                    Perd++;
                }
                else
                {
                    Perd = 0;
                }

                if(Perd == 4)
                {
                    app = "p";
                }
                essaie++;
            }
            if (app == "q")
            {
                Console.WriteLine("Au revoir");
            }   
            if (Perd == 4)
            {
                Console.WriteLine("Impossibilité d'avancé veuillez quitter, vous avez fait " + essaie + " essaies");
            } 
                  
        }
    }
}
