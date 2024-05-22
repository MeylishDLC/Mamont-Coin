using UnityEngine;

namespace Script.Data
{
    [System.Serializable]
    public class RankNamePair
    {
        [field:SerializeField] public string Name { get; private set; }
        [field:SerializeField] public Sprite NamePicture { get; private set; }
    }
}