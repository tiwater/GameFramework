using System;
using GameFramework.GameStructure.GameItems.ObjectModel;
using static GameFramework.GameStructure.Model.GameItemEquipment;

namespace GameFramework.GameStructure.Model
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
