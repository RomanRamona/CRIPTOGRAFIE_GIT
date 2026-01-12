using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CezarCipher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Introduceti textul:");
            string text = Console.ReadLine();

            string criptat = Criptare(text);
            Console.WriteLine("\nText criptat:");
            Console.WriteLine(criptat);

            string decriptat = Decriptare(criptat);
            Console.WriteLine("\nText decriptat:");
            Console.WriteLine(decriptat);

            Criptanaliza(criptat);
        }
        static string CezarCipher(string text, int shift)
        {
            char[] rez = text.ToCharArray();

            for (int i = 0; i < rez.Length; i++)
            {
                char c = rez[i];


                if (char.IsUpper(c))
                {
                    rez[i] = (char)('A' + (c - 'A' + shift + 26) % 26);
                }

                else if (char.IsLower(c))
                {
                    rez[i] = (char)('a' + (c - 'a' + shift + 26) % 26);
                }

            }

            return new string(rez);
        }
        static string Criptare(string text)
        {
            return CezarCipher(text, 3);
        }


        static string Decriptare(string text)
        {
            return CezarCipher(text, -3);
        }

        // criptanaliza – se incearca toate cheile posibile
        static void Criptanaliza(string text)
        {
            Console.WriteLine("\nCriptanaliza:");
            for (int i = 1; i <= 25; i++)
            {
                Console.WriteLine("Shift " + i + ": " + CezarCipher(text, -i));
            }
        }
    }
}
