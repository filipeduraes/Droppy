using System.Threading.Tasks;
using IdeaToGame.PersistenceSystem;
using UnityEngine;

namespace Droppy.LevelSystem
{
    public class PersistenceLoader : MonoBehaviour
    {
        [SerializeField] private int slotToLoadFrom = 0;

        public void Load()
        {
            Task.Run(async () => await Persistence.LoadFromSlotAsync(slotToLoadFrom));
        }
    }
}