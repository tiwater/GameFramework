using System;

namespace GameFramework.GameStructure.PlayerGameItems.ObjectModel
{
    /// <summary>
    /// Keep the equipment status
    /// </summary>
    [Serializable]
    public class GameItemEquipment
    {
        public enum Slot
        {
            None = 0, All = 1, Root = 2,
            Body = 10,
            Hat = 20, Coat = 21, Trousers = 21, Shoes = 22,
            LHand = 30, RHand = 31,
            LEar = 40, REar = 41
        };
        public string EquiptorId;
        public Slot EquipSlot;
        public string GameItemId;

        /// <summary>
        /// Constructor of PlayerEquipment
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="gameItemId"></param>
        public GameItemEquipment(string equiptorId, Slot slot, string gameItemId)
        {
            EquiptorId = equiptorId;
            EquipSlot = slot;
            GameItemId = gameItemId;
        }
    }
}