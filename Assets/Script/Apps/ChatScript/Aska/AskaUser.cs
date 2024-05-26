using Script.Data;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

namespace Script.Apps.ChatScript.Aska
{
    [System.Serializable]
    public class AskaUser
    {
        [field:SerializeField] public ChatCharacter CharacterType { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Status { get; private set; }
        [field: SerializeField] public Sprite ProfilePicture { get; private set; }
    }
}