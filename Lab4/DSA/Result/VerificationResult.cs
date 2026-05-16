using System.Numerics;

namespace DSA
{
    internal class VerificationResult
    {
        public bool IsValid { get; set; }
        public BigInteger Hash { get; set; }
        public BigInteger W { get; set; }
        public BigInteger U1 { get; set; }
        public BigInteger U2 { get; set; }
        public BigInteger V { get; set; }
    }
}