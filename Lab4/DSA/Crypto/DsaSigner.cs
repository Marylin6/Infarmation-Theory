using DSA;
using System.Numerics;

namespace DSA
{
    internal class DsaSigner
    {
        public static BigInteger CalculateG(
            BigInteger p,
            BigInteger q,
            BigInteger h)
        {
            return MathUtils.ModPow(
                h,
                (p - 1) / q,
                p
            );
        }

        public static SignatureResult Sign(
            string data,
            BigInteger p,
            BigInteger q,
            BigInteger g,
            BigInteger x,
            BigInteger k)
        {
            BigInteger hash = HashCalculator.CalculateHash(data, q);
            BigInteger r = MathUtils.ModPow(g, k, p) % q;
            BigInteger kInv = MathUtils.ModInverseFermat(k, q);
            BigInteger s = (kInv * (hash + x * r)) % q;

            return new SignatureResult
            {
                R = r,
                S = s,
                Hash = hash
            };
        }
    }
}