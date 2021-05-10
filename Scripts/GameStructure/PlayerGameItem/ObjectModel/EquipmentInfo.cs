using System;
using GameFramework.GameStructure.GameItems.ObjectModel;
using static GameFramework.GameStructure.PlayerGameItems.ObjectModel.GameItemEquipment;

namespace GameFramework.GameStructure.PlayerGameItems.ObjectModel
{
    /// <summary>
    /// The equipment meta info
    /// </summary>
    [Serializable]
    public class EquipmentInfo
    {
        public Slot slot;
        public GameItem equipment;
    }
}
