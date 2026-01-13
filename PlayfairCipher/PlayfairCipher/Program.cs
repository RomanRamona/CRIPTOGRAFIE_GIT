using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayfairCipher
{
    internal class Program
    {
        static char[,] tabla = new char[5, 5];
        static string alfabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
        static void Main(string[] args)
        {
            Console.Write("Text original: ");
            string textOriginal = Console.ReadLine().ToUpper();

            Console.Write("Cheia: ");
            string cheia = Console.ReadLine().ToUpper();

            char[,] tabla = GenereazaTabla(cheia);

            string textCriptat = Cripteaza(textOriginal, tabla);
            string textDecriptat = Decripteaza(textCriptat, tabla);

            Console.WriteLine("\nTabla Playfair:");
            AfiseazaTabla(tabla);

            Console.WriteLine("\nText criptat: " + textCriptat);
            Console.WriteLine("Text decriptat: " + textDecriptat);
        }
        static char[,] GenereazaTabla(string cheia)
        {
            string alfabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ"; // J = I
            StringBuilder sb = new StringBuilder();

            // eliminăm duplicatele din cheie și J -> I
            foreach (char c in cheia)
            {
                char ch = (c == 'J') ? 'I' : c;
                if (char.IsLetter(ch) && !sb.ToString().Contains(ch))
                    sb.Append(ch);
            }

            // completăm cu restul alfabetului
            foreach (char c in alfabet)
            {
                if (!sb.ToString().Contains(c))
                    sb.Append(c);
            }

            // creare tabla 5x5
            char[,] tabla = new char[5, 5];
            int k = 0;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    tabla[i, j] = sb[k++];

            return tabla;
        }

        // 🔹 Criptare
        static string Cripteaza(string text, char[,] tabla)
        {
            text = text.ToUpper().Replace("J", "I");
            text = text.Replace(" ", "");
            text = PregatesteDigrame(text);

            StringBuilder rezultat = new StringBuilder();
            for (int i = 0; i < text.Length; i += 2)
            {
                char a = text[i];
                char b = text[i + 1];
                rezultat.Append(EncodePair(a, b, tabla, true));
            }
            return rezultat.ToString();
        }

        // 🔹 Decriptare
        static string Decripteaza(string text, char[,] tabla)
        {
            StringBuilder rezultat = new StringBuilder();
            for (int i = 0; i < text.Length; i += 2)
            {
                char a = text[i];
                char b = text[i + 1];
                rezultat.Append(EncodePair(a, b, tabla, false));
            }
            return rezultat.ToString();
        }

        // 🔹 Pregătește digrame (X între litere identice, adaugă X la final dacă e impar)
        static string PregatesteDigrame(string text)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                sb.Append(c);
                if (i + 1 < text.Length && text[i] == text[i + 1])
                    sb.Append('X');
            }
            if (sb.Length % 2 != 0)
                sb.Append('X');
            return sb.ToString();
        }

        // 🔹 Encode/decode o pereche
        static string EncodePair(char a, char b, char[,] tabla, bool criptare)
        {
            int rowA = 0, colA = 0, rowB = 0, colB = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (tabla[i, j] == a) { rowA = i; colA = j; }
                    if (tabla[i, j] == b) { rowB = i; colB = j; }
                }
            }

            if (rowA == rowB)
            {
                colA = (colA + (criptare ? 1 : 4)) % 5;
                colB = (colB + (criptare ? 1 : 4)) % 5;
            }
            else if (colA == colB)
            {
                rowA = (rowA + (criptare ? 1 : 4)) % 5;
                rowB = (rowB + (criptare ? 1 : 4)) % 5;
            }
            else
            {
                int temp = colA;
                colA = colB;
                colB = temp;
            }

            return "" + tabla[rowA, colA] + tabla[rowB, colB];
        }

        // 🔹 Afisare tabla Playfair
        static void AfiseazaTabla(char[,] tabla)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                    Console.Write(tabla[i, j] + " ");
                Console.WriteLine();
            }
        }
    }
}
