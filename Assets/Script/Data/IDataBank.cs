using System.Numerics;

namespace Script.Data
{
    public interface IDataBank
    {
        public BigInteger Clicks { get; set; }
        public BigInteger Multiplier { get; set; }
    }
}