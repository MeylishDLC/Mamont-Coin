using UnityEngine;

namespace Script.Core.Buffs
{
    public class TimeLimitedBuff: Buff
    {
        [SerializeField] protected int timeLimitMilliseconds;
    }
}