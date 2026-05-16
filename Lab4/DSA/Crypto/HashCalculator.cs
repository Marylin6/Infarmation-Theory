using System.Numerics;

namespace DSA
{
    internal class HashCalculator
    {
        public static BigInteger CalculateHash(string text, BigInteger q)
        {
            BigInteger h = 100;

            foreach (char ch in text)
            {
                int m = GetValue(ch);

                h = ((h + m) * (h + m)) % q;
            }

            return h;
        }

        private static int GetValue(char ch)
        {
            if (ch >= 'А' && ch <= 'Я')
                return ch - 'А' + 1;

            if (ch >= 'а' && ch <= 'я')
                return ch - 'а' + 1;

            if (ch == 'Ё' || ch == 'ё')
                return 7; 

            if (ch <= 127)
                return (int)ch;

            return (int)ch;
        }
    }
}