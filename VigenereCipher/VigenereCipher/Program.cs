using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VigenereCipher
{
    internal class Program
    {
        static string alfabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static void Main(string[] args)
        {
            string[] p =
        {
            "FRQSPYMHNJOELKDVAGXTIWBUZC",
                "SWZCINJTELAFQUMKPXDOVBRGHY",
                "CITYWLNZEVOMQGUPJFXBRAHSKD"
                
        };

            Console.Write("Text original: ");
            string txtO = Console.ReadLine();

            string txtC = Cripteaza(txtO, p);
            string txtD = Decripteaza(txtC, p);

            Console.WriteLine("\nPermutări folosite:");
            for (int i = 0; i < p.Length; i++)
                Console.WriteLine($"  P{i + 1}: {p[i]}");

            Console.WriteLine("\nText criptat: " + txtC);
            Console.WriteLine("Text decriptat: " + txtD);
        }
        static string Cripteaza(string text, string[] permutari)
        {
            string rezultat = "";
            text = text.ToUpper();
            int n = permutari.Length;
            int k = 0; // indexul permutării pentru litera curentă

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    int index = alfabet.IndexOf(c);
                    string perm = permutari[k % n];
                    rezultat += perm[index];
                    k++;
                }
                else
                {
                    rezultat += c;
                    // dacă este separator (spațiu, punctuație etc.), resetăm ciclul
                    k = 0;
                }
            }

            return rezultat;
        }
        static string Decripteaza(string text, string[] permutari)
        {
            string rezultat = "";
            text = text.ToUpper();
            int n = permutari.Length;
            int k = 0;

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    string perm = permutari[k % n];
                    int index = perm.IndexOf(c);
                    rezultat += alfabet[index];
                    k++;
                }
                else
                {
                    rezultat += c;
                }
            }

            return rezultat;
        }
    }
}
