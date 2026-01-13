using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoCipher
{
    internal class Program
    {
        static readonly string alfabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        static void Main(string[] args)
        {
            // Cheia de criptare (permuta alfabetul)
            string cheie = "QWERTYUIOPASDFGHJKLZXCVBNM";

            Console.WriteLine("--- Sistem Criptografic Monoalfabetic ---");
            Console.Write("Introduceti textul: ");
            string text = Console.ReadLine();

            // 1. Criptare
            string criptat = Cripteaza(text, cheie);
            Console.WriteLine("\n[1] Text Criptat:\n" + criptat);

            // 2. Decriptare Clasica (cu cheie)
            string decriptat = Decripteaza(criptat, cheie);
            Console.WriteLine("\n[2] Text Decriptat (cu cheia corecta):\n" + decriptat);

            // 3. Criptanaliza
            Console.WriteLine("\n[3] Rezultat Criptanaliza (bazat pe frecventa):");
            CriptanalizaFrecventa(criptat);

            Console.ReadKey();
        }

        static string Cripteaza(string text, string p)
        {
            StringBuilder rez = new StringBuilder();
            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char upperC = char.ToUpper(c);
                    int index = alfabet.IndexOf(upperC);
                    char corespondent = p[index];
                    rez.Append(char.IsLower(c) ? char.ToLower(corespondent) : corespondent);
                }
                else rez.Append(c);
            }
            return rez.ToString();
        }

        static string Decripteaza(string text, string p)
        {
            StringBuilder rez = new StringBuilder();
            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char upperC = char.ToUpper(c);
                    int index = p.IndexOf(upperC);
                    char corespondent = alfabet[index];
                    rez.Append(char.IsLower(c) ? char.ToLower(corespondent) : corespondent);
                }
                else rez.Append(c);
            }
            return rez.ToString();
        }

        static void CriptanalizaFrecventa(string text)
        {
            // Statistici oficiale frecventa limba romana (E e cea mai deasa)
            char[] frecventeRo = { 'E', 'A', 'I', 'R', 'N', 'U', 'T', 'L', 'O', 'S', 'C', 'P', 'M', 'D', 'G', 'F', 'V', 'B', 'H', 'J', 'X', 'K', 'Q', 'W', 'Y', 'Z' };

            // 1. Calcul frecventa in textul primit
            var dictFrecventa = text.ToUpper()
                .Where(char.IsLetter)
                .GroupBy(c => c)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .ToList();

            // 2. Mapare
            Dictionary<char, char> harta = new Dictionary<char, char>();
            for (int i = 0; i < dictFrecventa.Count && i < frecventeRo.Length; i++)
            {
                harta[dictFrecventa[i]] = frecventeRo[i];
            }

            // 3. Reconstructie text (folosim litere mici pentru a marca predictiile)
            StringBuilder rezultat = new StringBuilder();
            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char upperC = char.ToUpper(c);
                    if (harta.ContainsKey(upperC))
                        rezultat.Append(char.ToLower(harta[upperC]));
                    else
                        rezultat.Append('?');
                }
                else rezultat.Append(c);
            }

            Console.WriteLine(rezultat.ToString());
            Console.WriteLine("\nNota: Literele mici reprezinta incercari de ghicire bazate pe statistica.");
        }
    }
}