using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherN
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Introduceti textul:");
            string text = Console.ReadLine();

            Console.WriteLine("Introduceti cheia n (0..25):");
            int n = int.Parse(Console.ReadLine());

            // Criptare
            string criptat = Criptare(text, n);
            Console.WriteLine("\nText criptat:");
            Console.WriteLine(criptat);

            // Decriptare
            string decriptat = Decriptare(criptat, n);
            Console.WriteLine("\nText decriptat:");
            Console.WriteLine(decriptat);

            // Criptanaliza
            Criptanaliza(criptat);
        }
        static string CifruN(string text, int n)
        {
            char[] rezultat = text.ToCharArray();

            for (int i = 0; i < rezultat.Length; i++)
            {
                char c = rezultat[i];

                if (char.IsUpper(c)) // litere mari
                {
                    rezultat[i] = (char)('A' + (c - 'A' + n + 26) % 26);
                }
                else if (char.IsLower(c)) // litere mici
                {
                    rezultat[i] = (char)('a' + (c - 'a' + n + 26) % 26);
                }
                // alte caractere nu se modifica
            }

            return new string(rezultat);
        }
        static string Criptare(string text, int n)
        {
            return CifruN(text, n);
        }

        // Decriptare -n
        static string Decriptare(string text, int n)
        {
            return CifruN(text, -n);
        }

        // Criptanaliza: incearca toate cheile posibile
        static void Criptanaliza(string text)
        {
            Console.WriteLine("\nCriptanaliza:");
            for (int n = 1; n <= 25; n++)
            {
                Console.WriteLine("Cheia " + n + ": " + CifruN(text, -n));
            }
        }
    }
}
