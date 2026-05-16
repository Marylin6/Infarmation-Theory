using DSA;
using System.Numerics;
using System.Windows;

namespace DSA
{
    internal class Validator
    {
        public static bool ValidateParameters(
            BigInteger p,
            BigInteger q,
            BigInteger h,
            BigInteger x)
        {
            if (!MathUtils.IsPrime(p))
            {
                MessageBox.Show("p must be prime");
                return false;
            }

            if (!MathUtils.IsPrime(q))
            {
                MessageBox.Show("q must be prime");
                return false;
            }

            if ((p - 1) % q != 0)
            {
                MessageBox.Show("q must divide (p - 1)");
                return false;
            }

            if (h <= 1 || h >= p - 1)
            {
                MessageBox.Show("h must be in range (1, p - 1)");
                return false;
            }

            if (x <= 0 || x >= q)
            {
                MessageBox.Show("x must be in range (0, q)");
                return false;
            }

            return true;
        }

        public static bool ValidateK(BigInteger k, BigInteger q)
        {
            if (k <= 0 || k >= q)
            {
                MessageBox.Show("k must be in range (0, q)");
                return false;
            }

            return true;
        }
    }
}