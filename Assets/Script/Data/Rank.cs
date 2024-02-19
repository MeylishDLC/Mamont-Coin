using UnityEngine;
using UnityEngine.Events;

namespace Script.Data
{
    [System.Serializable]
    public class Rank
    {
        [field:SerializeField] public string RankName { get; private set; }
        [field:SerializeField] public int RankGoal { get; private set; }
    }
}