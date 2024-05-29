using System;
using System.Numerics;

namespace Script.Data
{
    public class DataBank: IDataBank
    {
        public BigInteger Clicks { get; set; }
        public BigInteger Multiplier { get; set; } = 1;

        public static Action OnFinalGoalReached;
    }
}