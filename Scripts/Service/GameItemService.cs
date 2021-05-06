using System.Collections.Generic;
using GameFramework.GameStructure.GameItems.ObjectModel;

namespace GameFramework.Service
{
    public class GameItemService
    {
        public GameItemService()
        {
        }

        public IList<T> GetGameItems<T>() where T : GameItem {
            return null;
        }
    }
}