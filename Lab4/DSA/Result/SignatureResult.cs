using System.Numerics;

namespace DSA
{
    internal class SignatureResult
    {
        public BigInteger R { get; set; }
        public BigInteger S { get; set; }
        public BigInteger Hash { get; set; }
    }
}