using UnityEngine;

namespace Droppy.UI.ViewModel
{
    [CreateAssetMenu(fileName = "LevelIntroduction", menuName = "Droppy/UI/Level Introduction", order = 0)]
    public class LevelIntroductionData : ScriptableObject
    {
        [SerializeField] private string title;
        [SerializeField, TextArea] private string description;
        [SerializeField] private Sprite image;

        public string Title => title;
        public string Description => description;
        public Sprite Image => image;
    }
}