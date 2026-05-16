using System.Numerics;

namespace DSA
{
    internal class DsaVerifier
    {
        public static VerificationResult Verify(
            string data,
            BigInteger r,
            BigInteger s,
            BigInteger p,
            BigInteger q,
            BigInteger g,
            BigInteger y)
        {
            BigInteger hash = HashCalculator.CalculateHash(data, q);

            BigInteger w = MathUtils.ModInverseFermat(s, q);
            BigInteger u1 = (hash * w) % q;
            BigInteger u2 = (r * w) % q;

            BigInteger gu1 = MathUtils.ModPow(g, u1, p);
            BigInteger yu2 = MathUtils.ModPow(y, u2, p);

            BigInteger v =((gu1 * yu2) % p) % q;

            return new VerificationResult
            {
                IsValid = (v == r),

                Hash = hash,

                W = w,
                U1 = u1,
                U2 = u2,
                V = v
            };
        }
    }
}