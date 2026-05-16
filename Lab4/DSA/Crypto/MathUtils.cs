using System.Numerics;

namespace DSA
{
    internal class MathUtils
    {
        public static BigInteger ModPow(BigInteger baseVal, BigInteger exp, BigInteger mod)
        {
            BigInteger result = 1;

            baseVal %= mod;

            while (exp > 0)
            {
                if ((exp & 1) == 1)
                    result = (result * baseVal) % mod;

                baseVal = (baseVal * baseVal) % mod;
                exp >>= 1;
            }

            return result;
        }

        public static BigInteger ModInverseFermat(BigInteger value, BigInteger mod)
        {
            return ModPow(value, mod - 2, mod);
        }

        public static bool IsPrime(BigInteger n)
        {
            if (n < 2)
                return false;

            for (BigInteger i = 2; i * i <= n; i++)
            {
                if (n % i == 0)
                    return false;
            }

            return true;
        }
    }
}