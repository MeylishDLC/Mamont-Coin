﻿using Cysharp.Threading.Tasks;
using Script.Data;
using Script.Managers;
using Script.Sound;
using UnityEngine;

namespace Script.Core.Buffs
{
    public class AddMultiplierBuffTimeLimited: AddMultiplierBuff
    {
        [SerializeField] private int timeLimitMilliseconds;

        public override void BuyBuff()
        {
            BuyAddMultiplierLimitedAsync().Forget();
        }
        private async UniTask BuyAddMultiplierLimitedAsync()
        {
            base.BuyBuff();
            await UniTask.Delay(timeLimitMilliseconds);
            DataBank.Multiplier -= buffAmount;
        }
    }
}